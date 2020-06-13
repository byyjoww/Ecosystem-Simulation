using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BossState { IDLE, MOVING, SUMMONING, SPECIAL }
public class AIBoss : TransitionStateMachine<BossState>
{
    protected override BossState DefaultState => BossState.IDLE;

    #region BOSS_DETAILS
    [Header("Components")]
    [SerializeField] private Rigidbody2D body;

    [Header("State Time Limits")]
    [SerializeField] private Vector2 timeRangeOnIdle = new Vector2(2, 3);
    [SerializeField] private Vector2 timeRangeOnMoving = new Vector2(2, 3);

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Rect movementBounds;

    [Header("Summoning")]
    [SerializeField] private GameObject summonPrefab;
    [SerializeField] private Rect minionBounds;
    [SerializeField] private float timeBeforeSummon = 2;
    [SerializeField] private float timeBetweenSummons = 0.5f;
    [SerializeField] private int summonPerRound; 
    [SerializeField] private bool randomizeSpawnOffsetOrder;
    [SerializeField] private List<Vector2> spawnOffsets;

    [Header("Special Attack")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject specialEffect;
    [SerializeField] private Transform effectParent;
    [SerializeField] private float delayBetweenProjectiles = 0.5f;
    #endregion

    #region INITIALIZE
    protected override Dictionary<BossState, Action> StateDictionary { get; set; }
    protected override List<Transition> TransitionList { get; set; }

    protected override void Initialize()
    {
        StateDictionary = new Dictionary<BossState, Action>()
        {
            { BossState.IDLE,       null },
            { BossState.MOVING,     OnStateMoving },
            { BossState.SUMMONING,  OnStateSummoning },
            { BossState.SPECIAL,    OnStateSpecial }
        };
        TransitionList = new List<Transition>()
        {
            new Transition(BossState.IDLE,      BossState.MOVING,       CanIdleToMoving,        OnEnterMoving),
            new Transition(BossState.MOVING,    BossState.SUMMONING,    CanMovingToSummoning,   OnEnterSummoning),
            new Transition(BossState.MOVING,    BossState.SPECIAL,      CanMovingToSpecial,     OnEnterSpecial),
            new Transition(BossState.SUMMONING, BossState.IDLE,         CanSummoningToIdle,     OnEnterIdle),
            new Transition(BossState.SPECIAL,   BossState.IDLE,         CanSpecialToIdle,       OnEnterIdle)
        };

        timeBetweenSummonerStates = minTimeBetweenSummonerStates - 10;
    }
    #endregion

    #region TRANSITIONS
    private float timeLimitOnState;
    private float timeBetweenSummonerStates = 0f;
    private float minTimeBetweenSummonerStates = 15f;

    bool CanIdleToMoving()
    {
        return TimeOnState >= timeLimitOnState;
    }
    bool CanMovingToSummoning()
    {
        return TimeOnState >= timeLimitOnState && afterMoving == BossState.SUMMONING && timeBetweenSummonerStates >= minTimeBetweenSummonerStates;
    }
    bool CanMovingToSpecial()
    {
        return TimeOnState >= timeLimitOnState && afterMoving == BossState.SPECIAL;
    }
    bool CanSpecialToIdle()
    {
        return isSpecialComplete;
    }
    bool CanSummoningToIdle()
    {
        return minions.Count >= spawnOffsets.Count || summonedInRound >= summonPerRound;
    }
    #endregion#

    #region STATE_IDLE
    private void OnEnterIdle()
    {
        timeLimitOnState = UnityEngine.Random.Range(timeRangeOnIdle.x, timeRangeOnIdle.y);
    }
    #endregion

    #region STATE_MOVING
    private BossState afterMoving;
    private Vector2 _MDestination;

    protected void OnEnterMoving()
    {
        _MDestination = transform.position;

        timeLimitOnState = UnityEngine.Random.Range(timeRangeOnMoving.x, timeRangeOnMoving.y);

        // Here we decide what to do after the moving timer ends: either go to summoning or to special
        int bit = UnityEngine.Random.Range(0, 2);
        afterMoving = (bit == 1) ? BossState.SPECIAL : BossState.SUMMONING;
    }

    protected void OnStateMoving()
    {
        if (Vector2.Distance(body.position, _MDestination) < 0.5f)
        {
            print("Setting new destination.");
            _MDestination = new Vector2(UnityEngine.Random.Range(movementBounds.xMin, movementBounds.xMax),
                                        UnityEngine.Random.Range(movementBounds.yMin, movementBounds.yMax));
        }
        Vector2 direction = (_MDestination - body.position).normalized;
        body.MovePosition(body.position + direction * Time.deltaTime * moveSpeed);
    }
    #endregion

    #region STATE_SUMMONING
    private List<AIMinion> minions = new List<AIMinion>();
    private int summonedInRound;

    void OnEnterSummoning()
    {
        timeBetweenSummonerStates = 0;
        summonedInRound = 0;
        minions = minions.Where(x => x != null && x).ToList();
    }

    void OnStateSummoning()
    {
        int instantiatedSummons = minions.Count;

        if (TimeOnState > timeBeforeSummon)
        {
            if(TimeOnState - timeBeforeSummon > instantiatedSummons * timeBetweenSummons)
            {
                if (instantiatedSummons < spawnOffsets.Count && summonedInRound < summonPerRound)
                {
                    int index = randomizeSpawnOffsetOrder ? UnityEngine.Random.Range(0, spawnOffsets.Count) : instantiatedSummons;

                    Vector2 offset = spawnOffsets[index];
                    GameObject obj = Instantiate(summonPrefab, transform.position + (Vector3)offset, Quaternion.identity);
                    AIMinion minion = obj.GetComponent<AIMinion>();
                    minion.minionBounds = minionBounds;
                    minions.Add(minion);
                    summonedInRound++;
                }
            }
        }
    }
    #endregion

    #region STATE_SPECIAL
    bool isSpecialComplete = false;

    void OnEnterSpecial()
    {
        isSpecialComplete = false;
        StartCoroutine(CastSpecial());
    }    

    IEnumerator CastSpecial()
    {
        var damageData = GetComponent<IDamageDealer>();

        for (int i = 0; i < 1; i++)
        {
            var skill = Instantiate(specialEffect, effectParent.transform.position, Quaternion.identity);
            var orb = skill.GetComponent<Projectile>();
            orb.Damage = damageData.Damage;
            orb.SetTarget(target);
            yield return new WaitForSeconds(delayBetweenProjectiles);
        }

        isSpecialComplete = true;

        yield return null;
    }

    protected override void Update()
    {
        base.Update();
        timeBetweenSummonerStates += Time.deltaTime;
    }

    void OnStateSpecial()
    {
        //state update
    }
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0.5f, 0.2f);
        Gizmos.DrawWireCube(movementBounds.center, movementBounds.size);
        Gizmos.DrawSphere(_MDestination, 1f);

        Gizmos.color = new Color(1, 0, 0, 0.2f);
        foreach (var offset in spawnOffsets) {
            Gizmos.DrawSphere(offset + (Vector2)transform.position, 0.5f);
        }
        Gizmos.DrawWireCube(minionBounds.center, minionBounds.size);
    }
    #endregion
}

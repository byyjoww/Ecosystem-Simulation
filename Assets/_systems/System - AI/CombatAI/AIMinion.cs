using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinionStates { MOVING }
public class AIMinion : TransitionStateMachine<MinionStates>
{
    protected override MinionStates DefaultState => MinionStates.MOVING;

    #region MINION_DETAILS
    [Header("Components")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Collider2D minionCollider;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;    
    [SerializeField] private float maxDistance = 2f;    
    public Rect minionBounds;
    #endregion

    #region INITIALIZE
    protected override Dictionary<MinionStates, Action> StateDictionary { get; set; }
    protected override List<Transition> TransitionList { get; set; }

    protected override void Awake()
    {
        base.Awake();
        spawnPoint = transform.position;
    }

    protected override void Initialize()
    {
        StateDictionary = new Dictionary<MinionStates, Action>()
        {
            { MinionStates.MOVING, null }
        };
        TransitionList = new List<Transition>()
        {

        };
    }
    #endregion

    #region BEHAVIOUR
    private Vector2 spawnPoint;
    private Vector2 _MDestination;

    private void Start()
    {
        _MDestination = RandomDestination();
    }    
    
    void FixedUpdate()
    {
        Vector2 pos = body.position;

        if (Vector2.Distance(transform.position, _MDestination) < 2f)
        {
            {
                _MDestination = RandomDestination();
            }
        }
        else
        {
            Vector2 direction = (_MDestination - pos);
            Vector2 newPos = pos + direction.normalized * Time.deltaTime * moveSpeed;

            if (Physics2D.Raycast(transform.position, direction, direction.magnitude))
            {
                _MDestination = pos;
            }
            else
            {
                body.velocity = direction.normalized * moveSpeed;
            }                
        }
    }

    Vector2 RandomDestination()
    {
        Vector2 pos = (Vector2)transform.position;

        Vector2 dest = new Vector2(UnityEngine.Random.Range(minionBounds.xMin, minionBounds.xMax), UnityEngine.Random.Range(minionBounds.yMin, minionBounds.yMax));
        float distance = UnityEngine.Random.Range(0, Mathf.Min(maxDistance, (dest - pos).magnitude));
        return dest;
    }
    #endregion

    #region GIZMOS
    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _MDestination);
        Gizmos.DrawSphere(transform.position, 1f);
    }
    #endregion
}
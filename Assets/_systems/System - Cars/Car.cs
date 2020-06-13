using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour, IUpdater
{
    [Header("CAR STATS")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float breakTorque;

    [Header("CAR COMPONENTS")]
    [SerializeField] private Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody;
    [SerializeField] private Axles[] axles;
    public Axles[] Axles => axles;
    [SerializeField] private Weapon[] weapons;
    public Weapon[] Weapons => weapons;

    [Header("CAR PARTS")]    
    [SerializeField] private Engine engine;    
    [SerializeField] private Breaks breaks;
    [SerializeField] private Wheels wheels;
    [SerializeField] private WheelAnimation wheelAnimation;

    [Header("DEATH ANIMATION")]
    [SerializeField] private GameObject pfExplosion;

    public event Action OnUpdateEvent;
    public event Action OnFixedUpdateEvent;

    private void Start()
    {
        engine = new Engine(rigidbody, axles, maxSpeed, maxMotorTorque, this);
        wheels = new Wheels(rigidbody, axles, maxSteeringAngle);
        breaks = new Breaks(axles, breakTorque);

        wheelAnimation = new WheelAnimation(rigidbody, axles, this);

        var mainWeapons = weapons;
        weapons = new Weapon[mainWeapons.Length];

        for (int i = 0; i < mainWeapons.Length; i++)
        {
            var newWeapon = ScriptableObject.Instantiate(mainWeapons[i]);
            weapons[i] = newWeapon;
        }
    }
    
    private void Update()
    {        
        OnUpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdateEvent?.Invoke();
    }

    public void RaceComplete()
    {
        breaks.Break(true);
    }

    public void Die()
    {
        Debug.Log($"I'm dead!");
        Instantiate(pfExplosion, transform.position, Quaternion.identity);
        Respawner.QueueRespawn(new Respawner.RespawnData(this.gameObject, this.gameObject, transform));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
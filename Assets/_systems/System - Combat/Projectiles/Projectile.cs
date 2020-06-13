using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, IDamageDealer
{
    [SerializeField] private GameObject vfx_HitExplosion;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeed = 1000f;

    public System.Action OnTargetHit;

    public int Damage { get; set; }

    [SerializeField] List<DamageTeam> _DealsDamageTo = new List<DamageTeam>() { DamageTeam.PLAYER };
    public List<DamageTeam> DealsDamageTo => _DealsDamageTo;

    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            this.target = target;
        }
    }

    bool isAppliedInitialForce = false;
    private void FixedUpdate()
    {
        TranslateMovement();
    }

    public Vector3 direction;
    private void InitialForce()
    {
        print("initialforceApplied");
        rb.AddForce(direction * speed * 10);
        print(direction);
    }

    private void TranslateMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    //private void HomingMovement()
    //{        
    //    Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
    //    direction.Normalize();
    //    float rotateAmount = Vector3.Cross(direction, transform.up).z;
    //    rb.angularVelocity = -rotateAmount * rotateSpeed;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform != target || collision.gameObject.CompareTag("NotTarget"))
        {
            NotTargetHit();
        }
        else if (collision.transform == target)
        {
            TargetHit();
        }
    }

    protected void TargetHit()
    {
        OnTargetHit?.Invoke();
        Debug.Log(target.name + " gets hit by the projectile.");
        Instantiate(vfx_HitExplosion, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void NotTargetHit()
    {
        Debug.Log("something gets hit by the projectile.");
        Instantiate(vfx_HitExplosion, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }
}

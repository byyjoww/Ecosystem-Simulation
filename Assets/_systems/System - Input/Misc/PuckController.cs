using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuckController : MonoBehaviour
{
    [SerializeField] Rigidbody2D master;
    [SerializeField] List<Transform> slaves;
    [SerializeField] SwipeScriptableEvent OnSingleSwipe;

    [Header("Swipe:")]
    [SerializeField] float speedAfterSwipe = 8f;
    [SerializeField, Range(0,1)] float swipeEnergy = 1f;
    [Header("Movement")]
    [SerializeField] Vector2 startVelocity = new Vector2(3, 3);

    [SerializeField] List<float> delay = new List<float>() { 0.05f , 0.15f };
    [SerializeField, Range(0,1)] float lerpSpeed = 0.8f;
    [SerializeField, Range(0, 1)] float gravityScale = 0.2f;
    int DelayIndex(int slave) => (int)(delay[slave] / Time.fixedDeltaTime);
    [SerializeField] float minSpeed = 12;

    [SerializeField, HideInInspector] FIFO<Vector2> positions;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(master.position, master.position + master.velocity);
    }
    private void FixedUpdate()
    {
        positions.EnqueueDequeue(master.position);

        int slavenum = 0;
        foreach(Transform slave in slaves)
        {
            Vector2 P = positions.Peek(DelayIndex(slavenum));
            Vector2 slave_P = Vector2.Lerp(slave.position, P, lerpSpeed);
            slave.position = slave_P;
            slavenum += 1;
        }

        if (InputValidator.Instance)
        {
            if (InputValidator.Instance.ValidatePosition(master.position))
            {
                master.gravityScale = 0;
            }
            else
                master.gravityScale = gravityScale;
        }
        
        if (master.velocity.magnitude < minSpeed && master.velocity.magnitude > 0.01f)
        {
            master.velocity *= minSpeed/master.velocity.magnitude;
        }
    }

    void Start()
    {
        OnValidate();
        master.velocity = startVelocity;
        OnSingleSwipe.OnRaise += ReceiveSwipe;
    }

    private void OnDestroy()
    {
        OnSingleSwipe.OnRaise -= ReceiveSwipe;
    }

    void OnValidate()
    {
        if (master == null)
        {
            master = GetComponent<Rigidbody2D>();
        }

        if (positions == null)
        {
             positions = new FIFO<Vector2>();
        }

        int sum = 0;
        for (int i = 0; i < delay.Count; i++)
            sum += DelayIndex(i);

        positions.Resize(sum + 1, master.position);
    }

    public void ResetPosition(Vector2 position)
    {
        master.position = position;
        master.velocity = Vector2.zero;

        positions = new FIFO<Vector2>();
        int sum = 0;
        for (int i = 0; i < delay.Count; i++)
            sum += DelayIndex(i);
        positions.Resize(sum + 1, master.position);
    }

    void ReceiveSwipe(Vector2 position, Vector2 direction, SwipeInputController.Quadrant quadrant)
    {
        if (InputValidator.Instance.ValidatePosition(master.position))
        {
            direction.Normalize();
            float dot = Vector2.Dot(direction, master.velocity);

            master.velocity -= 2 * dot * direction;
            master.velocity = (1 - swipeEnergy) * master.velocity + swipeEnergy * direction;
            // Normalize Velocity
            master.velocity = speedAfterSwipe * master.velocity.normalized;
        }
    }

    public Vector2 GetPosition()
    {
        return master.position;
    }
    
    public void ReceiveDoubleSwipe(Vector2 StartPosition, Vector2 Delta, bool isValidated)
    {
    }
}

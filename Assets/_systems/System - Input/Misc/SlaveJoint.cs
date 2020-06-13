using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveJoint : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] Rigidbody2D target;

    [SerializeField, Range(0,1)] float springStrength = 0.5f;
    [SerializeField, Range(0,1)] float mimicVelocity = 0.5f;

    [SerializeField, Range(0.001f, 10)] float momentum = 1f;
    float LerpSpeed => 1 / momentum;

    [SerializeField] float delay = 1f;
    [SerializeField] FIFO<Vector2> directions = new FIFO<Vector2>();
    
    private void FixedUpdate()
    {
        Vector2 t_S = target.position - body.position;
        Vector2 t_V = target.velocity;
        
        Vector2 direction = springStrength * t_S * t_S.magnitude + mimicVelocity * t_V;

        direction = directions.EnqueueDequeue(direction);        

        body.velocity = Vector2.Lerp(body.velocity, direction, LerpSpeed * Time.fixedDeltaTime);
    }

    private void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        body = body ?? GetComponent<Rigidbody2D>();

        directions.Resize((int)(delay / Time.fixedDeltaTime), body.velocity);
    }
}
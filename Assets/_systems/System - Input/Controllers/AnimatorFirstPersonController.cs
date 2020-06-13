using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;

public class AnimatorFirstPersonController : MonoBehaviour
{
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Animator anim;
    [SerializeField] private FOVKick fovKick = new FOVKick();
    private CharacterController controller;
    private Camera cam;
    private CollisionFlags collisionFlags;
    private Rigidbody rigidbody;

    [Header("Scriptable Events")]
    [SerializeField] private FloatScriptableEvent OnHorizontalInput;
    [SerializeField] private FloatScriptableEvent OnVerticalInput;
    [SerializeField] private BoolScriptableEvent OnJump;
    [SerializeField] private BoolScriptableEvent OnSprint;
    [SerializeField] private BoolScriptableEvent OnShoot;

    [Header("Character Stats")]
    [SerializeField] private bool useFovKick = true;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float stickToGroundForce = 10f;

    public Vector3 moveDirection = Vector3.zero;
    public bool isJumping;
    public string animatorTrigger;
    public bool isShooting;

    bool isRunning { get; set; }    
    bool previouslyGrounded { get; set; }    
    bool startedMoving { get; set; }    

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        mouseLook.Init(transform, cam.transform);

        OnHorizontalInput.OnRaise += HorizontalMove;
        OnVerticalInput.OnRaise += VerticalMove;
        OnJump.OnRaise += Jump;
        OnShoot.OnRaise += Shoot;
        OnSprint.OnRaise += (sprintStatus) => isRunning = sprintStatus;
    }

    private void VerticalMove(float input)
    {
        if (isShooting)
        {
            return;
        }

        if(input > 0 && !isRunning && !isJumping)
        {
            if (animatorTrigger != "isWalking")
            {
                anim.ResetTrigger(animatorTrigger);

                anim.SetTrigger("isWalking");
                animatorTrigger = "isWalking";

                Debug.Log("changed to walk");
            }
        }
        else if (input > 0 && isRunning && !isJumping)
        {
            if (animatorTrigger != "isRunning")
            {
                anim.ResetTrigger(animatorTrigger);

                anim.SetTrigger("isRunning");
                animatorTrigger = "isRunning";

                Debug.Log("changed to run");
            }
        }
        else if(input == 0 && !isJumping)
        {
            if (animatorTrigger != "isIdle")
            {
                anim.ResetTrigger(animatorTrigger);

                anim.SetTrigger("isIdle");
                animatorTrigger = "isIdle";

                Debug.Log("changed to idle");
            }
        }
    }

    private void HorizontalMove(float input)
    {
        
    }

    private void Jump(bool jumpStatus)
    {
        if (jumpStatus)
        {
            if (isJumping)
            {
                Debug.Log("already isJumping");
                return;
            }

            Debug.Log("starting jump");

            anim.ResetTrigger(animatorTrigger);
            anim.SetTrigger("isIdle");
            animatorTrigger = "isIdle";

            moveDirection = rigidbody.velocity;
            moveDirection.y = jumpSpeed;
            isJumping = true;
        }        
    }

    private void Land()
    {
        Debug.Log("landed");
        moveDirection = Vector3.zero;
        isJumping = false;
    }

    private void CheckJumpStatus()
    {
        // If character just landed
        if (!previouslyGrounded && controller.isGrounded && isJumping)
        {
            Debug.Log("time to land");
            Land();
        }

        // If character isn't jumping
        if (!controller.isGrounded && !isJumping && previouslyGrounded)
        {
            moveDirection.y = 0f;
        }

        previouslyGrounded = controller.isGrounded;
    }

    private void CheckForJump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = -stickToGroundForce;
        }
        else
        {
            moveDirection.y += (Physics.gravity * gravityMultiplier * Time.fixedDeltaTime).y;
        }
    }

    private void Shoot(bool isShooting)
    {
        if (isShooting & !this.isShooting)
        {
            anim.ResetTrigger(animatorTrigger);
            anim.SetTrigger("isShooting");
            animatorTrigger = "isShooting";
            this.isShooting = true;
        }
        else
        {
            anim.ResetTrigger(animatorTrigger);
            anim.SetTrigger("isIdle");
            animatorTrigger = "isIdle";
            this.isShooting = false;
        }
    }

    private void CheckFOVKick()
    {
        bool waswalking = !isRunning;

        // handle speed change to give an fov kick only if the player is going to a run, is running and the fovkick is to be used
        if (!isRunning != waswalking && useFovKick && controller.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(isRunning ? fovKick.FOVKickUp() : fovKick.FOVKickDown());
        }
    }

    private void Update()
    {        
        mouseLook.LookRotation(transform, cam.transform);
        CheckJumpStatus();
    }

    private void FixedUpdate()
    {
        CheckFOVKick();
        CheckForJump();

        collisionFlags = controller.Move(moveDirection * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }

        body.AddForceAtPosition(controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}

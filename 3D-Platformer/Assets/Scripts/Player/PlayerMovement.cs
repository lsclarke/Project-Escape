using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class PlayerMovement : MonoBehaviour
{

    private PlayerInput input;

    [Header("Class Reference Variables")]
    [SerializeField] private PlayerScript pScript;
    [SerializeField] private LedgeGrab ledgeGrab;
    [SerializeField] public Rigidbody rb;

    [SerializeField] public TrailRenderer trailRenderer;

    [Header("Movement Variables")]
    public Vector3 move;



    private Vector3 moveDirection;
    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    public bool canMove;



    public float currentSpeed = 0.0f;
    public float walkSpeed = 7.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 12f;
    public float stompVelocity = 15f;
    public float airControl = 2f;
    public float accSpeed = 1f;


    public bool condition;

    public bool isRunning;
    public bool isWalking;
    public bool isJumping;
    public bool isFalling;
    public bool isStomping;


    [Header("Collision Variables")]
    public Transform groundcheck;

    public float groundcheckRadius;

    public LayerMask groundLayer;

    public bool grounded;

    public float groundDrag;





    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;

        pScript.setSPD(walkSpeed);
        pScript.setJMP(jumpForce);
        walkSpeed = pScript.getSPD();
        jumpForce = pScript.getJMP();
        isRunning = false;

        rb.freezeRotation = true;

        canMove = true;

        trailRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        grounded = isGrounded();
        accelerationRun();

    

        if (grounded)
        {
            isFalling = false;
            isStomping = false;
            isJumping = false;

            rb.drag = groundDrag;
        }
        if (!grounded)
        {
            rb.drag = 0f;
        }

        if ((0 < (currentSpeed) && (currentSpeed) < runSpeed || 0 < (currentSpeed) && (currentSpeed) < runSpeed) && grounded)
        {
            isWalking = true;
        }

        if (isJumping && !grounded)
        {
            condition = isJumping;
        }
        if (isFalling && !grounded && !isStomping)
        {
            condition = isFalling;
        }
        if (isRunning)
        {
            condition = isRunning;
        }
        if (isWalking)
        {
            condition = isWalking;
        }
        if (isStomping)
        {
            condition = isStomping;
        }


    }

    private void Update()
    {
        MyIput();
        VelocityControl();

        if (isStomping || isRunning)
        {
            trailRenderer.enabled = true;
        }
        else
        {
            trailRenderer.enabled = false;
        }

    }

    public void CameraControl(InputAction.CallbackContext context)
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector3>();

        if (!context.performed && grounded)
        {
            rb.velocity = Vector3.zero;
            
        }
        else
        {
            MovePlayer();
        }


    }

    public void Run(InputAction.CallbackContext context)
    {

        if (context.performed && grounded)
        {
            isRunning = true;
            // currentSpeed = runSpeed;
            Debug.Log("Running");
        }
        if (context.canceled)
        {
            isRunning = false;
            //currentSpeed = walkSpeed;
            Debug.Log("Walking");
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();

        if (context.performed && grounded && !isJumping)
        {

            JumpPlayer();
            // rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
            //isJumping = true;
            //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }else if (context.performed && ledgeGrab.isHanging)
        {
            rb.useGravity = true;
            ledgeGrab.isHanging = false;
            JumpPlayer();
            StartCoroutine(EnableCanMove(.25f));
        }
        else if (context.canceled && rb.velocity.y > 0f)
        {
            isJumping = false;
            isFalling = true;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * -.1f, rb.velocity.z);
        }

    }

    private IEnumerator EnableCanMove(float time)
    {
        yield return new WaitForSeconds(time);
        currentSpeed = walkSpeed;
        canMove = true;
    }

    private void JumpPlayer()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        isJumping = true;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        if (ledgeGrab.isHanging)
        {
            rb.useGravity = true;
            ledgeGrab.isHanging = false;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isJumping = true;
            rb.AddForce(transform.up * (jumpForce - .5f), ForceMode.Impulse);
        }
        else
        {
            ledgeGrab.isHanging = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isJumping = true;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void AirStomp(InputAction.CallbackContext context)
    {
        if(context.performed && !grounded && !ledgeGrab.isHanging)
        {
            isStomping = true;
            rb.velocity = new Vector3(0f, -stompVelocity, 0f);
        }

    }

    public void accelerationRun()
    {
        Debug.Log("Revving");
        if (currentSpeed < runSpeed && isRunning)
        {
            currentSpeed += accSpeed * Time.deltaTime;
        }else if (currentSpeed > walkSpeed && !isRunning)
        {
            currentSpeed -= accSpeed * Time.deltaTime;
        }
        Mathf.Clamp(currentSpeed, walkSpeed, runSpeed);
    }

    public bool isGrounded()
    {
        //Check for ground collision
        return Physics.CheckSphere(groundcheck.position, groundcheckRadius, groundLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundcheck.position, groundcheckRadius);
    }

    private void MyIput()
    {
         horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void VelocityControl()
    {
        Vector3 groundVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (groundVel.magnitude > currentSpeed)
        {
            Vector3 limitSpeed = groundVel.normalized * currentSpeed; 
            rb.velocity = new Vector3(limitSpeed.x,rb.velocity.y,limitSpeed.z);
        }
    }

    private void MovePlayer()
    {
        if (canMove)
        {
            if (!ledgeGrab.isHanging)
            {
                if (grounded)
                {
                    move = orientation.forward * verticalInput + orientation.right * horizontalInput;

                    rb.AddForce(move.normalized * currentSpeed * 5f, ForceMode.Force);


                }
                else if (!grounded)
                {
                    move = orientation.forward * verticalInput + orientation.right * horizontalInput;
                    rb.AddForce(move.normalized * currentSpeed * 5f * airControl, ForceMode.Force);
                }

                if (move != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.15f);
                }
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(-ledgeGrab.forwardHit.normal);
            }
        }
        else if (!canMove && ledgeGrab.isHanging)
        {
            move = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(move.normalized * currentSpeed * 5f, ForceMode.Force); ;
        }

    }
}

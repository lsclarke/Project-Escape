using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    //Reference Variables - Access Character Script and obtain variable data
    [Header("Refernce Settings")]
    [SerializeField]
    public CharacterScript characterScript;
    [SerializeField]
    public CharacterJumpScript jumpScript;
    [SerializeField]
    public Rigidbody rb;
    [SerializeField]
    public PlayerInput input;
    [Space(5)]
    //Movement Variables
    [Header("Speed Settings")]
    public float moveSpeed;
    private float walkSpeed = 5f, runSpeed = 8f;
    private float accSpeed = 1f;

    public enum Speed { walkSpeed, runSpeed };
    public Speed currentSpeed;
    [Space(5)]
    [Header("Movement Variables")]
    public Vector3 move;
    public Transform orientation;
    [Space(5)]
    public float horizontalInput;
    public float verticalInput;
    [Space(5)]
    //bool variables
    public bool isRunning;
    public bool isWalking;
    private bool grounded;


    private void Awake()
    {
        //Initialize Classes
        characterScript = GetComponent<CharacterScript>();
        rb = GetComponent<Rigidbody>();

        //Set Speed to Character Script speed var
        characterScript.setSpeed(walkSpeed);
        moveSpeed = characterScript.getSpeed();

        Debug.Log("MOVE SPEED: " + characterScript.getSpeed());
        isWalking = false;
        currentSpeed = Speed.walkSpeed;
        
    }

    private void FixedUpdate()
    {
        grounded = jumpScript.isGrounded();
        MovementHandler();
        AccelerationHandler();

    }

    private void VelocityHandler()
    {
        Vector3 groundVelocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);

        if(groundVelocity.magnitude > moveSpeed)
        {
            Vector3 limitVelocity = groundVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVelocity.x,rb.velocity.y,limitVelocity.z);
        }
    }

    private void MovementHandler()
    {
        VelocityHandler();

        move = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(move.normalized * moveSpeed * 5f, ForceMode.Force);
       
    }
    
    public void AccelerationHandler()
    {
        if (!moveSpeed.Equals(runSpeed) && isRunning)
        {
            moveSpeed += accSpeed * Time.deltaTime;
            if (moveSpeed > runSpeed) { moveSpeed = runSpeed; }
        }
        else if (!moveSpeed.Equals(walkSpeed) && !isRunning)
        {
            moveSpeed -= accSpeed * Time.deltaTime;
            if (moveSpeed < walkSpeed) { moveSpeed = walkSpeed; }
        }
        Mathf.Clamp(moveSpeed, walkSpeed, runSpeed);
    }
    public void Movement(InputAction.CallbackContext context) 
    {
        move = context.ReadValue<Vector2>();
        isWalking = true;
        if (context.canceled)
        {
            isWalking = false;
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            isRunning = true;
        }
        if (context.canceled)
        {
            isRunning = false;
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
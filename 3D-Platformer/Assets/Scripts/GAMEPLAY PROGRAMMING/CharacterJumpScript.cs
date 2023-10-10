using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterJumpScript : MonoBehaviour
{


    [Header("Refernce Settings")]
    [SerializeField]
    public CharacterScript characterScript;
    [SerializeField]
    public Rigidbody rb;
    [SerializeField]
    public PlayerInput input;

    //Movement Variables
    [Header("Speed Settings")]
    private float jumpForce;

    [Header("Bool Variables")]
    //bool variables
    private bool isJumping;
    private bool grounded;

    [Header("Ground Collision Settings")]
    public LayerMask groundLayer;
    public Transform groundcheckLoc;

    [Range(0.0f, 10.0f)]
    public float groundcheckRadius;


    private void Awake()
    {
        //Initialize Classes
        characterScript = GetComponent<CharacterScript>();
        rb = GetComponent<Rigidbody>();

        //Set jump to Character Script jump var
        jumpForce = characterScript.getJumpPower();

    }
    private void FixedUpdate()
    {
        grounded = isGrounded();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();

        if (context.performed && isGrounded())
        {
            //rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
        }
    }

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundcheckLoc.position, groundcheckRadius, groundLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundcheckLoc.position, groundcheckRadius);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class LedgeMovement : MonoBehaviour
{
    [Header("References")]

    public LedgeGrab ledgeScript;
    public Rigidbody rb;

    public float MoveLedgeSpeed;

    public float moveHor;
    private float moveVert;

    public bool canMoveRight;
    public bool canMoveLeft;

    private RaycastHit forwardHit;
    private RaycastHit sideHit;

    private RaycastHit rightHit;
    private RaycastHit leftHit;

    public Vector3 RlineStart;
    public Vector3 RlineEnd;

    public Vector3 LlineStart;
    public Vector3 LlineEnd;

    public float Rightlength;
    public float Rstartpoint;

    public float Leftlength;
    public float Lstartpoint;


    public LayerMask ledgemask;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (ledgeScript.isHanging)
        {
            RightLineCheck(rightHit, Rstartpoint, Rightlength);
            LeftLineCheck(leftHit, Lstartpoint, Leftlength);

           // ForwardLineCheck(forwardHit, 0, 1f);

            MoveOnLedge();
        }

    }
    

    private void MoveOnLedge()
    {
        moveHor = Input.GetAxis("Horizontal") * MoveLedgeSpeed;
        moveVert = Input.GetAxis("Horizontal");

        //rb.velocity = new Vector3(moveHor, rb.velocity.y, rb.velocity.z);

        if (canMoveRight)
        {
            if (Input.GetKey(KeyCode.D))
            {
                Debug.Log("Right");
                transform.Translate(Vector3.right * Time.deltaTime * moveHor);

            }
        }

        if (canMoveLeft)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("Left");
                transform.Translate(-Vector3.right * Time.deltaTime * -moveHor);
                //rb.velocity = new Vector3(moveHor * -1, rb.velocity.y, rb.velocity.z);
            }
        }
    }

    private void ForwardLineCheck(RaycastHit hit, float startPoint, float length)
    {
        Vector3 lineStart;
        Vector3 lineEnd;


        lineStart = new Vector3(transform.position.x, transform.position.y + startPoint, transform.position.z);
        lineEnd = (lineStart + transform.forward * length);

        Physics.Linecast(lineStart, lineEnd, out hit, ledgeScript.hangMask);
        //Debug.DrawLine(lineStart, lineEnd, Color.red);

        if (hit.collider)
        {
            transform.forward = -hit.normal;

        }
    }


    private void RightLineCheck(RaycastHit hit, float startPoint, float length)
    {

        RlineStart = (transform.position + transform.right * startPoint) + transform.right;
        //lineStart = new Vector3(transform.position.x + transform.right + startPoint, transform.position.y, transform.position.z) ;
        RlineEnd = (RlineStart + transform.forward * length);

        Physics.Linecast(RlineStart, RlineEnd, out hit, ledgeScript.hangMask);
        Debug.DrawLine(RlineStart, RlineEnd, Color.green);

        if (hit.collider)
        {
            canMoveRight = true;
            //transform.forward = -hit.normal;
        }
        else
        {
            canMoveRight = false;
        }

    }

    private void LeftLineCheck(RaycastHit hit, float startPoint, float length)
    {

        LlineStart = (transform.position - transform.right * startPoint) - transform.right;
        //lineStart = new Vector3(transform.position.x + transform.right + startPoint, transform.position.y, transform.position.z) ;
        LlineEnd = (LlineStart + transform.forward * length);

        Physics.Linecast(LlineStart, LlineEnd, out hit, ledgeScript.hangMask);
        Debug.DrawLine(LlineStart, LlineEnd, Color.green);

        if (hit.collider)
        {
            canMoveLeft = true;
            //transform.forward = -hit.normal;
        }
        else
        {
            canMoveLeft = false;
        }
    }


}

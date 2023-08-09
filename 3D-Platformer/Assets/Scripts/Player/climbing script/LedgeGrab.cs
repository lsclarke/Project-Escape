using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using static UnityEngine.Tilemaps.Tilemap;

public class LedgeGrab : MonoBehaviour
{
    [Header("References")]

    public PlayerMovement pm;

    public LedgeMovement lm;

    public Rigidbody rb;

    public float distance;

    public bool isHanging;
    public bool frontWall;
    public LayerMask hangMask;

    public float ledgeSpeed;


    public RaycastHit downHit;
    private Vector3 downLineStart;
    private Vector3 downLineEnd;

    public Vector3 hangingPos;
    public Vector3 offset;

    public RaycastHit forwardHit;
    private Vector3 lineFwdStart;
    private Vector3 lineFwdEnd;

    public float Forwardlength;
    public float Fstartpoint;

    public float Downlength;
    public float Dstartpoint;

    public float Rightlength;
    public float Rstartpoint;

    public float Leftlength;
    public float Lstartpoint;



    private void Awake()
    {

        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        // HeightLineCheck();

        if (rb.velocity.y < -0.1f)
        {
            HeightLineCheck();
        }

    }

    void Update()
    {
        

        
    }

    private void ForwardLineCheck(RaycastHit hit, float startPoint, float length)
    {
        Vector3 lineStart;
        Vector3 lineEnd;


        lineStart = new Vector3(transform.position.x, transform.position.y + startPoint, transform.position.z);
        lineEnd = (lineStart + transform.forward * length);
        
        Physics.Linecast(lineStart, lineEnd, out hit, hangMask);
        Debug.DrawLine(lineStart, lineEnd, Color.red);

        if (forwardHit.collider)
        {
            Debug.Log("FORWARD HIT TRUE: " + forwardHit.point);
        }

        LedgeGrabCheck(hit, downHit);

    }

    private void HeightLineCheck()
    {
        Vector3 lineStart;
        Vector3 lineEnd;
        lineStart = (transform.position + transform.up * Dstartpoint) + transform.forward ;
        lineEnd = new Vector3(lineStart.x, lineStart.y - Downlength, lineStart.z); /*(lineStart + transform.up * Downlength) + transform.forward;*/

        Physics.Linecast(lineStart, lineEnd, out downHit, hangMask);
        Debug.DrawLine(lineStart, lineEnd, Color.red);

        if (downHit.collider)
        {
            frontWall = true;

            if (frontWall)
            {
                ForwardLineCheck(forwardHit, Fstartpoint, Forwardlength);

                if (forwardHit.collider != null)
                {
                    Debug.Log("FORWARD HIT TRUE: " + forwardHit.point);
                }
            }

        }
        else
        {
            frontWall = false;
        }
    }

    private void LedgeGrabCheck(RaycastHit hit, RaycastHit height)
    {
        Debug.Log("FWD HIT");
        if (hit.collider != null)
        {
            isHanging = true;

            if (isHanging)
            {
                Debug.Log("Stay connected accessed 1");
                rb.useGravity = false;
                rb.velocity = Vector3.zero;

                pm.canMove = false;
                pm.currentSpeed = ledgeSpeed;

                //Hang animation

                hangingPos = new Vector3(hit.point.x, height.point.y, hit.point.z);

                offset = -hit.normal * -0.5f + transform.up * -1f;
                hangingPos += offset;


                transform.position = hangingPos;

               transform.forward = -hit.normal;


            }

        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(downHit.point, 1f);

       Gizmos.DrawSphere(forwardHit.point, 1f);
    }

}

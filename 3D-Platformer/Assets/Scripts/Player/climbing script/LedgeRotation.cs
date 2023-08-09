using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class LedgeRotation : MonoBehaviour
{
    public LayerMask hangMask;
    private RaycastHit Forhit;

    public RaycastHit FollowHit;
    public RaycastHit RightHit;


    public float startFollowPoint;
    public float startFollowLength;

    public bool canTurnRight;

    public float turnSpeed;

    public float moveHor;
    private float moveVert;

    public LedgeGrab ledge;
    public LedgeMovement ledgeM;


    //Rotation variables
    public float angleY = 90f;

    public Quaternion newRotationAngle;
    public Quaternion currentRotationAngle;

    // Start is called before the first frame update
    private void Awake()
    {
        ledge = GetComponent<LedgeGrab>();
        ledgeM = GetComponent<LedgeMovement>();
        //angleY = 0f;
        //currentRotationAngle = transform.rotation;
        //Debug.Log("Current Rotation" + currentRotationAngle);
        //newRotationAngle = Quaternion.Euler(0, angleY,0);
    }

    private void FixedUpdate()
    {
        

        if (!ledgeM.canMoveRight)
        {
            // ForwardLineCheck(Forhit, 0, 1.5f);
            //FollowHitCheck(FollowHit, startFollowPoint, startFollowLength);
            RightLineCheck(RightHit, 0, 1);
        }


        //if(ledge.isHanging){
        //    if (transform.rotation.eulerAngles.y <= 110 && transform.rotation.eulerAngles.y > 90)
        //    {
        //        Debug.Log("135 condition");
        //        transform.rotation = Quaternion.Euler(0, angleY, 0);
        //    }


        //}
        TurnLedge();

    }

    // Update is called once per frame
    void Update()
    {
        TurnLedge();
        ChangeRotation();
        Debug.Log("Current Rotation:" + currentRotationAngle);
        Debug.Log("New Rotattion: " + newRotationAngle);
    }

    private void TurnLedge()
    {
        moveHor = Input.GetAxis("Horizontal") * turnSpeed;
        moveVert = Input.GetAxis("Horizontal");

        //rb.velocity = new Vector3(moveHor, rb.velocity.y, rb.velocity.z);

        if (canTurnRight)
        {
            if (Input.GetKey(KeyCode.D) && canTurnRight)
            {
                //StartCoroutine(TurnPosition(1f,0));
                ////StartCoroutine(RightLedgeTurn(1f, 0.5f));
                ////transform.Translate(Vector3.right * Time.deltaTime * moveHor);

                //float currentRotation = transform.rotation.y;
                ////// get desired rotation
                //float yRotation = Mathf.Round(currentRotation / 90) * 90f;
                //// set rotation
                //newRotationAngle = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);

                //transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, yRotation, 0), 0.0020f);
                ChangeRotation();

            }

            
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currentRotationAngle, 0.25f);
        }

    }

    private IEnumerator TurnPosition(float time, float time2)
    {
        transform.Translate(Vector3.right * Time.deltaTime * turnSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * turnSpeed);

        yield return new WaitForSeconds(time);

    }

    private void ChangeRotation()
    {
        //if (currentRotationAngle.eulerAngles.y == currentRotationAngle.y)
        //{
        //    newRotationAngle.y += 90f;
        //    currentRotationAngle = newRotationAngle;
        //    Debug.Log("New Rotattion: " + newRotationAngle);

        //    float currentRotation = transform.localRotation.eulerAngles.y;
        //    // get desired rotation
        //    float yRotation = Mathf.Round(currentRotation / 90) * 90f;
        //    // set rotation
        //    transform.localRotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);

        //}
        //else
        //{
        //    currentRotationAngle = transform.rotation;
        //}

        transform.Translate(Vector3.right * turnSpeed * Time.deltaTime);


    }


    public void FollowHitCheck(RaycastHit hit, float startPoint, float length)
    {
        Vector3 lineStart;
        Vector3 lineEnd;


        lineStart = (transform.position + transform.forward * startPoint) + transform.right;
        lineEnd = (lineStart + transform.right * length);

        Physics.Linecast(lineStart, lineEnd, out hit, hangMask);
        Debug.DrawLine(lineStart, lineEnd, Color.blue);

        if (hit.collider)
        {
            canTurnRight = true;
            Debug.Log("HIT TRUE: " + hit.point);
            // endPos = hit.point;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, 90f, 0), 0.025f);

            //switch (this.transform.rotation.y) 
            //{
            //    case < 1:
            //        this.transform.rotation = Quaternion.Euler(0, 90f, 0);
            //        break;
            //}

        }
        else
        {
            canTurnRight = false;
        }
    }

    private void RightLineCheck(RaycastHit hit, float startPoint, float length)
    {
        Vector3 lineStart;
        Vector3 lineEnd;

        lineStart = (transform.position + transform.right * startPoint) + transform.right;
        //lineStart = new Vector3(transform.position.x + transform.right + startPoint, transform.position.y, transform.position.z) ;
        lineEnd = (lineStart + transform.forward * length);

        Physics.Linecast(lineStart, lineEnd, out hit, hangMask);
        Debug.DrawLine(lineStart, lineEnd, Color.black);

        if (!hit.collider)
        {
            FollowHitCheck(FollowHit, startFollowPoint, startFollowLength);
            //transform.forward = -hit.normal;
        }

    }

        private void ForwardLineCheck(RaycastHit hit, float startPoint, float length)
    {
        Vector3 lineStart;
        Vector3 lineEnd;


        lineStart = new Vector3(transform.position.x, transform.position.y + startPoint, transform.position.z);
        lineEnd = (lineStart + transform.forward * length);

        Physics.Linecast(lineStart, lineEnd, out hit, hangMask);
        Debug.DrawLine(lineStart, lineEnd, Color.red);

        if (hit.collider)
        {
            Debug.Log("FORWARD HIT TRUE: " + hit.point);

            if (!canTurnRight) { 
                transform.forward = -hit.normal;
            }
        }

        

    }
}

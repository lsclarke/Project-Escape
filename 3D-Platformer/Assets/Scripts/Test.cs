using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    public Transform startPos;
    
    public Transform target;

    public Transform endPos;
    public float timeCount = 1.0f;
    public float speed;
    public bool repeatable;

    float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    private LedgeMovement ledgeMove;

    private void Awake()
    {


}


private void Update()
    {
        //
        GetCenter(Vector3.right);

        //Vector3 relativePos = target.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);
        //transform.rotation = Quaternion.Lerp(transform.rotation,
        //                                         rotation, Time.deltaTime * speed);

        if (!repeatable )
        {
            float fracComplete = (Time.time - startTime) / timeCount * speed;
            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * -speed);
            transform.position += centerPoint;

            //transform.LookAt(centerPoint);
        }
        else
        {
            float fracComplete = Mathf.PingPong(Time.time - startTime, timeCount / speed);
            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * -speed);
            transform.position += centerPoint;
            transform.LookAt(centerPoint);
        }

    }

    public void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos.position + endPos.position) * .5f;
        centerPoint -= direction;
        startRelCenter = startPos.position - centerPoint;
        endRelCenter = endPos.position - centerPoint;
    }

    private void ForwardLineCheck(float startPoint, float length)
    {
        Vector3 lineStart;
        Vector3 lineEnd;
        RaycastHit hit;

        lineStart = new Vector3(transform.position.x, transform.position.y + startPoint, transform.position.z);
        lineEnd = (lineStart + transform.forward * length);

        Physics.Linecast(lineStart, lineEnd, out hit);
        Debug.DrawLine(lineStart, lineEnd, Color.red);

        //if (hit.collider)
        //{
        //    Quaternion targetRotation;

                
        //    targetRotation = Quaternion.LookRotation(hit.point - transform.position);
            

        //    //Other code 

        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        //}

        

    }

    //private void FaceRotation()
    //{
    //    Vector3 relativePos = target.position - transform.position;
    //    Quaternion rotation = Quaternion.LookRotation(relativePos);
    //    transform.rotation = Quaternion.Lerp(transform.rotation,
    //                                             rotation, Time.deltaTime * speed);
    //}
}
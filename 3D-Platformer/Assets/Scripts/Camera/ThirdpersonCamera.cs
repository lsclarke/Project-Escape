using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdpersonCamera : MonoBehaviour
{
    [Header("References")]

    public Transform Player;
    public Transform PlayerObj;
    public Transform Orientation;
    public float Ypos;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = Player.position - new Vector3(transform.position.x, Player.position.y + Ypos, transform.position.z);
        Orientation.forward = viewDir.normalized;

        float inputHor = Input.GetAxis("Horizontal");
        float inputVer = Input.GetAxis("Vertical");

        Vector3 inputDir = Orientation.forward * inputVer + Orientation.right * inputHor;

        if(inputDir != Vector3.zero)
        {
            PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

    }
}

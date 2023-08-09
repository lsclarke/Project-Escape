using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class AnimationScript : MonoBehaviour
{

    [Header("References")]

    [SerializeField] public PlayerMovement MoveScript;

    public Animator anim;

    /// Ground States [Idle = 0, Jogging = 1, Running = 2, Jumping = 3, Falling = 4]
    private enum GroundStates { Idle, Jogging, Running};

    /// Air States [Jumping = 0, Falling = 1]
    private enum AirStates { Jumping, Falling};

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Move_Animations()
    {
        GroundStates states;

        //WALK ANIMATION
        if (MoveScript.currentSpeed > 0f && MoveScript.currentSpeed <= MoveScript.walkSpeed)
        {
            //walk or jogg
            states = GroundStates.Jogging;
        }
        else if (MoveScript.currentSpeed > MoveScript.walkSpeed && MoveScript.currentSpeed >= MoveScript.runSpeed)
        {
            //running 
            states = GroundStates.Running;
        }
        else
        {
            //walk
            states = GroundStates.Idle;

        }

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            states = GroundStates.Jogging;
        }

        if (Input.GetMouseButton(0))
        {
            states = GroundStates.Running;
        }

        anim.SetInteger("GroundStates", (int)states);
    }

    // Update is called once per frame
    void Update()
    {
        Move_Animations();



    }
}

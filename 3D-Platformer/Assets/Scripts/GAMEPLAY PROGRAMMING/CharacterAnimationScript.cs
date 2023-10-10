using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovementScript;

public class CharacterAnimationScript : MonoBehaviour
{

    //Animator Variables - To access animator to control animation states
    public MovementScript movementScript;
    public CharacterJumpScript jumpScript;
    [SerializeField]
    private Animator animator;


    //Enum Variables - To control the state of animations 
    private enum AnimationStates { idle, walking, running, jumping }
    [SerializeField]
    private AnimationStates state;

    //Bool Variables - For current state conditions
    private bool isMoving;
    private bool isJumping;
    private bool isWalking;
    private bool isRunning;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        movementScript = GameObject.Find("Player").GetComponent<MovementScript>();
        jumpScript = GameObject.Find("Player").GetComponent<CharacterJumpScript>();
    }

    private void FixedUpdate()
    {
        AnimationBehaviorHandler();
    }

    private void GroundAnimationLogic()
    {
        if (jumpScript.isGrounded())
        {
            if (movementScript.isWalking)
            {
                state = AnimationStates.walking;
                if (movementScript.isRunning)
                {
                    state = AnimationStates.running;
                }
            }
            else
            {
                state = AnimationStates.idle;
            }
        }
       
    }

    private void AnimationBehaviorHandler()
    {
        GroundAnimationLogic();
        animator.SetInteger("AnimStates", (int) state);
    }
}


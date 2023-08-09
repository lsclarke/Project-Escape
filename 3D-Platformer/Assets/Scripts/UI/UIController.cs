using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [Header("References")]

    public PlayerMovement player;

    public TextMeshProUGUI CondText;
    public TextMeshProUGUI AccText;


    public void setCondText() 
    {
        if(player.condition == player.isWalking)
        {
            CondText.text = "Condition: " + "Walking";
        }
        if (player.condition == player.isRunning)
        {
            CondText.text = "Condition: " + "Running";
        }
        if (player.condition == player.isJumping)
        {
            CondText.text = "Condition: " + "Jumping";
        }
        if (player.condition == player.isFalling)
        {
            CondText.text = "Condition: " + "Falling";
        }
        if (player.condition == player.isStomping)
        {
            CondText.text = "Condition: " + "Stomping";
        }

        AccText.text = "Accel/Decel: " + player.currentSpeed;

    }

    public void setControlText() { }

    void Update()
    {
        setCondText();
        
    }

}

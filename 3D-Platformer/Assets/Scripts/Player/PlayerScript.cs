using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    #region General variables for all playable characters...
    [Header("General Player(s) Variables")]
    [SerializeField] private static float hpCount;

    [SerializeField] private float speed;

    [SerializeField] private static float manaCount;

    [SerializeField] private float atkPower;

    [SerializeField] private float defPower;

    [SerializeField] private float jumpValue;

    [SerializeField] private bool canAttack;



    #endregion


    public void setHP(float hp)
    {
        hpCount = hp;
    }

    public float getHP()
    {
        return hpCount;
    }

    public void setMana(float mana)
    {
        manaCount = mana;
    }

    public float getMana()
    {
        return manaCount;
    }

    public void setSPD(float spd)
    {
        speed = spd;
    }

    public float getSPD()
    {
        return speed;
    }

    public void setJMP(float jmp)
    {
        jumpValue = jmp;
    }

    public float getJMP()
    {
        return jumpValue;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

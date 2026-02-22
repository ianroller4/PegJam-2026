using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentGobHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public Image currentGobDisplay;
    public Sprite MagicGob;
    public Sprite DaggerGob;
    public Sprite TankGob;
    public Sprite BomberGob;

    public Sack sack;
    public Sack.ActiveGob currentGob;
    void Start()
    {
        GetCurrentGob();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCurrentGob()
    {
        currentGob = sack.activeGob;
        Debug.Log(currentGob);
        DisplayCurrentGob();
    }

    public void DisplayCurrentGob()
    {
        if(currentGob == Sack.ActiveGob.DAGGER)
        {
            currentGobDisplay.sprite = DaggerGob;
        }

        if (currentGob == Sack.ActiveGob.MAGIC)
        {
            currentGobDisplay.sprite = MagicGob;

        }

        if (currentGob == Sack.ActiveGob.TANK)
        {
            currentGobDisplay.sprite = TankGob;

        }

        if (currentGob == Sack.ActiveGob.BOMBER)
        {
            currentGobDisplay.sprite = BomberGob;

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowGob : MonoBehaviour
{
    private Sack sack;
    public CurrentGobHUD HUDGobDisplay;

    private bool throwing = false;
    private bool canceledThrow = false;

    private Gob heldGob = null;

    private PlayerController pc;

    private AudioSource audioSource;
    [SerializeField] private AudioClip throwSound;

    // Start is called before the first frame update
    void Start()
    {
        sack = GetComponent<Sack>();   
        pc = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActive();
        ThrowInput();
    }

    private void ThrowInput()
    {
        if (Input.GetMouseButton(0) && !canceledThrow)
        {
            GrabGob();
            if (Input.GetKeyDown(KeyCode.LeftShift) && heldGob != null)
            {
                sack.AddGob(heldGob);
                heldGob = null;
                canceledThrow = true;
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            canceledThrow = false;
            if (throwing)
            {
                Throw();
            }
        }
    }

    private void GrabGob()
    {
        if (heldGob == null)
        {
            heldGob = sack.GetGob();
            if (heldGob != null)
            {
                throwing = true;
                heldGob.EnterHeld();
            }
        }
    }

    private void Throw()
    {
        if (heldGob != null)
        {
            audioSource.clip = throwSound;
            audioSource.Play();
            heldGob.EnterThrown(pc.facingRight);
            heldGob = null;
            throwing = false;
        }
    }

    private void UpdateActive()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            sack.SwitchGob(true);
            HUDGobDisplay.GetCurrentGob();
        } else if (Input.GetKeyDown(KeyCode.Q))
        {
            sack.SwitchGob(false);
            HUDGobDisplay.GetCurrentGob();
        }
    }
}

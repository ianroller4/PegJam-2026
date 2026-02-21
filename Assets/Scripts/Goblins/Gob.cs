using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gob : MonoBehaviour
{
    public enum GobState
    {
        IDLE,
        HELD,
        THROWN,
        ATTACK
    }

    public GobState state = GobState.IDLE;

    public GameObject player;

    public virtual void EnterIdle() { }

    public virtual void UpdateIdle() {}

    public virtual void EnterHeld() {}

    public virtual void UpdateHeld() {}

    public virtual void EnterThrown(bool right) {}

    public virtual void UpdateThrown() {}

    public virtual void EnterAttack() {}

    public virtual void UpdateAttack() {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventsScript : MonoBehaviour
{
    PlayerMovRB movScr;



    void Awake()
    {
        movScr = GetComponentInParent<PlayerMovRB>();
    }

    public void AnimEv_TakeStep()
    {
        movScr.SetIsStepTaken(true);
    }

    public void AnimEv_SetTrueIsInAirAfterJump()
    {
        movScr.SetTrueIsInAirAfterJump();
    }
}

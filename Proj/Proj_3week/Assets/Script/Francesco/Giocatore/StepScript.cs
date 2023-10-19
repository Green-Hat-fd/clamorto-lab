using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepScript : MonoBehaviour
{
    PlayerMovRB movScr;



    void Awake()
    {
        movScr = GetComponentInParent<PlayerMovRB>();
    }

    public void TakeStep()
    {
        movScr.SetIsStepTaken(true);
    }
}

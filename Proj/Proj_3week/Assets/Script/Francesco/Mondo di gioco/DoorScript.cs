using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Collider2D doorColl;
    SpriteRenderer doorSpr;

    [SerializeField] int howManyKeys = 1;
    int keysCollected;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource openSfx;



    private void Awake()
    {
        doorColl = GetComponent<Collider2D>();
        doorSpr = GetComponent<SpriteRenderer>();


        keysCollected = 0;
    }



    void CheckDoor()
    {
        //Se ha collezionato tante chiavi quante ne servono...
        if (keysCollected >= howManyKeys)
        {
            //Toglie la porta
            doorColl.enabled = false;
            doorSpr.enabled = false;

            //Feedback
            openSfx.PlayOneShot(openSfx.clip);
        }
    }


    public void AddKeyCollected()
    {
        keysCollected++;

        CheckDoor();
    }
}

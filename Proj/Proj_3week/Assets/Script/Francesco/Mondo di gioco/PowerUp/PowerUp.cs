using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerUp : MonoBehaviour
{
    Collider2D collid;
    SpriteRenderer powerUpSpr;

    [SerializeField] AudioSource pickUpSfx;



    private void Awake()
    {
        collid = GetComponent<Collider2D>();
        powerUpSpr = GetComponentInChildren<SpriteRenderer>();

        collid.isTrigger = true;
    }


    protected void DisablePowerUp()
    {
        collid.enabled = false;
        powerUpSpr.enabled = false;

        //Feedback
        pickUpSfx.PlayOneShot(pickUpSfx.clip);
    }
}

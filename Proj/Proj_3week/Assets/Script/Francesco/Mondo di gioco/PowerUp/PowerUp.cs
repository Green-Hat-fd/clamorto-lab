using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerUp : MonoBehaviour
{
    Collider2D collid;
    SpriteRenderer powerUpSpr;

    [SerializeField] PlayerStatsSO_Script stats_SO;
    [Min(0)]
    [SerializeField] int scoreWhenPickUp;

    [Space(10)]
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

        //Aggiunge il punteggio
        stats_SO.AddScore(scoreWhenPickUp);
    }
}

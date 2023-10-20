using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KeyScript : MonoBehaviour
{
    [SerializeField] DoorScript doorScr;

    Collider2D keyColl;
    SpriteRenderer keySpr;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource collectedSfx;



    private void Awake()
    {
        keyColl = GetComponent<Collider2D>();
        keySpr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();

        if (playerCheck != null)
        {
            //Aggiunge la chiave a quelle collezionate
            doorScr.AddKeyCollected();

            //Nasconde l'oggetto
            keyColl.enabled = false;
            keySpr.enabled = false;

            //Feedback
            collectedSfx.PlayOneShot(collectedSfx.clip);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaDistruggibile : MonoBehaviour
{
    private bool contattoConGiocatore = false;
    private float timer = 0f;
    public float tempoDistruggi = 3f;
    
    private SpriteRenderer piattafSpr;
    private Animator piattafAnim;

    [SerializeField] AudioSource suonoBreak;


    private void Awake()
    {
        piattafSpr = GetComponent<SpriteRenderer>();
        piattafAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (contattoConGiocatore)
        {
            timer += Time.deltaTime;

            if (timer >= tempoDistruggi)
            {
                // Distrugge l'oggetto dopo il tempo 3 secondi
                gameObject.SetActive(false);

                suonoBreak.Play();
            }
        }


        //Feedback
        piattafAnim.SetBool("GiocatoreSopra", contattoConGiocatore);
        piattafAnim.SetBool("PocoTempoRimasto", tempoDistruggi - timer <= 1.25f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        IPlayer playerCheck = collision.gameObject.GetComponent<IPlayer>();

        if (playerCheck != null)
        {
            contattoConGiocatore = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IPlayer playerCheck = collision.gameObject.GetComponent<IPlayer>();

        if (playerCheck != null)
        {
            contattoConGiocatore = false;

            timer = 0;
            piattafSpr.color = Color.white;
        }
    }
}
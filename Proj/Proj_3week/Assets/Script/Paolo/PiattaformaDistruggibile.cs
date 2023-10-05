using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaDistruggibile : MonoBehaviour
{
    private bool contattoConGiocatore = false;
    private float timer = 0f;
    public float tempoDistruggi = 3f;

    public AudioSource suonoBreak;

    private void Update()
    {
        if (contattoConGiocatore)
        {
            timer += Time.deltaTime;

            if (timer >= tempoDistruggi)
            {
                // Distrugge l'oggetto dopo il tempo 3? secondi
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            contattoConGiocatore = true;
            suonoBreak.Play();
        }
    }
}
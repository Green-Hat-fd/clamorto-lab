using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRegen : MonoBehaviour
{
    public AudioSource suonoAria;
    [SerializeField] private int hpRegen =+ 1 ;
   // PlayerHP playerHP;

    private void Start()
    {
       // playerHP = FindObjectOfType<PlayerHP>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            suonoAria.Play();
           // playerHP.PlayerHealth += hpRegen;
            StartCoroutine(DestroyAfterDelay(1f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
} 


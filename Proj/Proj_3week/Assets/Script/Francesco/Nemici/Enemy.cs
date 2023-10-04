using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] int maxHealth = 2;
    int health = 0;

    [SerializeField] PlayerStatsSO_Script stats_SO;
    [Min(0)] public int scoreAtDeath;



    void Awake()
    {
        health = maxHealth;    //Reset della vita
    }

    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collObj = collision.gameObject;
        IPlayer playerCheck = collObj.GetComponent<IPlayer>();

        if (playerCheck != null)    //Se ha colliso con il giocatore
        {
            //Danneggia il giocatore
            collObj.GetComponent<PlayerStatsManager>().Pl_TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.gameObject.GetComponent<IPlayer>();

        if (playerCheck != null)    //Se il giocatore è entrato nel trigger
        {
            //Danneggia il nemico
            En_TakeDamage();
        }
    }



    public void En_TakeDamage()
    {
        if (health - 1 >= 0)    //Se ha ancora punti vita...
        {
            health--;
        }

        En_CheckDeath();
    }

    public void En_CheckDeath()
    {
        bool idDead = health <= 0;

        if (idDead)   //Se viene ucciso
        {
            //TODO

            stats_SO.AddScore(scoreAtDeath);

            gameObject.SetActive(false);
        }
    }
}

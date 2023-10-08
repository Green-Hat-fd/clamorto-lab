using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IEnemy
{
    [Min(1)]
    [SerializeField] protected int maxHealth = 2;
    protected int health = 0;

    [Space(10)]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [Min(0)]
    [SerializeField] int scoreAtDeath;



    void Start()
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
            En_TakeDamage(int.MaxValue);
        }
    }



    public void En_TakeDamage(int damage)
    {
        if (health > 0)    //Se ha ancora punti vita...
        {
            health -= damage;
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

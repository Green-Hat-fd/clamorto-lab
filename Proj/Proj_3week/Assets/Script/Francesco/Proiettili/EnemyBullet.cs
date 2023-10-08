using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatsManager playerCheck = collision.GetComponent<PlayerStatsManager>();

        if (playerCheck != null)    //Se colpisce il giocatore
        {
            //Lo danneggia
            playerCheck.Pl_TakeDamage();
        }

        if (collision.GetComponent<IEnemy>() == null)
        {
            //Toglie il proiettile
            //(se non ha colpito un nemico)
            RemoveBullet();
        }
    }
}

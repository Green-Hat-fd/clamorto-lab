using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [Space(20)]
    [Min(1)]
    [SerializeField] int bulletDamage = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyCheck = collision.GetComponent<Enemy>();

        if (enemyCheck != null)    //Se colpisce il nemico
        {
            //Lo danneggia
            enemyCheck.En_TakeDamage(bulletDamage);
        }

        if (collision.GetComponent<IPlayer>() == null)
        {
            //Toglie il proiettile
            //(se non ha colpito il giocatore)
            RemoveBullet();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [SerializeField] PlayerStatsSO_Script stats_SO;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyCheck = collision.GetComponent<Enemy>();

        if (enemyCheck != null)    //Se colpisce il nemico
        {
            //Lo danneggia
            enemyCheck.En_TakeDamage(stats_SO.GetBulletDamage());
        }
    }
}

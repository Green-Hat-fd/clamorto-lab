using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        //print(collision.name);

        //if (collision.GetComponent<IPlayer>() == null
        //    &&
        //    collision.GetComponent<BossBullet>() == null)
        //{
        //    print("TOLTO");
        //    //Toglie il proiettile
        //    //(se non ha colpito il giocatore)
        //    RemoveBullet();
        //}

        if (collision.GetComponent<TilemapCollider2D>()
            ||
            collision.name == "Tilemap")
        {
            print(collision.name);
            //Toglie il proiettile
            //(se ha colpito la tilemap)
            RemoveBullet();
        }
    }
}

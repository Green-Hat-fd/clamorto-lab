using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAmmoScript : PowerUp
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();

        if (playerCheck != null)
        {
            //Aggiunge un punto vita al giocatore
            ShootScript shootScr = collision.GetComponent<ShootScript>();

            shootScr.SetInfiniteAmmo(true);


            //Fa sparire il power-up
            DisablePowerUp();
        }
    }
}

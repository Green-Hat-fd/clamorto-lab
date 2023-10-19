using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_AddHealth : PowerUp
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();

        if (playerCheck != null)
        {
            //Aggiunge un punto vita al giocatore
            PowerUpManager powUpMng = collision.GetComponentInChildren<PowerUpManager>();

            bool hasPickUp = powUpMng.AddHealth_PowerUp();


            if (hasPickUp)
            {
                //Fa sparire il power-up
                DisablePowerUp();
            }
        }
    }
}

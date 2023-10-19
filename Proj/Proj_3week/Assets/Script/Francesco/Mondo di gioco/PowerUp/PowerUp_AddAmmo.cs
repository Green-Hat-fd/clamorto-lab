using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_AddAmmo : PowerUp
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();

        if (playerCheck != null)
        {
            //Ricarica completamente le munizioni
            PowerUpManager powUpMng = collision.GetComponentInChildren<PowerUpManager>();

            bool hasPickUp = powUpMng.RechargeAmmo_PowerUp();


            if (hasPickUp)
            {
                //Fa sparire il power-up
                DisablePowerUp();
            }
        }
    }
}

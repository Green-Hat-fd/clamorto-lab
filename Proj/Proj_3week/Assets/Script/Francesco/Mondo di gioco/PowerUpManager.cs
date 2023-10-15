using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;

    [Space(10)]
    [SerializeField] PlayerStatsManager statsMng;
    [SerializeField] ShootScript shootScr;

    UnityEvent evAtPowerUpEnd;

    bool hasPowerUpActive;



    IEnumerator StartPowerUp(float secToWait)
    {
        //Blocca il poter prendere un nuovo power-up a tempo
        hasPowerUpActive = true;


        yield return new WaitForSeconds(secToWait);


        evAtPowerUpEnd.Invoke();                //Attiva ogni funzione nell'evento
        evAtPowerUpEnd.RemoveAllListeners();    //Li toglie


        //Rende di nuovo possibile il poter prendere un power-up
        hasPowerUpActive = false;
    }


    #region Attivazione dei Power-Up

    public void ActivateShootBoost_PowerUp()
    {
        if (!hasPowerUpActive)
        {
            //Attiva il potenziamento dei proiettili
            shootScr.SetIsShootBoostActive(true);


            //Mette come evento alla fine
            evAtPowerUpEnd.AddListener(() => shootScr.SetIsShootBoostActive(false));

            //Attiva la Coroutine
            float sec = stats_SO.GetBigShoot_PowerUpDuration();
            StartCoroutine(StartPowerUp(sec));
        }
    }

    public void ActivateBonusHealth_PowerUp()
    {
        //Attiva il power-up solo se
        //NON ha il punto vita bonus attivo
        if (!statsMng.GetHasBonusHealth())
        {
            statsMng.SetHasBonusHealth(true);
        }
    }

    public void AddHealth_PowerUp()
    {
        statsMng.AddOneHealthPoint();
    }

    #endregion
}

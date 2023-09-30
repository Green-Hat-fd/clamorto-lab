using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Stats (S.O.)", fileName = "PlayerStats_SO")]
public class PlayerStatsSO_Script : ScriptableObject
{
    [SerializeField] int score;
    [SerializeField] int howManyCollectableTaken = 0;
    [Range(1, 5)]
    [SerializeField] int maxCollectableToTake = 4;

    [Header("—— Inventario ——")]
    //[SerializeField] PowerUp.PowerUpType_Enum activePowerUp;
    float powerUpDuration;

    [Header("—— Effetti ——")]
    [SerializeField] float timeSpeed_TimerPowUp = 0.5f;

    //const PowerUp.PowerUpType_Enum POW_EMPTY = PowerUp.PowerUpType_Enum._empty;



    /*/// <summary>
    /// Ritorna vero se ha messo da parte il power-up
    /// </summary>
    /// <param name="powUp"></param>
    /// <returns></returns>
    public bool PickUpPowerUp(PowerUp.PowerUpType_Enum powUp, float newDuration)
    {
        //Se non c'e' gia' un power-up messo da parte...
        if (powerUp_toUse == POW_EMPTY)
        {
            //Aggiunge il power-up a quello messo da parte
            powerUp_toUse = powUp;
            powerUpDuration = newDuration;    //...con la sua durata

            return true;
        }
        else
        {
            return false;
        }
    }//*/



    /// <summary>
    /// Ritorna il power-up da utilizzare se ce n'è uno 
    /// </summary>
    /// <returns></returns>
    /*public PowerUp.PowerUpType_Enum UsePowerUp()
    {
        if (powerUp_toUse != POW_EMPTY)
        {
            //Utilizza il power-up messo da parte
            //e lo rimuove da quello messo da parte
            PowerUp.PowerUpType_Enum _powUpToUse = powerUp_toUse;

            powerUp_toUse = POW_EMPTY;

            activePowerUp = _powUpToUse;


            return _powUpToUse;
        }
        else
        {
            return POW_EMPTY;
        }
    }//*/



    #region Funz. Set personalizzate

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void AddCollectableTaken()
    {
        if (howManyCollectableTaken + 1 <= maxCollectableToTake)
            howManyCollectableTaken++;
    }

    #endregion


    #region Funz. Get personalizzate

    public int GetScore() => score;

    public int GetHowManyCollectableTaken()
    {
        return howManyCollectableTaken;
    }
    public float GetHowManyCollectableTaken_Percent()
    {
        return (float)howManyCollectableTaken / maxCollectableToTake;
    }

    //public PowerUp.PowerUpType_Enum GetPowerToUse() => powerUp_toUse;
    //public PowerUp.PowerUpType_Enum GetActivePowerUp() => activePowerUp;

    public float GetPowerUpDuration()
    {
        return powerUpDuration;
    }

    public float GetTimeSpeed_TimerPowUp() => timeSpeed_TimerPowUp;

    #endregion


    #region Funzioni Reset

    public void ResetCollectableTaken()
    {
        howManyCollectableTaken = 0;
    }

    /*public void ResetPowerUps()
    {
        powerUp_toUse = POW_EMPTY;
        activePowerUp = POW_EMPTY;
    }

    public void ResetActivePowerUp()
    {
        activePowerUp = POW_EMPTY;
    }//*/

    public void ResetPowerUpDuration()
    {
        powerUpDuration = 0;
    }

    public void ResetScore()
    {
        score = 0;
    }

    #endregion
}

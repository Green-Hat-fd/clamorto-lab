using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Stats (S.O.)", fileName = "PlayerStats_SO")]
public class PlayerStatsSO_Script : ScriptableObject
{
    [SerializeField] int score;

    [Header("—— Livelli ——")]
    [SerializeField] Vector3 checkpointPos = Vector3.zero;
    [SerializeField] bool isLevelOneCompleted = false;
    [SerializeField] bool isLevelTwoCompleted = false;
    [SerializeField] bool isBossLevelCompleted = false;

    [Header("—— Inventario ——")]
    //[SerializeField] PowerUp.PowerUpType_Enum activePowerUp;
    float powerUpDuration;

    [Header("—— Danno ——")]
    [SerializeField] int bulletDamage = 1;



    #region Funz. Set personalizzate

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void SetCheckpointPos(Vector3 newPos)
    {
        checkpointPos = newPos;
    }

    public void SetLevelOneCompleted(bool value)
    {
        isLevelOneCompleted = value;
    }
    public void SetLevelTwoCompleted(bool value)
    {
        isLevelTwoCompleted = value;
    }
    public void SetBossLevelCompleted(bool value)
    {
        isBossLevelCompleted = value;
    }

    public void SetBulletDamage(int newDmg)
    {
        bulletDamage = newDmg;
    }

    #endregion


    #region Funz. Get personalizzate

    public int GetScore() => score;

    public Vector3 GetCheckpointPos() => checkpointPos;

    public bool GetIsLevelOneCompleted() => isLevelOneCompleted;
    public bool GetIsLevelTwoCompleted() => isLevelTwoCompleted;
    public bool GetIsBossLevelCompleted() => isBossLevelCompleted;

    //public PowerUp.PowerUpType_Enum GetPowerToUse() => powerUp_toUse;
    //public PowerUp.PowerUpType_Enum GetActivePowerUp() => activePowerUp;

    public float GetPowerUpDuration()
    {
        return powerUpDuration;
    }

    public int GetBulletDamage() => bulletDamage;

    #endregion


    #region Funzioni Reset

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

    public void ResetBulletDamage()
    {
        bulletDamage = 1;
    }

    public void ResetScore()
    {
        score = 0;
    }

    #endregion
}

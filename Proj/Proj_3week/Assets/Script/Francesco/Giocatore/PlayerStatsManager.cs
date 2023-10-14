using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour, IPlayer
{
    DeathManager deathMng;
    PlayerMovRB playerMovScr;

    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] ShootScript shootScr;

    [Space(20)]
    [Range(-100, 0)]
    [SerializeField] float yMinDeath = -10;
    [Min(1)]
    [SerializeField] int maxHealth = 3;
    int health,
        lives;
    bool canBeDamaged,
         hasBonusHealth,
         isDead;

    [Space(10)]
    [Min(0.1f)]
    [SerializeField] float invSec = 3;

    [Header("覧 Feedback 覧")]
    [SerializeField] AudioSource deathSfx;
    [SerializeField] Canvas fakeDeathCanvas,
                            deathCanvas;
    [SerializeField] Color invColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] SpriteRenderer normalSpr;
    [SerializeField] SpriteRenderer deathSpr;

    [Space(10)]
    [SerializeField] AudioSource jumpSfx;
    [SerializeField] AudioSource damageSfx;
    [SerializeField] AudioSource powUpPickUpSfx;
    [Space(5)]
    [SerializeField] AudioSource collectablePickUpSfx;

    [Header("覧 UI 覧")]
    [SerializeField] Text scoreTxt;

    [Space(10)]
    [SerializeField] Text ammoTxt;
    [SerializeField] Text maxAmmoTxt;

    [Header("覧 DEBUG 覧")]
    [SerializeField] float deathZoneSize = 15;


    #region Costanti

    //const PowerUp.PowerUpType_Enum POW_EMPTY = PowerUp.PowerUpType_Enum._empty;
    //const PowerUp.PowerUpType_Enum POW_TIMER = PowerUp.PowerUpType_Enum.Timer;
    //const PowerUp.PowerUpType_Enum POW_INVINCIBLE = PowerUp.PowerUpType_Enum.Invincible;

    #endregion




    private void Awake()
    {
        deathMng = FindObjectOfType<DeathManager>();
        playerMovScr = FindObjectOfType<PlayerMovRB>();

        ResetAllHealthVariables();
        deathCanvas.gameObject.SetActive(false);
        stats_SO.SetCheckpointPos(transform.position);

        //Reset degli sprite
        SwapToDeathSprite(false);


        //Fissa il frame-rate da raggiungere dal gioco a 60 fps
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //Muore quando supera il limite minimo sulla Y
        if (transform.position.y <= yMinDeath)
        {
            SetHealthToZero();
        }



        #region Cambiare l'HUD

        //TODO: tutto questa parte

        //Cambio del testo (punteggio)
        scoreTxt.text = "Score: " + stats_SO.GetScore();

        //Cambia la vita e la vita bonus
        //quello in uso in base a quale sia
        /*
        ChangePowerUpImage(stats_SO.GetPowerToUse(), powUpToUseImg);
        ChangePowerUpImage(stats_SO.GetActivePowerUp(), activePowUpImg);//*/

        //Cambia le munizioni e il limite massimo
        if (shootScr.GetHasInfiniteAmmo())
        {
            ammoTxt.text = shootScr.GetAmmo().ToString();
            maxAmmoTxt.text = shootScr.GetMaxAmmo().ToString();
        }
        else
        {
            //TODO: sistema le munizioni infinite
            //ammoTxt.text = shootScr.GetAmmo().ToString();
            //maxAmmoTxt.text = shootScr.GetMaxAmmo().ToString();
        }

        #endregion


        #region Feedback

        //Cambia lo sprite quando
        //pu� essere danneggiato
        normalSpr.color = canBeDamaged
                           ? Color.white
                           : invColor;

        #endregion
    }


    #region Danno e Morte

    public void SetHealthToZero()
    {
        health = 0;

        Pl_CheckDeath();
    }

    public void Pl_TakeDamage()
    {
        //Se pu� essere danneggiato...
        if (canBeDamaged)
        {
            if (hasBonusHealth)     //Prende come priorit� il punto vita bonus
            {
                hasBonusHealth = false;
            }
            else if (health - 1 >= 0)     //Se ha ancora punti vita
            {
                health--;


                #region Feedback

                damageSfx.Play();

                #endregion


                canBeDamaged = false;
                Invoke(nameof(EnableCanBeDamaged), invSec);
            }
        }

        Pl_CheckDeath();
    }

    void EnableCanBeDamaged()
    {
        canBeDamaged = true;
    }

    public void Pl_CheckDeath()
    {
        bool canDie = health <= 0;

        if (canDie && !isDead)   //Se si puo' uccidere
        {
            lives--;    //Toglie una vita

            isDead = true;


            if (lives <= 0)    //Se NON hai pi� vite
            {
                Die_RespawnFromCheckpoint();

                #region Feedback

                //TODO

                #endregion
            }
            else    //Se hai ancora altre vite
            {
                Pl_Die();

                #region Feedback

                //TODO

                #endregion
            }
        }
    }

    public void Die_RespawnFromCheckpoint()
    {
        //TODO-----
        transform.position = stats_SO.GetCheckpointPos();


        SwapToDeathSprite(true);    //Toglie lo sprite del giocatore
                                    //e mostra quello di morte

        ResetAllPowerUps();

        deathMng.ActivateScripts(false);    //Disattiva tutti gli script nella lista

        #region Feedback

        //Mostra la canvas di Game Over (per il checkpoint)
        fakeDeathCanvas.gameObject.SetActive(true);

        //Audio
        //deathMng.ActivateLevelMusic(false);    //Disattiva la musica
        deathSfx.Play();                    //Riproduce il suono di morte

        #endregion
    }

    public void Pl_Die()
    {
        //TODO-----


        SwapToDeathSprite(true);    //Toglie lo sprite del giocatore
                                    //e mostra quello di morte

        ResetAllPowerUps();

        deathMng.ActivateScripts(false);    //Disattiva tutti gli script nella lista

        #region Feedback

        //Mostra la canvas di Game Over
        deathCanvas.gameObject.SetActive(true);

        //Audio
        //deathMng.ActivateLevelMusic(false);    //Disattiva la musica
        deathSfx.Play();                    //Riproduce il suono di morte

        #endregion
    }

    public void SwapToDeathSprite(bool isDead)
    {
        normalSpr.gameObject.SetActive(!isDead);
        deathSpr.gameObject.SetActive(isDead);
    }

    #endregion


    #region Rinascita (Checkpoint + dal livello)

    public void RespawnFromCheckpoint()
    {
        deathCanvas.gameObject.SetActive(false);
        deathMng.ActivateScripts(true);

        //Rimette il giocatore nella posizione dell'ultimo checkpoint
        transform.position = stats_SO.GetCheckpointPos();
    }

    public void RespawnLevel()
    {
        //Ricarica il livello in s�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion


    #region Power-Up - 1

    //TODO
    IEnumerator ActivateSlowTimerPowUp(float powUpTime, float timeSpeed)
    {
        print("inizio SlowTime");


        //Inizio effetti
        Time.timeScale = timeSpeed;
        playerMovScr.SetPlayerSpeedMultip(2);    // funzione per raddoppiare velocita

        #region Feedback - inizio effetti

        //DeSaturateAllSprites();

        #endregion


        yield return new WaitForSeconds(powUpTime);


        //Fine effetti
        EndSlowTimerPowUp();

        #region Feedback - fine effetti

        //SaturateAllSprites();

        #endregion


        //stats_SO.ResetActivePowerUp();    //Toglie il ppower-up da quello attivo

        print("fine SlowTime");
    }


    void EndSlowTimerPowUp()
    {
        Time.timeScale = 1;
        playerMovScr.ResetPlayerSpeedMultip();    // funzione per ripristinare la velocita
    }

    #endregion



    public void SetHasBonusHealth(bool value)
    {
        hasBonusHealth = value;
    }


    public bool GetIsDead() => isDead;

    public bool GetHasBonusHealth() => hasBonusHealth;



    #region Funzioni Reset

    void ResetAllPowerUps()
    {
        StopAllCoroutines();

        //Disattiva tutti i power-up
        EndSlowTimerPowUp();
    }


    void ResetAllHealthVariables()
    {
        health = maxHealth;
        lives = 5;

        canBeDamaged = true;
        isDead = false;

        hasBonusHealth = false;
    }


    public void ResetMaxHealth()
    {
        maxHealth = 3;
    }

    public void AddOneHealthPoint()
    {
        if (health + 1 <= maxHealth)
            health++;
    }

    #endregion



    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        Vector3 deathOffset = Vector3.up * yMinDeath,
                cubeOffset = Vector3.up * (deathZoneSize / 2),
                pos_yDeath = Vector3.zero + deathOffset - cubeOffset;
        pos_yDeath.x = transform.position.x;

        //Disegna un rettangolo dove il giocatore muore
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(pos_yDeath, new Vector3(deathZoneSize * 2,
                                                    deathZoneSize ,
                                                    deathZoneSize * 2));
    }

    #endregion
}

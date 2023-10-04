using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour, IPlayer
{
    #region Classi



    #endregion

    DeathManager deathMng;
    PlayerMovRB playerMovScr;

    [SerializeField] PlayerStatsSO_Script stats_SO;

    [Space(20)]
    [Range(-100, 0)]
    [SerializeField] float yMinDeath = -10;
    [Min(1)]
    [SerializeField] int maxHealth = 3;
    int health,
        lives;
    bool canBeDamaged,
         isDead;

    [Space(10)]
    [Min(0.1f)]
    [SerializeField] float invSec = 3;
    [Min(0)]
    [SerializeField] int scoreWhenUsePowerUp;

    [Header("覧 Feedback 覧")]
    [SerializeField] AudioSource deathSfx;
    [SerializeField] Canvas fakeDeathCanvas,
                            deathCanvas;
    [SerializeField] SpriteRenderer normalSpr;
    [SerializeField] SpriteRenderer deathSpr;

    [Space(10)]
    [SerializeField] AudioSource jumpSfx;
    [SerializeField] AudioSource powUpPickUpSfx;
    [SerializeField] AudioSource powUpTimer_usedSfx;
    [SerializeField] AudioSource powUpTimer_endedSfx;
    [SerializeField] AudioSource powUpInvincibile_usedSfx;
    [SerializeField] AudioSource powUpInvincibile_endedSfx;
    [Space(5)]
    [SerializeField] AudioSource collectablePickUpSfx;

    [Header("覧 UI 覧")]
    [SerializeField] Text scoreTxt;

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

        health = maxHealth;
        canBeDamaged = true;
        isDead = false;
        deathCanvas.gameObject.SetActive(false);
        stats_SO.SetCheckpointPos(transform.position);

        //Reset degli sprite
        SwapToDeathSprite(false);


        //Fissa il frame-rate da raggiungere dal gioco a 60 fps
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //Quando devo utilizzare il powerup
        /*if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Attiva il power-up messo da parte,
            //sovrascrivendolo e passandolo in questo script
            PowerUp.PowerUpType_Enum used_PU = stats_SO.UsePowerUp();
            
            if(used_PU != POW_EMPTY)
            {
                float dur = stats_SO.GetPowerUpDuration();    //Prende la durata dell'effetto
                stats_SO.ResetPowerUpDuration();              //La toglie dallo Scrip.Obj.


                //Reset tutti gli altri power-up
                ResetAllPowerUps();


                //Attiva il corrispettivo effetto,
                //passando anche la durata del power-up
                switch (used_PU)
                {
                    case POW_TIMER:
                        float _tSpeed = stats_SO.GetTimeSpeed_TimerPowUp();

                        StartCoroutine(ActivateSlowTimerPowUp(dur, _tSpeed));
                        break;

                    case POW_INVINCIBLE:
                        StartCoroutine(ActivateInvincibilePowUp(dur));
                        break;
                }
            }
        }//*/


        //Muore quando supera il limite minimo sulla Y
        if (transform.position.y <= yMinDeath)
        {
            SetHealthToZero();
        }



        #region Cambiare l'HUD

        //Cambio del testo (punteggio)
        scoreTxt.text = "Score: " + stats_SO.GetScore();

        //Cambia la vita e la vita bonus
        //quello in uso in base a quale sia
        /*
        ChangePowerUpImage(stats_SO.GetPowerToUse(), powUpToUseImg);
        ChangePowerUpImage(stats_SO.GetActivePowerUp(), activePowUpImg);//*/

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
        //Se ha ancora punti vita
        //e pu� essere danneggiato...
        if (canBeDamaged && health - 1 >= 0)
        {
            health--;

            canBeDamaged = false;
            Invoke("EnableCanBeDamaged", invSec);
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
            }
            else    //Se hai ancora altre vite
            {
                Pl_Die();
            }
        }
    }

    public void Die_RespawnFromCheckpoint()
    {
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

        stats_SO.AddScore(scoreWhenUsePowerUp);


        //Inizio effetti
        Time.timeScale = timeSpeed;
        playerMovScr.SetPlayerSpeedMultip(2);    // funzione per raddoppiare velocita

        #region Feedback - inizio effetti

        //DeSaturateAllSprites();

        powUpTimer_usedSfx.Play();    //Audio

        #endregion


        yield return new WaitForSeconds(powUpTime);


        //Fine effetti
        EndSlowTimerPowUp();

        #region Feedback - fine effetti

        //SaturateAllSprites();

        powUpTimer_endedSfx.Play();    //Audio

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


    void ResetAllPowerUps()
    {
        StopAllCoroutines();

        //Disattiva tutti i power-up
        EndSlowTimerPowUp();
    }


    public bool GetIsDead() => isDead;



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

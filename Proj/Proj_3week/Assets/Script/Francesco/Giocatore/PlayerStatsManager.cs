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

    [Space(10)]
    [Min(0.1f)]
    [SerializeField] float secBeforeCheckpoint = 2;
    [Min(0.1f)]
    [SerializeField] float secBeforeReload = 10;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource deathSfx;
    [SerializeField] Color invColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] List<SpriteRenderer> playerSprites;
    [SerializeField] Canvas gameOverCanvas;                            //TODO: -----canvas di morte

    [Space(10)]
    [SerializeField] AudioSource jumpSfx;
    [SerializeField] AudioSource damageSfx;
    [SerializeField] AudioSource powUpPickUpSfx;
    [Space(5)]
    [SerializeField] AudioSource collectablePickUpSfx;

    [Header("—— UI ——")]
    [SerializeField] Text scoreTxt;

    [Space(10)]
    [SerializeField] Slider healthBar;
    [SerializeField] Image bonusHealthImg;
    [SerializeField] Text livesTxt;

    [Space(10)]
    [SerializeField] Slider ammoSlider;

    [Header("—— DEBUG ——")]
    [SerializeField] float deathZoneSize = 15;




    private void Awake()
    {
        deathMng = FindObjectOfType<DeathManager>();
        playerMovScr = FindObjectOfType<PlayerMovRB>();

        ResetAllHealthVariables();
        stats_SO.SetCheckpointPos(transform.position);

        //Reset degli sprite
        ActivatePlayerSprites(true);
        gameOverCanvas.gameObject.SetActive(false);

        //Reset del punteggio
        stats_SO.ResetScore();


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

        //Cambio del testo (punteggio)
        scoreTxt.text = stats_SO.GetScore() + "";

        //Cambia la barra della vita (health)
        healthBar.value = (float)health / maxHealth;

        //Rende visibile la vita bonus
        //se ha preso il potenziamento
        bonusHealthImg.enabled = hasBonusHealth;


        //Cambia il testo delle vite (lives)
        livesTxt.text = "x" + lives;


        //Cambia le munizioni e il limite massimo
        //(se ha munizioni infinite, è sempre max)
        ammoSlider.value = !shootScr.GetHasInfiniteAmmo()
                             ? (float)shootScr.GetAmmo() / shootScr.GetMaxAmmo()
                             : 1;

        #endregion



        #region Feedback

        //Cambia lo sprite quando
        //può essere danneggiato
        foreach (SpriteRenderer spr in playerSprites)
        {
            spr.color = canBeDamaged
                         ? Color.white
                         : invColor;
        }

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
        //Se può essere danneggiato...
        if (canBeDamaged)
        {
            if (hasBonusHealth)     //Prende come priorità il punto vita bonus
            {
                hasBonusHealth = false;
            }
            else if (health - 1 >= 0)     //Se ha ancora punti vita
            {
                health--;


                #region Feedback

                damageSfx.pitch = Random.Range(0.8f, 1.75f);
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


            ResetAllPowerUps();


            if (lives <= 0)    //Se NON hai più vite
            {
                Pl_Die();

                #region Feedback

                //TODO

                #endregion
            }
            else    //Se hai ancora altre vite
            {
                Die_RespawnFromCheckpoint();

                #region Feedback

                //TODO

                #endregion
            }
        }
    }

    public void Die_RespawnFromCheckpoint()
    {
        //Ricarica l'ultimo checkpoint
        Invoke(nameof(Respawn), secBeforeCheckpoint);

        //Nasconde lo sprite del
        //giocatore e lo blocca
        ActivatePlayerSprites(false);
        playerMovScr.GetRB().bodyType = RigidbodyType2D.Static;


        #region Feedback

        //Audio
        //deathMng.ActivateLevelMusic(false);    //Disattiva la musica
        deathSfx.Play();                    //Riproduce il suono di morte

        #endregion
    }

    public void Pl_Die()
    {
        //Ricarica l'intero livello
        Invoke(nameof(ReloadScene), secBeforeReload);

        //Nasconde lo sprite del giocatore
        ActivatePlayerSprites(false);

        //Mostra la schermata di Game Over
        gameOverCanvas.gameObject.SetActive(true);


        #region Feedback

        //Audio
        //deathMng.ActivateLevelMusic(false);    //Disattiva la musica
        deathSfx.Play();                    //Riproduce il suono di morte

        #endregion
    }


    void ActivatePlayerSprites(bool value)
    {
        foreach (SpriteRenderer spr in playerSprites)
        {
            spr.gameObject.SetActive(value);
        }
    }

    void Respawn()
    {
        //Mostra il giocatore
        ActivatePlayerSprites(true);

        //Riporta il giocatore al checkpoint
        transform.position = stats_SO.GetCheckpointPos();

        //Reset delle variabili della vita
        ResetHealthVariables_Respawn();

        //Fa muovere il giocatore
        playerMovScr.GetRB().bodyType = RigidbodyType2D.Dynamic;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion


    #region Rinascita (Checkpoint + dal livello)

    public void RespawnFromCheckpoint()
    {
        deathMng.ActivateScripts(true);

        //Rimette il giocatore nella posizione dell'ultimo checkpoint
        transform.position = stats_SO.GetCheckpointPos();
    }

    public void RespawnLevel()
    {
        //Ricarica il livello in sè
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        SetHasBonusHealth(false);

        shootScr.SetIsShootBoostActive(false);
    }


    void ResetAllHealthVariables()
    {
        health = maxHealth;
        lives = 5;

        canBeDamaged = true;
        isDead = false;

        hasBonusHealth = false;
    }


    void ResetHealthVariables_Respawn()
    {
        health = maxHealth;

        canBeDamaged = true;
        isDead = false;

        hasBonusHealth = false;
    }


    public void ResetMaxHealth()
    {
        maxHealth = 3;
    }

    public bool AddOneHealthPoint()
    {
        if (health + 1 <= maxHealth)
        {
            health++;

            return true;
        }
        else
        {
            return false;
        }
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

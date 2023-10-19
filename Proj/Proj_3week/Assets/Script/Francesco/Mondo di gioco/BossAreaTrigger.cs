using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossAreaTrigger : MonoBehaviour
{
    [SerializeField] ConfinedCameraScript confinedCamScr;
    [SerializeField] BossScript bossScr;

    [Header("—— Zona Boss ——")]
    [SerializeField] Vector2 newBossZone_camPos;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource levelMusic;
    [SerializeField] AudioSource bossMusic;



    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    { 
        IPlayer playerCheck = collision.gameObject.GetComponent<IPlayer>();

        if (playerCheck != null)    //Se ha il giocatore è entrato nel trigger
        {
            //Imposta la variabile nello script
            //del movimento della camera
            confinedCamScr.SetIsPlayerInBossZone(true);
            confinedCamScr.SetBossZone_CamPos(newBossZone_camPos);


            //Attiva il boss
            bossScr.SetIsStopped(false);


            //Attiva la musica del boss
            //se non è ancora attiva
            if (!bossMusic.isPlaying)
            {
                bossMusic.Play();
                levelMusic.Stop();
            }
        }
    }


    #region EXTRA - Gizmos

    private void OnDrawGizmos()
    {
        float dim = 0.2f;

        //Disegna un piccolo cubo nella nuova posizione
        //della telecamera quando si scontra contro il boss
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(newBossZone_camPos, dim);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(newBossZone_camPos, dim);
    }

    private void OnDrawGizmosSelected()
    {
        //Disegna una linea che collega
        //la posizione di arrivo della telecamera
        //alla posiz. di questo script
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, newBossZone_camPos);
    }

    #endregion
}

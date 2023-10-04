using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckpointScript : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;

    [Space(10)]
    [SerializeField] Vector2 checkpointOffset = Vector2.zero;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.gameObject.GetComponent<IPlayer>();

        if (playerCheck != null)    //Se ha il giocatore è entrato nel trigger
        {
            //Imposta il nuovo checkpoint
            stats_SO.SetCheckpointPos(transform.position + (Vector3)checkpointOffset);
        }
    }



    #region EXTRA - Gizmos

    private void OnDrawGizmos()
    {
        //Disegna un rettangolo dove il giocatore muore
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position + (Vector3)checkpointOffset, Vector2.one * 0.2f);
    }

    #endregion
}

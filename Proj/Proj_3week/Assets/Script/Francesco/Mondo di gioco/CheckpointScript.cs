using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckpointScript : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;

    [Space(10)]
    [SerializeField] Vector2 checkpointOffset = Vector2.zero;

    List<CheckpointScript> allCheckPoints = default;

    [Header("—— Feedback ——")]
    [SerializeField] SpriteRenderer checkpSpr;
    [SerializeField] Sprite normalSprite,
                            activeSprite;



    private void Awake()
    {
        CheckpointScript[] allChecks = FindObjectsOfType<CheckpointScript>();
        allCheckPoints = new List<CheckpointScript>(allChecks);

        allCheckPoints.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.gameObject.GetComponent<IPlayer>();

        if (playerCheck != null)    //Se ha il giocatore è entrato nel trigger
        {
            //Imposta il nuovo checkpoint
            stats_SO.SetCheckpointPos(transform.position + (Vector3)checkpointOffset);


            #region Feedback

            //Cambia lo sprite con quello
            //attivo in questo checkpoint
            SetSprite(activeSprite);

            //Cambia lo sprite con quello
            //normale negli altri checkpoint
            foreach (var scr in allCheckPoints)
            {
                scr.SetSprite(normalSprite);
            }

            #endregion
        }
    }


    public void SetSprite(Sprite newSpr)
    {
        checkpSpr.sprite = newSpr;
    }



    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //
        checkpSpr.transform.position = transform.position
                                        + (Vector3)checkpointOffset
                                        + Vector3.up * 0.2f;
    }

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmos()
    {
        //Disegna un rettangolo nel punto di respawn del checkpoint
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position + (Vector3)checkpointOffset, Vector2.one * 0.2f);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectableScript : MonoBehaviour
{
    #region Enum
    
    enum CollectType_Enum
    {
        [InspectorName("Finale - Notte Stellata")]
        NotteStellata_Fine,
        [InspectorName("Finale - Grande Onda")]
        GrandeOnda_Fine,
        [InspectorName("Solo per Punteggio")]
        score
    }
    
    #endregion


    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] OptionsSO_Script options_SO;

    [Space(20)]
    [SerializeField] CollectType_Enum collectableType;

    [Space(20)]
    [Min(0)]
    [SerializeField] int scoreWhenCollected;

    [Header("�� Feedback ��")]
    [SerializeField] SpriteRenderer collectSpr;
    [Range(0, 5)]
    [SerializeField] float sinVel = 1;
    [Range(0, 2)]
    [SerializeField] float sinHeight = 2;
    Transform spriteTransf;
    Vector3 startPos;
    
    [Space(10)]
    [SerializeField] AudioSource smallCollectedSfx;
    [SerializeField] AudioSource bigCollectedSfx;


    #region Costanti

    const CollectType_Enum COLL_NS = CollectType_Enum.NotteStellata_Fine;
    const CollectType_Enum COLL_GO = CollectType_Enum.GrandeOnda_Fine;
    const CollectType_Enum COLL_PUNTI = CollectType_Enum.score;

    #endregion



    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        spriteTransf = collectSpr.transform;
        startPos = spriteTransf.position;
    }

    void Update()
    {
        //Muove lo sprite con un movimento ad onda seno
        float moveVal = Time.time * sinVel;
        Vector3 positionToMove = Vector3.up * Mathf.Sin(moveVal) * sinHeight;
        
        spriteTransf.position = startPos + positionToMove;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();
        AudioSource sfxToPlay;

        if (playerCheck != null)
        {
            switch (collectableType)
            {
                //---Notte Stellata---//
                case COLL_NS:
                    stats_SO.SetLevelOneCompleted(true);

                    sfxToPlay = bigCollectedSfx;

                    //Torna al menu principale
                    options_SO.OpenChooseScene(0);
                    break;
                
                //---Grande Onda---//
                case COLL_GO:
                    stats_SO.SetLevelTwoCompleted(true);

                    sfxToPlay = bigCollectedSfx;

                    //Torna al menu principale
                    options_SO.OpenChooseScene(0);
                    break;
                
                //---Solo il Punteggio---//
                default:
                case COLL_PUNTI:
                    sfxToPlay = smallCollectedSfx;
                    break;
            }

            //Aggiunge il punteggio
            stats_SO.AddScore(scoreWhenCollected);

            //Feedback
            sfxToPlay.PlayOneShot(sfxToPlay.clip);

            //Nasconde il collezionabile
            spriteTransf.gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

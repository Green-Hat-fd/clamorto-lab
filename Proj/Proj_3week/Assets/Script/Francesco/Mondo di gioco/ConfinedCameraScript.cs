using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfinedCameraScript : MonoBehaviour
{
    Transform player;

    [SerializeField] float cameraZOffset = -10;
    Vector3 realCameraZOffset;

    [Space(20)]
    [SerializeField] Vector2 xLimits_inWorld;
    [SerializeField] Vector2 yLimits_inWorld;
    Vector3 startBgSprPos,
            camStartPos,
            newPos_cam;
    Vector2 sprSize;

    [Space(20)]
    [SerializeField] SpriteRenderer backgroundSpr;

    Vector2 bossZone_camPos;
    bool isPlayerInBossZone;



    void Awake()
    {
        player = FindObjectOfType<PlayerMovRB>().transform;

        isPlayerInBossZone = false;


        //Mette lo sfondo come figlio della camera
        backgroundSpr.transform.parent = transform;

        //Prende le posizioni iniziali rispetto al mondo
        sprSize = backgroundSpr.size;
        startBgSprPos = backgroundSpr.transform.localPosition;
        camStartPos = transform.position;
        realCameraZOffset = Vector3.forward * cameraZOffset;
    }

    private void Update()
    {
        if (isPlayerInBossZone)
        {
            //Mette la posizione della telecamera in quella del boss
            //solo quando il giocatore si trova nella zona
            newPos_cam = Vector3.Lerp(newPos_cam,
                                      (Vector3)bossZone_camPos + realCameraZOffset,
                                      Time.deltaTime * 2.5f);
        }
        else
        {
            //Se no, prende la posizione del giocatore
            //(con l'offset dell'asse Z)
            //e lo limita nei confini
            newPos_cam = player.position + realCameraZOffset;

            newPos_cam.x = Mathf.Clamp(newPos_cam.x, xLimits_inWorld.x, xLimits_inWorld.y);
            newPos_cam.y = Mathf.Clamp(newPos_cam.y, yLimits_inWorld.x, yLimits_inWorld.y);
        }


        transform.position = newPos_cam;
    }

    void FixedUpdate()
    {
        //Prende la distanza tra i due limiti
        //e la posizione di mezzo
        Vector2 levelDist = new Vector2(xLimits_inWorld.x - xLimits_inWorld.y,
                                        yLimits_inWorld.x - yLimits_inWorld.y),
                halfLevelDist = new Vector2(levelDist.x / 2,
                                            levelDist.y / 2),
                halfPos = halfLevelDist + (Vector2)camStartPos;


        //Calcola la distanza del giocatore
        //rispetto all'inizio del livello
        Vector2 playerDist = new Vector2(halfPos.x - player.position.x,
                                         halfPos.y - player.position.y),
                playerDistPercent = new Vector2(playerDist.x / halfLevelDist.x,
                                                playerDist.y / halfLevelDist.y);

        playerDistPercent.x = Mathf.Clamp(playerDistPercent.x, -1, 1);
        playerDistPercent.y = Mathf.Clamp(playerDistPercent.y, -1, 1);


        //Sposta lo sfondo rispetto al giocatore
        Vector3 newPos_bgSpr = default;
        newPos_bgSpr.z = startBgSprPos.z;

        if (!isPlayerInBossZone)
        {
            newPos_bgSpr.x -= (backgroundSpr.transform.localScale.x / 2) * playerDistPercent.x;
            newPos_bgSpr.y -= (backgroundSpr.transform.localScale.y / 2) * playerDistPercent.y;
        }


        //Aggiorna la posizione dello sfondo
        //per coprire l'intero livello
        backgroundSpr.transform.localPosition = newPos_bgSpr;
    }


    public void SetIsPlayerInBossZone(bool value)
    {
        isPlayerInBossZone = value;
    }
    public bool GetIsPlayerInBossZone() => isPlayerInBossZone;

    public void SetBossZone_CamPos(Vector2 newPos)
    {
        bossZone_camPos = newPos;
    }



    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //Rende i limiti X sempre orizzontale alla cam
        //e i limiti Y sempre verticali alla cam
        float camLimit_x = transform.position.x,
              camLimit_y = transform.position.y;

        //Cambia i limiti della telecamera
        //per non superare la posizione della cam
        xLimits_inWorld.x = Mathf.Min(xLimits_inWorld.x, camLimit_x);
        xLimits_inWorld.y = Mathf.Max(camLimit_x, xLimits_inWorld.y);
        
        yLimits_inWorld.x = Mathf.Min(yLimits_inWorld.x, camLimit_y);
        yLimits_inWorld.y = Mathf.Max(camLimit_y, yLimits_inWorld.y);
    }

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmos/*Selected*/()
    {
        Vector3 _posHoriz = Vector3.zero,
                _posVert = Vector3.zero,
                _dim;
        float xDim = xLimits_inWorld.y - xLimits_inWorld.x,
              yDim = yLimits_inWorld.y - yLimits_inWorld.x;

        //Prende le dimensioni e la posizione
        //dei confini della telecamera
        _posHoriz += - Vector3.left * xLimits_inWorld.x / 2
                        + Vector3.right * xLimits_inWorld.y / 2;
        
        _posVert += - Vector3.down * yLimits_inWorld.x / 2
                        + Vector3.up * yLimits_inWorld.y / 2;
        
        _dim = Vector3.right * xDim + Vector3.up * yDim;


        //Disegna un rettangolo nel quale si muoverà la telecamera
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(_posHoriz + _posVert, _dim);


        //Disegna un piccolo cubo dove si metterà
        //la telecamera quando si scontra contro il boss
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(bossZone_camPos, Vector3.one * 0.4f);
    }

    #endregion
}

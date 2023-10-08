using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FakeParallaxEffect : MonoBehaviour
{
    #region Classi

    enum ParallaxOrientation_Enum
    {
        Horizontal,
        Vertical
    }

    #endregion

    Transform player;

    [SerializeField] float cameraZOffset = -10;

    [Space(20)]
    [SerializeField] ParallaxOrientation_Enum parallaxOrient;
    [SerializeField] Vector2 limits_inWorld;
    Vector3 parallaxAxis,
            startBgSprPos;
    Vector2 sprSize,
            levelStartPos, levelEndPos;

    [Space(20)]
    [SerializeField] SpriteRenderer backgroundSpr;
    


    void Awake()
    {
        player = FindObjectOfType<PlayerMovRB>().transform;


        //Mette lo sfondo come figlio della camera
        backgroundSpr.transform.parent = transform;

        //Prende l'asse rispetto all'orientamento
        SwitchSet(ref parallaxAxis, Vector2.right, Vector2.up);

        //Prende le posizioni iniziali rispetto al mondo
        sprSize = backgroundSpr.size;
        startBgSprPos = backgroundSpr.transform.localPosition;
        levelStartPos = parallaxAxis * limits_inWorld.x;
        levelEndPos = parallaxAxis * limits_inWorld.y;
    }

    private void Update()
    {
        Vector3 newPos_cam = player.position + Vector3.forward * cameraZOffset;


        SwitchSet(ref newPos_cam.x,
                  ref newPos_cam.y,
                  Mathf.Clamp(newPos_cam.x, limits_inWorld.x, limits_inWorld.y),
                  Mathf.Clamp(newPos_cam.y, limits_inWorld.x, limits_inWorld.y));


        transform.position = newPos_cam;
    }

    void FixedUpdate()
    {//Prende la distanza tra i due limiti
        //e la posizione di mezzo
        float levelDist = Vector2.Distance(levelEndPos, levelStartPos),
              halfLevelDist = levelDist / 2;
        Vector2 halfPos = Vector2.Lerp(levelStartPos, levelEndPos, 0.5f);
        

        //Calcola la distanza del giocatore
        //rispetto all'inizio del livello
        float playerDist = default;

        SwitchSet(ref playerDist,
                  halfPos.x - player.position.x,
                  halfPos.y - player.position.y);

        float playerDistPercent = playerDist / halfLevelDist;
        playerDistPercent = Mathf.Clamp(playerDistPercent, -1, 1);

        
        //Sposta lo sfondo rispetto al giocatore
        Vector3 newPos_bgSpr = default;
        newPos_bgSpr.z = startBgSprPos.z;

        SwitchAdd(ref newPos_bgSpr.x,
                  ref newPos_bgSpr.y,
                  (backgroundSpr.transform.localScale.x
                   / 2) * playerDistPercent,
                  (backgroundSpr.transform.localScale.y
                   / 2) * playerDistPercent);

        //Limita il movimento dello sfondo
        //ai limiti della camera
        Vector2 realSprDim = sprSize - (Vector2)backgroundSpr.transform.localScale,
                halfRealSprDim = realSprDim / 2;

        SwitchSet(ref newPos_bgSpr.x,
                  ref newPos_bgSpr.y,
                  Mathf.Clamp(newPos_bgSpr.x,
                              -(Camera.main.orthographicSize * Camera.main.aspect - halfRealSprDim.x),
                              Camera.main.orthographicSize * Camera.main.aspect - halfRealSprDim.x),
                  Mathf.Clamp(newPos_bgSpr.y,
                              -(Camera.main.orthographicSize - halfRealSprDim.y),
                              Camera.main.orthographicSize - halfRealSprDim.y)
                  );

        //Aggiorna la posizione dello sfondo
        //per coprire l'intero livello
        backgroundSpr.transform.localPosition = newPos_bgSpr;
    }


    #region Funz. Switch personalizzate

    /// <summary>
    /// Cambia la variabile "varToChange" (passata per riferimento)
    /// <br></br>rispetto all'orientamento scelto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="varToChange">La variabile da cambiare</param>
    /// <param name="newVal_Horiz">Il nuovo valore (in orizzontale)</param>
    /// <param name="newVal_Vert">Il nuovo valore (in verticale)</param>
    void SwitchSet<T>(ref T varToChange, T newVal_Horiz, T newVal_Vert)
    {
        switch (parallaxOrient)
        {
            default:
            case ParallaxOrientation_Enum.Horizontal:
                varToChange = newVal_Horiz;
                break;
            
            case ParallaxOrientation_Enum.Vertical:
                varToChange = newVal_Vert;
                break;
        }
    }
    /// <summary>
    /// Cambia la variabile passate per riferimento
    /// <br></br>- "varToChange_Horiz" con il nuovo valore in orizzontale
    /// <br></br>- "varToChange_Vert" con il nuovo valore in verticale
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="varToChange_Horiz">La variabile da cambiare (in orizz)</param>
    /// <param name="varToChange_Vert">La variabile da cambiare (in vert)</param>
    /// <param name="newVal_Horiz">Il nuovo valore (in orizzontale)</param>
    /// <param name="newVal_Vert">Il nuovo valore (in verticale)</param>
    void SwitchSet<T>(ref T varToChange_Horiz, ref T varToChange_Vert, T newVal_Horiz, T newVal_Vert)
    {
        switch (parallaxOrient)
        {
            default:
            case ParallaxOrientation_Enum.Horizontal:
                varToChange_Horiz = newVal_Horiz;
                break;
            
            case ParallaxOrientation_Enum.Vertical:
                varToChange_Vert = newVal_Vert;
                break;
        }
    }

    /// <summary>
    /// Aggiunge alla variabile passate per riferimento
    /// <br></br>- "varToChange_Horiz" al valore da agg. in orizzontale
    /// <br></br>- "varToChange_Vert" al valore da agg. in verticale
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="varToChange_Horiz">La variabile da cambiare (in orizz)</param>
    /// <param name="varToChange_Vert">La variabile da cambiare (in vert)</param>
    /// <param name="valToAdd_Horiz">Il valore da aggiungere (in orizzontale)</param>
    /// <param name="valToAdd_Vert">Il valore da aggiungere (in verticale)</param>
    void SwitchAdd(ref float varToChange_Horiz, ref float varToChange_Vert, float valToAdd_Horiz, float valToAdd_Vert)
    {
        switch (parallaxOrient)
        {
            default:
            case ParallaxOrientation_Enum.Horizontal:
                varToChange_Horiz += valToAdd_Horiz;
                break;
            
            case ParallaxOrientation_Enum.Vertical:
                varToChange_Vert += valToAdd_Vert;
                break;
        }
    }

    #endregion


    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //Rende sempre la X sempre a destra della cam
        //e la Y sempre alla sua sinistra
        float camLimit = default;

        //Cambia il limite della telecamera
        //rispetto all'orientamento scelto
        SwitchSet(ref camLimit, transform.position.x, transform.position.y);

        limits_inWorld.x = Mathf.Min(limits_inWorld.x, camLimit);
        limits_inWorld.y = Mathf.Max(camLimit, limits_inWorld.y);
    }

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmos/*Selected*/()
    {
        Vector3 _posMin = Vector3.zero,
                _posMax = Vector3.zero,
                _dim;

        //Cambia il limite della telecamera
        //rispetto all'orientamento scelto
        Vector3 _axis = default,
                _dimAxis = default;

        SwitchSet(ref _axis, transform.right, transform.up);
        SwitchSet(ref _dimAxis, transform.up, transform.right);

        _posMin += _axis * limits_inWorld.x;
        _posMax += _axis * limits_inWorld.y;
        _dim = _dimAxis * 7.5f + Vector3.forward;

        _posMin = Application.isPlaying ? (Vector3)levelStartPos : _posMin;
        _posMax = Application.isPlaying ? (Vector3)levelEndPos : _posMax;

        //Disegna due linee dove la telecamera si può muovere
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(_posMin, _dim);
        Gizmos.DrawWireCube(_posMax, _dim);

        //Disegna due sfere a metà schermo
        //in orizzontale (blu) e in verticale (rossa)
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + -transform.right * (Camera.main.orthographicSize * Camera.main.aspect * 0.5f), 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + -transform.up * (Camera.main.orthographicSize * 0.5f), 0.25f);
    }

    #endregion
}

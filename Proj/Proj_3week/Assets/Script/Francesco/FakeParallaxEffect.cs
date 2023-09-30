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

    [SerializeField] ParallaxOrientation_Enum parallaxOrient;
    [SerializeField] Vector2 limits;
    Vector3 parallaxAxis,
            startBgSprPos;
    Vector2 sprSize,
            levelStartPos, levelEndPos;

    [Space(20)]
    [SerializeField] SpriteRenderer backgroundSpr;
    
    
    float DEBUG_float;
    


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
        levelStartPos = transform.position + parallaxAxis * limits.x;
        levelEndPos = transform.position + parallaxAxis * limits.y;
        print(Camera.main.orthographicSize + " ~ " + Camera.main.orthographicSize * Screen.width / Screen.height);
    }
    
    void FixedUpdate()
    {
        Vector3 newPos_cam = player.position;


        SwitchSet(ref newPos_cam.x,
                  ref newPos_cam.y,
                  Mathf.Clamp(newPos_cam.x, limits.x, limits.y),
                  Mathf.Clamp(newPos_cam.y, limits.x, limits.y));

        transform.position = newPos_cam;


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

        
        //Aggiorna la posizione dello sprite
        //per coprire l'intero livello
        Vector3 newPos_bgSpr = default;
        newPos_bgSpr.z = startBgSprPos.z;

        SwitchAdd(ref newPos_bgSpr.x,
                  ref newPos_bgSpr.y,
                  (sprSize.x
                   * backgroundSpr.transform.localScale.x
                   * playerDistPercent) / 2,
                  (sprSize.y
                   * backgroundSpr.transform.localScale.y
                   * playerDistPercent) / 2);                           //TODO: da sistemare

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
        //e la Y sempre alla sua destra
        float camLimit = default;

        //Cambia il limite della telecamera
        //rispetto all'orientamento scelto
        SwitchSet(ref camLimit, transform.position.x, transform.position.y);

        limits.x = Mathf.Min(limits.x, camLimit);
        limits.y = Mathf.Max(camLimit, limits.y);
    }

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmos/*Selected*/()
    {
        Vector3 _posMin = transform.position,
                _posMax = transform.position,
                _dim;

        //Cambia il limite della telecamera
        //rispetto all'orientamento scelto
        Vector3 _axis = default,
                _dimAxis = default;

        SwitchSet(ref _axis, transform.right, transform.up);
        SwitchSet(ref _dimAxis, transform.up, transform.right);

        _posMin += _axis * limits.x;
        _posMax += _axis * limits.y;
        _dim = _dimAxis * 7.5f + Vector3.forward;

        _posMin = Application.isPlaying ? (Vector3)levelStartPos : _posMin;
        _posMax = Application.isPlaying ? (Vector3)levelEndPos : _posMax;

        //Disegna un rettangolo dove il giocatore muore
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(_posMin, _dim);
        Gizmos.DrawWireCube(_posMax, _dim);
    }

    #endregion
}

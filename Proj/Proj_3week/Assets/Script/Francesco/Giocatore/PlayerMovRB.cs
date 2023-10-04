using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D)),
 RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovRB : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float playerVel = 7.5f;
    [SerializeField] float jumpPower = 8.5f;
    float xMov;
    Vector3 moveAxis;
    float _speedMultip = 1;

    [Space(20)]
    [Min(0)]
    [SerializeField] float limitGroundCheck = 0.05f;
    [SerializeField] Vector2 boxcastDim = new Vector2(0.9f, 0.1f);
    float halfPlayerHeight;

    bool isOnGround = false;
    bool hasJumped = false;

    RaycastHit2D hitBase;
    RaycastHit2D slopeHit;

    [Header("—— Feedback ——")]
    [SerializeField] GameObject playerObj;

    [Space(10)]
    [SerializeField] Animator playerAnim;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        halfPlayerHeight = GetComponent<CapsuleCollider2D>().size.y / 2;
    }

    private void Update()
    {
        //Prende gli assi dall'input di movimento
        //xMov = ;GameManager.inst.inputManager.Giocatore.Movimento.ReadValue<Vector2>().x;
        if (Input.GetKey(KeyCode.A))
            xMov = -1;
        else if (Input.GetKey(KeyCode.D))
            xMov = 1;
        else
            xMov = 0;

        moveAxis = transform.right * xMov;      //Vettore movimento orizzontale


        //Prende l'input di salto
        //hasJumped = GameManager.inst.inputManager.Giocatore.Salto.ReadValue<float>() > 0;
        hasJumped = Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space);



        #region Feedback

        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        Quaternion leftRot = Quaternion.Euler(0, 180, 0),
                   rightRot = Quaternion.identity;


        if (isMoving)    //Se sta continuando a muoversi...
        {
            // flippa il gameObject se si muove verso sinistra, e torna normale se ti muovi a destra
            playerObj.transform.rotation = xMov < 0 ? leftRot : rightRot;
        }


        //Cambia l'animazione tra
        //corsa (se vel > 0.1)
        //e idle (se vel simile a 0)
        playerAnim.SetBool("isRunning", rb.velocity.magnitude > 0.15f);

        #endregion
    }

    void FixedUpdate()
    {
        //Calcolo se si trova a terra
        Vector3 cast_ToAdd = -transform.up * (halfPlayerHeight + limitGroundCheck);
        
        hitBase = Physics2D.BoxCast(transform.position + cast_ToAdd,
                                    boxcastDim,
                                    0,
                                    -transform.up,
                                    boxcastDim.y);
        isOnGround = hitBase;


        //Diminuisce la velocita' orizz. se si trova in aria
        float velMultip_air = !isOnGround ? 0.65f : 1;


        //Salta se premi Spazio e si trova a terra
        if (hasJumped && isOnGround)
        {
            //Resetta la velocita' Y e applica la forza d'impulso verso l'alto
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
        }


        //Movimento orizzontale (semplice) del giocatore
        rb.AddForce(moveAxis.normalized * playerVel * _speedMultip * 10f, ForceMode2D.Force);


        //Applica l'attrito dell'aria al giocatore
        //(Riduce la velocita' se il giocatore e' in aria e si sta muovendo)
        if (!isOnGround
            &&
            (rb.velocity.x >= 0.05f || rb.velocity.y >= 0.05f))
        {
            rb.AddForce(new Vector2(-rb.velocity.x * 0.1f, 0), ForceMode2D.Force);
        }


        #region Limitazione della velocita'

        //Prende la velocita' orizzontale del giocatore
        Vector2 horizVel = new Vector2(rb.velocity.x, 0);

        //Controllo se si accelera troppo, cioe' si supera la velocita'
        if (horizVel.magnitude >= playerVel)
        {
            //Limita la velocita' a quella prestabilita, riportandola al RigidBody
            Vector2 limit = horizVel.normalized * playerVel * velMultip_air;
            rb.velocity = new Vector2(limit.x, rb.velocity.y);
        }

        #endregion
    }


    public void SetPlayerSpeedMultip(float newMultip)
    {
        _speedMultip = newMultip;
    }

    public void ResetPlayerSpeedMultip()
    {
        _speedMultip = 1;
    }


    #region EXTRA - Gizmos

    private void OnDrawGizmos()
    {
        //Disegna il CubeCast per capire se e' a terra o meno (togliendo l'altezza del giocatore)
        Gizmos.color = new Color(0.85f, 0.85f, 0.85f, 1);
        Gizmos.DrawWireCube(transform.position + (-transform.up * halfPlayerHeight)
                             + (-transform.up * limitGroundCheck)
                             - (transform.up * (boxcastDim.y/2)),
                            (Vector3)boxcastDim + Vector3.forward);

        //Disegna dove ha colpito se e' a terra e se ha colpito un'oggetto solido (no trigger)
        Gizmos.color = Color.green;
        if (isOnGround && hitBase.collider)
        {
            Gizmos.DrawLine(hitBase.point + ((Vector2)transform.up * hitBase.distance), hitBase.point);
            Gizmos.DrawCube(hitBase.point, Vector3.one * 0.1f);
        }
    }

    #endregion
}

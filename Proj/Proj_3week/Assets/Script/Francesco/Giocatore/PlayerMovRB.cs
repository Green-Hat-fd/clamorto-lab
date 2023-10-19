using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CapsuleCollider2D)),
 RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovRB : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float playerVel = 7.5f;
    [SerializeField] float jumpPower = 8.5f;
    float xMov;
    Vector3 moveAxis;

    [Space(20)]
    [Min(0)]
    [SerializeField] float limitGroundCheck = 0.05f;
    [SerializeField] Vector2 boxcastDim = new Vector2(0.9f, 0.1f);
    float halfPlayerHeight;

    bool isOnGround = false,
         hasHitTileWall = false;
    bool hasJumped = false;

    RaycastHit2D hitBase,
                 hitWall,
                 hitStep;

    [Header("—— Feedback ——")]
    [SerializeField] SpriteRenderer playerSpr;

    [Space(10)]
    [SerializeField] AudioSource jumpSfx;
    [SerializeField] List<AudioSource> footstepsSfx;
    bool isStepTaken;

    [Space(10)]
    [SerializeField] Animator playerBodyAnim;
    [SerializeField] Animator playerLegsAnim;
    [SerializeField] Animator playerArmAnim;
    bool isInAirAfterJump,
         doOnce_jump = true;



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


        #region TODO: step assist
        //bool canStep = !hitStep && hasHitTileWall && xMov != 0;

        //if (canStep)
        //    transform.position += transform.up + moveAxis * 0.25f; 
        #endregion



        #region Feedback

        bool isMoving = xMov != 0;
        Quaternion leftRot = Quaternion.Euler(0, 180, 0),
                   rightRot = Quaternion.identity;


        if (isMoving)    //Se sta continuando a muoversi...
        {
            // flippa lo sprite se si muove verso sinistra, e torna normale se ti muovi a destra
            playerSpr.transform.rotation = xMov < 0 ? leftRot : rightRot;
        }


        //Appena calpesta il terreno col piede,
        //riproduce un suono casuale
        if (isStepTaken)
        {
            int rand_i = Random.Range(0, footstepsSfx.Count);
            AudioSource toPlay = footstepsSfx[rand_i];

            toPlay.PlayOneShot(toPlay.clip);

            isStepTaken = false;
        }


        //Cambia l'animazione a
        //corsa (se vel > 0.1)
        //e idle (se vel simile a 0)
        bool isWalking = rb.velocity.magnitude > 0.15f;

        AllAnimatorsSetBool("IsWalking", isWalking && !isInAirAfterJump);

        #endregion
    }

    void FixedUpdate()
    {
        //Calcolo se si trova a terra
        float dist_ToAdd = halfPlayerHeight + limitGroundCheck;
        
        hitBase = Physics2D.BoxCast(transform.position,
                                    boxcastDim,
                                    0,
                                    -transform.up,
                                    boxcastDim.y + dist_ToAdd);
        isOnGround = hitBase;


        //Diminuisce la velocita' orizz. se si trova in aria
        float velMultip_air = !isOnGround ? 0.65f : 1;


        //Salta se premi Spazio e si trova a terra
        if (hasJumped && isOnGround)
        {
            //Resetta la velocita' Y e applica la forza d'impulso verso l'alto
            Jump(jumpPower);

            //Riproduce il suono di salto
            //(con pitch casuale)
            jumpSfx.pitch = Random.Range(0.8f, 1.1f);
            jumpSfx.Play();


            if (doOnce_jump)
            {
                //Cambia l'animazione a quella di salto
                AllAnimatorsSetTrigger("Jump");
                Invoke(nameof(SetTrueIsInAirAfterJump), boxcastDim.y * 3);

                doOnce_jump = false;
            }
        }


        #region TODO: step assist
        //Calcolo se si trova di fianco ad un muro
        //Vector3 cast_ToSubtract = transform.up * halfPlayerHeight,
        //        stepHeight = transform.up * 1f;

        //hitWall = Physics2D.Raycast(transform.position - cast_ToSubtract,
        //                            moveAxis,
        //                            1f);

        //hitStep = Physics2D.Raycast(transform.position - cast_ToSubtract + stepHeight,
        //                            moveAxis,
        //                            1f);

        //hasHitTileWall = hitWall/*.transform.GetComponent<TilemapCollider2D>() != null*/;
        #endregion



        //Movimento orizzontale (semplice) del giocatore
        rb.AddForce(moveAxis.normalized * playerVel * 10f, ForceMode2D.Force);


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


        #region Feedback

        //Se è atterrato
        if (isInAirAfterJump && isOnGround)
        {
            //Cambia l'animazione a quella di atterraggio
            AllAnimatorsSetTrigger("Landed");

            //Reset della variabile
            isInAirAfterJump = false;

            doOnce_jump = true;
        }

        #endregion
    }

    public void Jump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }



    #region Funzioni Set Personalizzate

    void SetTrueIsInAirAfterJump() { isInAirAfterJump = true; }


    public void SetIsStepTaken(bool value)
    {
        isStepTaken = value;
    }


    public void AllAnimatorsSetBool(string boolName, bool value)
    {
        playerBodyAnim.SetBool(boolName, value);
        playerLegsAnim.SetBool(boolName, value);
        playerArmAnim.SetBool(boolName, value);
    }
    public void AllAnimatorsSetTrigger(string triggerName)
    {
        playerBodyAnim.SetTrigger(triggerName);
        playerLegsAnim.SetTrigger(triggerName);
        playerArmAnim.SetTrigger(triggerName);
    }

    #endregion



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

        #region TODO: step assist
        //Gizmos.DrawRay(transform.position, transform.right * xMov);
        //Gizmos.DrawCube(hitWall.point, Vector3.one * 0.1f);
        //Gizmos.DrawCube(hitStep.point, Vector3.one * 0.1f); 
        #endregion
    }

    #endregion
}

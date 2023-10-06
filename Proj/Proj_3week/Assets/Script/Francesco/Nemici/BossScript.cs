using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BossScript : Enemy
{
    int phaseNum = 1;

    [Header("—— Spara Palle-di-pelle ——")]
    [SerializeField] GameObject ballToSpawn;
    [SerializeField] float ballLife = 10;
    [Space(10)]
    [SerializeField] float angleRange_ball = 90;
    [SerializeField] Vector2 secRange_ball = new Vector2(0.5f, 1.5f);
    [Space(10)]
    [SerializeField] float ballRotatVel = 2.5f;

    [Header("—— Pesci che saltano ——")]
    [SerializeField] GameObject fishToSpawn;
    [SerializeField] float fishLife = 10;
    [Space(10)]
    [SerializeField] Vector2 offset = Vector2.down * 10;
    [SerializeField] float spawnArea_fish = 25;
    [SerializeField] Vector2 secRange_fish = new Vector2(0.5f, 1.5f);
    [SerializeField] float upForce_fish = 15;

    bool doOnce_ball = true;
    bool doOnce_fish = true;
    List<GameObject> leatherBalls = new List<GameObject>();

    [Header("—— Pesci che saltano ——")]
    [SerializeField] SpriteRenderer bossSpr;
    [SerializeField] float bossVelocity = 3.5f;
    [SerializeField] Transform leftPos, rightPos;
    Transform posToMove;
    Transform bossSpr_tr;




    void Awake()
    {
        bossSpr_tr = bossSpr.transform;

        //Reset alla fase iniziale
        phaseNum = 1;

        //Prende la posizione iniziale
        startPos = transform.position;
        posToMove = rightPos;
    }

    Vector3 startPos;
    void Update()
    {
        //Cambia la direzione rispetto a dove arriva
        if(Vector2.Distance(bossSpr_tr.position, posToMove.position) < 0.05f)
        {
            if (posToMove == leftPos)
                posToMove = rightPos;
            else if (posToMove == rightPos)
                posToMove = leftPos;
        }

        //Il movimento verso la direzione
        bossSpr_tr.position = Vector2.MoveTowards(bossSpr_tr.position,
                                                  posToMove.position,
                                                  Time.deltaTime * bossVelocity); 


        //Ogni volta che la vita scende dopo metà,
        //passa alla seconda fase, oppure controlla se è morto
        phaseNum =  health <= 0
                    ? -1
                    : health > maxHealth/2
                        ? 1
                        : 2;


        #region Sistemazione delle fasi

        switch (phaseNum)
        {
            //--Fase Palle di Pelle--//
            default:
            case 1:
                if (doOnce_ball)
                {
                    //Prende una rotazione a caso nel range dato
                    float randomRot = Random.Range(-angleRange_ball, angleRange_ball);
                    Quaternion ballRotation = Quaternion.Euler(0, 0, -90 + randomRot);

                    //Crea la palla nella rotazione scelta
                    GameObject ball = Instantiate(ballToSpawn, bossSpr_tr.position, ballRotation);

                    leatherBalls.Add(ball);
                    ball.GetComponent<EnemyBullet>().SetBulletLife(ballLife);



                    doOnce_ball = false;

                    StartCoroutine(EnableShootBalls());
                }

                //Ferma l'altro attacco
                StopCoroutine(EnableJumpingFishes());
                break;


            //--Fase Pesci--//
            case 2:
                if (doOnce_fish)
                {
                    //Prende una posizione a caso nel range dato
                    float randomPos = Random.Range(-spawnArea_fish / 2, spawnArea_fish / 2);
                    Vector3 startSpawnPoint = (Vector2)transform.position + offset,
                            fishPosition = new Vector3(startSpawnPoint.x + randomPos,
                                                       startSpawnPoint.y,
                                                       transform.position.z);

                    //Crea la palla nella rotazione scelta
                    GameObject fish = Instantiate(fishToSpawn, fishPosition, Quaternion.Euler(Vector3.up));

                    fish.GetComponent<EnemyBullet>().SetBulletLife(fishLife);

                    //Fa saltare il pesce verso l'alto
                    fish.GetComponent<Rigidbody2D>().AddForce(Vector3.up * upForce_fish,
                                                              ForceMode2D.Impulse);



                    doOnce_fish = false;

                    StartCoroutine(EnableJumpingFishes());
                }

                //Ferma l'altro attacco
                StopCoroutine(EnableShootBalls());
                break;


            //--Morte--//
            case -1:
                StopAllCoroutines();    //Ferma tutti gli attacchi

                WaitAndFinishBoss();    //Fa gli effetti di morte e poi
                                        //mostra lo schermo di vittoria
                break;
        }

        #endregion


        //Ruota le palle-proiettili
        foreach (GameObject _ball in leatherBalls)
        {
            Transform spr = _ball.GetComponentInChildren<SpriteRenderer>().transform;
            spr.rotation *= Quaternion.Euler(0, 0, ballRotatVel);
        }


        #region Feedback

        Quaternion rightRot = Quaternion.Euler(0, 180, 0),
                   leftRot = Quaternion.identity;


        //Flippa lo sprite se si muove verso sinistra,
        //e torna normale se si muove verso destra
        if (posToMove == leftPos)
            bossSpr_tr.rotation = leftRot;
        else if (posToMove == rightPos)
            bossSpr_tr.rotation = rightRot;

        #endregion
    }


    #region Funzioni per le Fasi

    IEnumerator EnableShootBalls()
    {
        float randomTime = Random.Range(secRange_ball.x, secRange_ball.y);
        yield return new WaitForSeconds(randomTime);

        doOnce_ball = true;
    }

    IEnumerator EnableJumpingFishes()
    {
        float randomTime = Random.Range(secRange_fish.x, secRange_fish.y);
        yield return new WaitForSeconds(randomTime);

        doOnce_fish = true;
    }

    #endregion


    IEnumerator WaitAndFinishBoss()
    {
        //Feedback - //TODO

        yield return new WaitForSeconds(10);
    }


    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //Limita il tempo dopo quanto ogni "proiettile" deve morire
        ballLife = Mathf.Max(0, ballLife);
        fishLife = Mathf.Max(0, fishLife);

        //Limita il range dei secondi di spawn
        secRange_ball.x = Mathf.Clamp(secRange_ball.x, 0, secRange_ball.x);
        secRange_ball.y = Mathf.Clamp(secRange_ball.y, secRange_ball.x, secRange_ball.y);

        secRange_fish.x = Mathf.Clamp(secRange_fish.x, 0, secRange_fish.x);
        secRange_fish.y = Mathf.Clamp(secRange_fish.y, secRange_fish.x, secRange_fish.y);
        
        //Limita l'angolo dello spawn delle palle
        angleRange_ball = Mathf.Clamp(angleRange_ball, 0, 180);
    }

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmos()
    {
        //Disegna la linea dove il boss si muove
        Vector3 bossSinPos = Application.isPlaying
                        ? startPos
                        : transform.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftPos.position, rightPos.position);
    }

    private void OnDrawGizmosSelected()
    {
        //Disegna due raggi nei limiti dove potrà spawnare le palle
        Quaternion rotLimitLeft = Quaternion.Euler(0, 0, -angleRange_ball),
                   rotLimitRight = Quaternion.Euler(0, 0, angleRange_ball);
        Vector3 angleLimitLeft = rotLimitLeft * Vector3.down,
                angleLimitRight = rotLimitRight * Vector3.down;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(bossSpr.transform.position, angleLimitLeft * 3);
        Gizmos.DrawRay(bossSpr.transform.position, angleLimitRight * 3);


        //Disegna un rettangolo dove si trova l'area di spawn dei pesci
        Vector3 fishBoxPos = transform.position + (Vector3)offset,
                fishBoxDim = Vector2.right * spawnArea_fish + Vector2.up;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(fishBoxPos, fishBoxDim);

        Gizmos.color = Color.green * 0.5f;
        fishBoxDim = new Vector3(fishBoxDim.x, 0);
        Gizmos.DrawWireCube(fishBoxPos + Vector3.up * (upForce_fish / (fishToSpawn.GetComponent<Rigidbody2D>().mass)),
                            fishBoxDim);
    }


    public float H_GetAngle() => angleRange_ball;
    public SpriteRenderer H_GetBossSprite() => bossSpr;

    #endregion
}


#region EXTRA - Handles

[CustomEditor(typeof(BossScript))]
public class BossHandles : Editor
{
    private void OnSceneGUI()
    {
        BossScript scr = (BossScript)target;

        Handles.color = Color.blue * 0.5f;
        Handles.DrawSolidArc(scr.H_GetBossSprite().transform.position,
                             Vector3.forward,
                             Quaternion.Euler(0, 0, -scr.H_GetAngle()) * Vector3.down,
                             scr.H_GetAngle() * 2,
                             1.1f);
    }
}

#endregion

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

    [Header("—— Pesci che saltano ——")]
    [SerializeField] GameObject fishToSpawn;
    [SerializeField] float fishLife = 10;
    [Space(10)]
    [SerializeField] Vector2 offset = Vector2.down * 10;
    [SerializeField] Vector2 spawnArea = Vector2.one * 25;
    [SerializeField] Vector2 secRange_fish = new Vector2(0.5f, 1.5f);

    bool doOnce_ball = true;
    bool doOnce_fish = true;
    List<GameObject> leatherBalls = new List<GameObject>();


    void Awake()
    {
        health = maxHealth;
        //Reset alla fase iniziale
        phaseNum = 1;
    }

    
    void Update()
    {
        //Ogni volta che la vita scende dopo metà,
        //passa alla seconda fase, oppure controlla se è morto
        phaseNum =  health <= 0
                    ? -1
                    : health > maxHealth/2
                        ? 1
                        : 2;

        switch (phaseNum)
        {
            default:
            case 1:
                if (doOnce_ball)
                {
                    //Prende una rotazione a caso nel range dato
                    float randomRot = Random.Range(-angleRange_ball, angleRange_ball);
                    Quaternion ballRotation = Quaternion.Euler(0, 0, -90 + randomRot);

                    //Crea la palla nella rotazione scelta
                    GameObject ball = Instantiate(ballToSpawn, transform.position, ballRotation);
                    leatherBalls.Add(ball);
                    Destroy(ball, ballLife);    //TODO: sistema la distruzione


                    doOnce_ball = false;

                    StartCoroutine(EnableShootBalls());
                }

                //Ferma l'altro attacco
                StopCoroutine(JumpingFishes());
                break;
            
            case 2:
                if (doOnce_fish)
                {
                    //TODO: fare


                    doOnce_fish = false;

                    StartCoroutine(JumpingFishes());
                }

                //Ferma l'altro attacco
                StopCoroutine(EnableShootBalls());
                break;
            
            case -1:
                StopAllCoroutines();    //Ferma tutti gli attacchi

                //Feedback - //TODO
                break;
        }
        
        //TODO: sistemare nello script apposito
        foreach (GameObject ball in leatherBalls)
        {
            if (!ball)
                leatherBalls.Remove(ball);
            
            ball.transform.position += ball.transform.right * Time.deltaTime * 3.5f;
            
            Transform spr = ball.GetComponentInChildren<SpriteRenderer>().transform;
            spr.rotation *= Quaternion.Euler(0, 0, 5);
        } 
    }


    IEnumerator EnableShootBalls()
    {
        float randomTime = Random.Range(secRange_ball.x, secRange_ball.y);
        yield return new WaitForSeconds(randomTime);

        doOnce_ball = true;
    }

    IEnumerator JumpingFishes()
    {
        float randomTime = Random.Range(secRange_fish.x, secRange_fish.y);
        yield return new WaitForSeconds(randomTime);

        doOnce_fish = true;
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

    private void OnDrawGizmosSelected()
    {
        Quaternion rotLimitLeft = Quaternion.Euler(0, 0, -angleRange_ball),
                   rotLimitRight = Quaternion.Euler(0, 0, angleRange_ball);
        Vector3 angleLimitLeft = rotLimitLeft * Vector3.down,
                angleLimitRight = rotLimitRight * Vector3.down;


        //Disegna due raggi nei limiti dove potrà spawnare le palle
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, angleLimitLeft * 3);
        Gizmos.DrawRay(transform.position, angleLimitRight * 3);

        //Disegna un rettangolo dove si trova l'area di spawn dei pesci
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube();
    }

    public float H_GetAngle() => angleRange_ball;

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
        Handles.DrawSolidArc(scr.transform.position,
                             scr.transform.forward,
                             Quaternion.Euler(0, 0, -scr.H_GetAngle()) * Vector3.down,
                             scr.H_GetAngle() * 2,
                             1.1f);
    }
}

#endregion

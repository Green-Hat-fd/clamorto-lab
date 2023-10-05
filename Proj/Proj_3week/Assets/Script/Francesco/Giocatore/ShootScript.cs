using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform leftShootingPoint,
                               rightShootingPoint,
                               upShootingPoint;
    Transform realShootingPoint;

    [Space(10)]
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float fireRate = 1f;

    private bool canShoot = true; // Aggiungiamo una variabile per controllare se è possibile sparare

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            Shoot();
            canShoot = false; // Disabilita temporaneamente la possibilità di sparare
            Invoke("EnableShooting", fireRate); // Richiama EnableShooting dopo 1 secondo
        }


        #region Cambio del punto di sparo rispetto al mouse

        //Calcola la posizione del mouse nel mondo e poi prende l'angolo rispetto alla telecamera
        Camera mainCam = Camera.main;
        Vector2 mouseInWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition),
                mouseInWorldPos_normalized = mouseInWorldPos - (Vector2)mainCam.transform.position;

            //Calcola l'angolo tra
            //il vettore (distanza tra posiz. mouse nel mondo e la camera)
            //normalizzato (ovvero posizionato all'origine [0;0])
            //e il vettore destra [1;0]
        float mouseAngle = Vector2.SignedAngle(mouseInWorldPos_normalized, Vector2.right);


        bool isPointingLeft = mouseAngle < -135 || mouseAngle > 90,
             isPointingUp = mouseAngle > -135 && mouseAngle < -45;


        //Cambia il punto rispetto a dove si trova il cursore
        //(se punta a sinistra -> sinistra)
        //(se no, se punta sopra -> in alto)
        //(se no, se punta a destra -> destra)
        realShootingPoint = isPointingLeft ? leftShootingPoint
                              : isPointingUp ? upShootingPoint
                                  : rightShootingPoint;

        #endregion
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab,
                                        realShootingPoint.position,
                                        realShootingPoint.localRotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = realShootingPoint.transform.right * bulletSpeed;
    }

    void EnableShooting()
    {
        canShoot = true; // Abilita nuovamente la possibilità di sparare
    }



    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        Camera _mainCam = Camera.main;

        Vector3 dirOne = CreaDirezioneDaAngolo(-90 * Mathf.Deg2Rad),
                dirTwo = CreaDirezioneDaAngolo(135 * Mathf.Deg2Rad),
                dirThree = CreaDirezioneDaAngolo(45 * Mathf.Deg2Rad);
        float length = 3;

            #region Funzioni interne

        Vector3 CreaDirezioneDaAngolo(float angolo)
        {
            return new Vector3(Mathf.Cos(angolo), Mathf.Sin(angolo));
        } 
        #endregion

        //Disegna le tre righe che dividono lo schermo
        Gizmos.color = Color.black;
        Gizmos.DrawRay(_mainCam.transform.position, length * dirOne);
        Gizmos.DrawRay(_mainCam.transform.position, length * dirTwo);
        Gizmos.DrawRay(_mainCam.transform.position, length * dirThree);
                                                                     
        //Disegna una piccola sfera dove si trova il mouse (quando si gioca)
        if (Application.isPlaying)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(_mainCam.ScreenToWorldPoint(Input.mousePosition),
                              0.1f);
        }
    }

    #endregion
}

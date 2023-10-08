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
    [Min(0)]
    [SerializeField] int maxAmmo = 20;
    int ammo;
    bool infiniteAmmo;

    [Space(10)]
    [SerializeField] float fireRate = 1f;

    bool canShoot = true; // Aggiungiamo una variabile per controllare se è possibile sparare



    private void Awake()
    {
        FullyRechargeAmmo();
    }

    void Update()
    {
        bool hasEnoughAmmo = ammo > 0;

        if (Input.GetKeyDown(KeyCode.Mouse0) && hasEnoughAmmo && canShoot)
        {
            Shoot();

            //Diminuisce le munizioni solo se ne ha limitate
            if (!infiniteAmmo)
                ammo--;

            canShoot = false; // Disabilita temporaneamente la possibilità di sparare
            Invoke("EnableShooting", fireRate); // Richiama EnableShooting dopo 1 secondo
        }


        //Limita il numero di munizioni tra 0 e il massimo
        ammo = Mathf.Clamp(ammo, 0, maxAmmo);


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
            //Spara solo dall'alto
            //se è entrato nella zona del boss
        Transform whereToShoot = infiniteAmmo
                                  ? upShootingPoint
                                  : realShootingPoint; 

        Instantiate(bulletPrefab,
                    whereToShoot.position,
                    whereToShoot.localRotation);
    }

    void EnableShooting()
    {
        canShoot = true; // Abilita nuovamente la possibilità di sparare
    }


    #region Funzioni Set Personalizzate

    public void RechargeAmmo(int ammoToAdd)
    {
        ammo += ammoToAdd;    //Aggiunge alle munizioni
    }
    public void FullyRechargeAmmo()
    {
        ammo = maxAmmo;    //Mette le munizioni al massimo
    }

    public void SetInfiniteAmmo(bool value)
    {
        infiniteAmmo = value;
    }

    #endregion


    #region Funzioni Get personalizzate

    public int GetMaxAmmo() => maxAmmo;
    public int GetAmmo() => ammo;
    public bool GetHasInfiniteAmmo() => infiniteAmmo;

    #endregion



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

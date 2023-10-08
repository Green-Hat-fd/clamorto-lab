using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : Enemy
{
    Transform player;
    [SerializeField] GameObject bulletPrefab;

    [Space(10)]
    [SerializeField] float fireRate = 1f;
    [SerializeField] Transform firePoint;
    [SerializeField] float maxShootDistance = 10f;

    private float nextFireTime;
    [SerializeField] Animator enAnim;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMovRB>().transform;
    }

    void Update()
    {
        // Calcola la distanza tra il nemico e il giocatore
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= maxShootDistance && Time.time >= nextFireTime)
        {
            // Calcola la direzione verso il giocatore
            Vector3 direction = (player.position - transform.position).normalized;

            // Calcola l'angolo in radianti tra la direzione e il vettore destro (1,0)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Forza l'angolo a essere 0 o 180 gradi
            angle = Mathf.Abs(angle) > 90f ? 180f : 0f;

            // Imposta la rotazione dell'oggetto in base all'angolo calcolato
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Shoot(angle);

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot(float bulletYRot)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.rotation = Quaternion.Euler(0, bulletYRot, 0);


        //Feedback
        enAnim.SetTrigger("Shoot");
    }



    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        //Disegna l'area di azione
        Gizmos.color = new Color(0, 0.75f, 1, 1);
        Gizmos.DrawWireSphere(transform.position, maxShootDistance);
    }

    #endregion
}

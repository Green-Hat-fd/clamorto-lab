using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float projectileSpeed = 2;
    [Min(0)]
    [SerializeField] float bulletLife = 5;



    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnEnable()
    {
        RemoveBullet();
    }

    void FixedUpdate()
    {
        //Muove il proiettile verso destra
        transform.position += transform.right * projectileSpeed * Time.deltaTime;
    }


    void RemoveBullet()
    {
        Destroy(gameObject, bulletLife);
    }

    public void SetBulletLife(float value)
    {
        bulletLife = value;
    }
}

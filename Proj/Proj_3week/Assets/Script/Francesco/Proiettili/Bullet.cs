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

    [Space(20)]
    [SerializeField] bool canRotate;
    [SerializeField] SpriteRenderer rotatingSpr;
    [Min(0)]
    [SerializeField] float rotatVel;



    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnEnable()
    {
        RemoveBulletWithLife();
    }

    void FixedUpdate()
    {
        //Muove il proiettile verso destra
        transform.position += transform.right * projectileSpeed * Time.deltaTime;


        //Ruota lo sprite solo se può ruotare
        if (canRotate)
        {
            Vector3 axisRot = Vector3.forward * rotatVel;
            
            rotatingSpr.transform.rotation *= Quaternion.Euler(axisRot);
        }
    }


    protected void RemoveBullet()
    {
        Destroy(gameObject);
    }
    void RemoveBulletWithLife()
    {
        Destroy(gameObject, bulletLife);
    }

    public void SetBulletLife(float value)
    {
        bulletLife = value;
    }
    public void SetBulletLife_RemoveIt(float value)
    {
        bulletLife = value;

        RemoveBulletWithLife();
    }

    public void SetBulletRotationVel(float value)
    {
        rotatVel = value;
    }
}

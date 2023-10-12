using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : Bullet
{
    #region Enum

    enum BossBulletType_Enum
    {
        [InspectorName("Palla di pelle")]
        Palla,
        [InspectorName("Pesce che salta")]
        Pesce,
    }

    #endregion

    [Space(10)]
    [SerializeField] BossBulletType_Enum bulletType;

    [Header("—— Palla di pelle ——")]
    [SerializeField] SpriteRenderer rotatingSpr;
    [SerializeField] float rotatVel = 2.5f;

    [Header("—— Pesci che saltano ——")]
    [SerializeField] SpriteRenderer fishSpr;
    [SerializeField] Rigidbody2D rb2D;



    protected override void FixedUpdate()
    {
        //Esegue prima tutta la logica
        //della classe genitore (Bullet)
        base.FixedUpdate();


        #region Cambio del comportamento rispetto all'attacco

        switch (bulletType)
        {
            //--Palle di pelle--//
            default:
            case BossBulletType_Enum.Palla:

                //Ruota lo sprite della palla
                Vector3 axisRot = Vector3.forward * rotatVel;
                rotatingSpr.transform.rotation *= Quaternion.Euler(axisRot);

                break;


            //--Pesci--//
            case BossBulletType_Enum.Pesce:

                //Ruota il pesce rispetto alla sua velocità
                float _rot = rb2D.velocity.y * -25,              //Prende la velocità del pesce
                      _direction = Mathf.Clamp(_rot, -90, 90);   //Limita la rotazione tra -90 e 90

                //Lo ruota sull'asse Z
                fishSpr.transform.rotation = Quaternion.Euler(Vector3.forward * _direction);

                break;
        }

        #endregion
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatsManager playerCheck = collision.GetComponent<PlayerStatsManager>();

        if (playerCheck != null)    //Se colpisce il giocatore
        {
            //Lo danneggia
            playerCheck.Pl_TakeDamage();
        }
    }

    public void SetRotationVel(float value)
    {
        rotatVel = value;
    }

    public void SetFishSprite(Sprite spr)
    {
        fishSpr.sprite = spr;
    }
}

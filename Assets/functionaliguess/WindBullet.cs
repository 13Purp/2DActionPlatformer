using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullets;
public class WindBullet : MonoBehaviour,IBullet
{
    // Start is called before the first frame update

    public float bulletSpeed = 20f;
    public float spreadAngle;
    public Rigidbody2D rigidBody2D;
    public int strongShootCnt;
    public float fireSpeed;
    public bool timerLocked;
    void Start()
    {

        float randomSpread = Random.Range(-spreadAngle, spreadAngle);
        Quaternion rotation = Quaternion.Euler(0, 0, randomSpread);
        Vector2 spreadDirection = rotation * transform.right;
        rigidBody2D.velocity = 1 * bulletSpeed * spreadDirection;
        rigidBody2D.gravityScale = 0;
        timerLocked = false;

        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame


     public void Shoot(Rigidbody2D parentRigidbody, Vector3 firepointPosition, Quaternion firepointRotation, Vector2 direction, bool isGrounded)
    {
       
        Instantiate(gameObject, firepointPosition, firepointRotation);
        //Vector2 forceDirection = playerController.facingRight ? Vector2.left : Vector2.right;
        //if (!isGrounded)
        //{
        //    parentRigidbody.AddForce(-1 * 2.5f * direction, ForceMode2D.Impulse);
        //}
        
    }



     public void strongShoot(Rigidbody2D parentRigidbody, Vector3 firepointPosition, Quaternion firepointRotation, Vector2 direction, bool isGrounded)
    {
       
        for (int i = 0; i < strongShootCnt; i++)
            Instantiate(gameObject, firepointPosition, firepointRotation);

        //Vector2 forceDirection = playerController.facingRight ? Vector2.left : Vector2.right;

        parentRigidbody.AddForce(-1 * strongShootCnt*1.2f * direction, ForceMode2D.Impulse);

        
    }
   
}



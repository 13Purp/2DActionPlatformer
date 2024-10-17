using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;
using Cache;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]private Rigidbody2D myRigidbody;
    [SerializeField] private Collider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
       
        WorldCache worldCache= WorldCache.GetInstance();
        worldCache.AddColiderRig(myCollider, myRigidbody);
    }


    void OnTriggerEnter2D(Collider2D hit)
    {

       // Debug.Log("hit");
        if (hit.GetComponent<Collider2D>() != null && hit.GetComponent<Collider2D>().CompareTag("Enemy"))//seti se da kesujes sve debile u recnik
        {
            //Debug.Log("22hit22");
            Enemy enemy = hit.GetComponent<Enemy>();


            if (enemy != null)
            {
                //Debug.Log("hit Enemy");
                Vector2 directionToEnemy = (hit.transform.position - transform.position).normalized;

                Vector2 forwardVelocity = myRigidbody.velocity.normalized;

                float dotProduct = Vector2.Dot(forwardVelocity, directionToEnemy);

                if (dotProduct > 0)
                {
                    float damage = myRigidbody.mass * myRigidbody.velocity.magnitude;
                    Debug.Log("Damaged by: " + damage);

                    enemy.damage(damage);
                }
            }

        }
    }
}

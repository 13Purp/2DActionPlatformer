using Bullets;
using Cache;
using Mobs;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class ZapBullet : MonoBehaviour, IBullet
{
    // Start is called before the first frame update

    public float bulletSpeed = 20f;
    public float spreadAngle;
    public int strongShootCnt;
    public float fireSpeed;
    public bool timerLocked;
    public float damage;
    public float radius;
    public float range;
    public LineRenderer zapLine;
    public GameObject impactEffect;
    private WorldCache WorldCache;
    void Start()
    {

        //float randomSpread = Random.Range(-spreadAngle, spreadAngle);
        //Quaternion rotation = Quaternion.Euler(0, 0, randomSpread);
        //Vector2 spreadDirection = rotation * transform.right;
        //rigidBody2D.velocity = 1 * bulletSpeed * spreadDirection;
        //timerLocked = false;

        //Destroy(gameObject, 2.5f);
        impactEffect.transform.localScale = new Vector3(radius * 2, radius * 2, 1);

    }

    // Update is called once per frame


    public void Shoot(Rigidbody2D parentRigidbody, Vector3 firepointPosition, Quaternion firepointRotation, Vector2 direction, bool isGrounded)
    {

        RaycastHit2D hit = Physics2D.Raycast(firepointPosition, firepointRotation * Vector2.right, range, ~(1 << LayerMask.NameToLayer("Player")));

        if (hit)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();//podlozno kesiranju
            if (enemy != null)
                enemy.damage(damage);
            DrawLightning(firepointPosition, hit.point, 0.07f);
            //DrawLightningDeprecated(firepointPosition, hit.point);

        }



    }



    public void strongShoot(Rigidbody2D parentRigidbody, Vector3 firepointPosition, Quaternion firepointRotation, Vector2 direction, bool isGrounded)
    {


        RaycastHit2D hit = Physics2D.Raycast(firepointPosition, firepointRotation * Vector2.right, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player")));

        if (hit)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(hit.point, radius);
            foreach (Collider2D bonked in hits)
            {
                Enemy enemy = bonked.transform.GetComponent<Enemy>();

                RaycastHit2D raycastHit = Physics2D.Raycast(firepointPosition, (hit.transform.position - firepointPosition).normalized, radius, (1 << LayerMask.NameToLayer("Ground")));

                // If no obstacle is hit, apply damage to the enemy
                if (raycastHit.collider == null)
                {
                    for (int i = 0; i < strongShootCnt; i++)
                    {
                        DrawLightning(firepointPosition, hit.point, 0.1f);
                    }
                    WorldCache = WorldCache.GetInstance();
                    Rigidbody2D bonkedBody2D = WorldCache.GetRigidbody2D(bonked);
                    Vector2 closestPoint = bonked.ClosestPoint(hit.point);
                    Vector2 explosionDirection = ((Vector2)bonked.transform.position - closestPoint).normalized;
                    if (bonkedBody2D != null)
                    {
                        bonkedBody2D.AddForceAtPosition(explosionDirection * 4000f, closestPoint);
                    }
                    if (enemy != null)
                        enemy.damage(damage);


                }
            }
            Instantiate(impactEffect, hit.point, firepointRotation);
        }


        //Vector2 forceDirection = playerController.facingRight ? Vector2.left : Vector2.right;

        parentRigidbody.AddForce(-1 * strongShootCnt * direction, ForceMode2D.Impulse);


    }
    private void DrawLightningDeprecated(Vector3 start, Vector3 end)
    {

        Vector3 direction = (end - start).normalized;
        Vector3 PerpDirection = new Vector3(-direction.y, direction.x, 0f);
        float distance = (end - start).magnitude;
        Vector3 perpDirection = new Vector3(-direction.y, direction.x, 0f);

        int pointNumber = Random.Range(2, 3);
        int directionModifier = Random.Range(0, 2) * 2 - 1;

        Vector3 sp = start + (end - start) / pointNumber;
        Vector3 ep = start;
        Vector3 distanceCovered = ep;





        LineRenderer zapLine2 = Instantiate(zapLine);
        zapLine2.widthMultiplier = 0.002f;
        zapLine2.SetPosition(0, start);
        zapLine2.SetPosition(1, ep);

        for (int i = 0; i < pointNumber; i++)
        {
            //directionModifier = directionModifier * -1;
            sp = ep;
            distanceCovered += ((end - start) / (pointNumber + 1)) * 2;
            float x = Random.Range(0f, 2.5f);
            ep = distanceCovered + directionModifier * x * (PerpDirection);

            zapLine2 = Instantiate(zapLine);
            zapLine2.SetPosition(0, sp);
            zapLine2.SetPosition(1, ep);


        }




    }
    //well aware that there is no reason for this to be a Task, especially since the normal function is called later on
    //just experimenting a bit
    private void DrawLightning(Vector3 start, Vector3 end, float strength)
    {
        // Start the calculations on another thread
        Task.Run(() =>
        {

            // Perform heavy calculations here

            return CalculateLightningPoints(start, end);


        }).ContinueWith(task =>
        {
            // This runs back on the main thread when calculations are done          
            DrawLineOnMainThread(task.Result, strength);

        }, TaskScheduler.FromCurrentSynchronizationContext()); // Ensure this part runs on the main thread
    }

    private List<Vector3> CalculateLightningPoints(Vector3 start, Vector3 end)
    {
        System.Random rand = new System.Random();
        Vector3 direction = (end - start).normalized;
        float distance = (end - start).magnitude;
        Vector3 perpDirection = new Vector3(-direction.y, direction.x, 0f);

        int pointNumber = (distance > 10) ? rand.Next(10, 15) : rand.Next(2, 5);
        Vector3 ep = start;
        Vector3 distanceCovered = ep;

        List<Vector3> points = new()
        {
            start
        };

        for (int i = 0; i < pointNumber; i++)
        {
            distanceCovered += (end - start) / (pointNumber + 1);
            ep = distanceCovered + (perpDirection * (float)(rand.NextDouble() * 3.0 - 1.5));
            points.Add(ep);
        }

        points.Add(end);
        return points;
    }

    private void DrawLineOnMainThread(List<Vector3> points, float strenght)
    {
        Vector3 start = points[0];
        Vector3 end = points[points.Count - 1];



        for (int i = 0; i < points.Count - 1; i++)
        {
            LineRenderer zapLineInstance = Instantiate(zapLine);
            zapLineInstance.widthMultiplier = strenght;
            zapLineInstance.SetPosition(0, points[i]);
            zapLineInstance.SetPosition(1, points[i + 1]);

        }

        DrawLightningDeprecated(points[points.Count / 3], points[points.Count / 3 + 2]);
        DrawLightningDeprecated(points[points.Count / 3 * 2], points[points.Count / 3 * 2 + 2]);
    }


}



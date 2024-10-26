using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullets;
using Controller;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform firepoint;
    public GameObject windBulletPrefab;
    public GameObject zapBulletPrefab;
    private int strongShootCnt;
    private float fireSpeed;
   private IBullet bullet;
    private bool timerLocked;
    public Transform shoulderTransform;
    private Rigidbody2D parentRigidbody;
    private IController IController;
    private float weaponLength;
    private Vector2 direction;
    private Collider2D _target;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer.enabled = false;

    }
    void Start()
    {
        parentRigidbody = GetComponentInParent<Rigidbody2D>();
        IController = GetComponentInParent<IController>();
        weaponLength = Vector2.Distance(shoulderTransform.position, firepoint.position);
        WindBullet windBullet = windBulletPrefab.GetComponent<WindBullet>();
        windBullet.timerLocked = false;
        fireSpeed = windBullet.fireSpeed;
        strongShootCnt=windBullet.strongShootCnt;
        bullet = windBullet as IBullet;

    }

    // Update is called once per frame
    void Update()
    {
        if(IController.isUsingWeapon())
            FollowTarget();
    }

    public void weakFire()
    {
        if (timerLocked == false)
        {
            timerLocked = true;
            StartCoroutine(BulletCooldown(1f));
            bullet.Shoot(parentRigidbody, firepoint.position, firepoint.rotation, direction, IController.IsGrounded());
        }
    }

    public void strongFire()
    {
        if (timerLocked == false)
        {
            timerLocked = true;
            StartCoroutine(BulletCooldown(strongShootCnt * 0.8f));
            bullet.strongShoot(parentRigidbody, firepoint.position, firepoint.rotation, direction, IController.IsGrounded());
        }
    }




    IEnumerator BulletCooldown(float strength)
    {
        yield return new WaitForSeconds(fireSpeed * strength);
        timerLocked = false;

    }
    public void SetWindBullet()
    {
        WindBullet windBullet = windBulletPrefab.GetComponent<WindBullet>();
        windBullet.timerLocked = false;
        fireSpeed = windBullet.fireSpeed;
        strongShootCnt = windBullet.strongShootCnt;
        bullet = windBullet as IBullet;
    }

    public void SetZapBullet()
    {
        
        ZapBullet zapBullet = zapBulletPrefab.GetComponent<ZapBullet>();
        fireSpeed = zapBullet.fireSpeed;
        strongShootCnt = zapBullet.strongShootCnt;
        bullet = zapBullet as IBullet;

    }

    public void displayWeapon()
    {
        spriteRenderer.enabled=!spriteRenderer.enabled;
    }
    void FollowTarget()
    {
        Vector3 targetPos;
        if (IController.IsPlayer())
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0f;
        }
        else
        {
            targetPos = _target.bounds.center;
            targetPos.z = 0f;
        }

            direction = (targetPos - shoulderTransform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            transform.SetPositionAndRotation(shoulderTransform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

            UpdateFirepoint(direction);

            Vector2 playerDirection = IController.FacingRight() ? Vector2.right : Vector2.left;
            float angleToPlayer = Vector2.Angle(playerDirection, direction);

            if (angleToPlayer > 90f)
            {

                IController.Flip();
            }
        


    }
    public void SetTarget(Collider2D col)
    {
        _target = col;
    }

  
    void UpdateFirepoint(Vector2 direction)
    {
        
        firepoint.SetPositionAndRotation(transform.position + (Vector3)direction * weaponLength, transform.rotation);
    }



}

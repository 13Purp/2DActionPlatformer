using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;
public class AnxElenaController : Enemy
{
    // Start is called before the first frame update

    [SerializeField] private Weapon _weapon;
    [SerializeField] Collider2D _col;


    void Start()
    {


        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        facingRight = true;
        locked = false;
        chasing = false;
        layerMask = LayerMask.GetMask("Player", "Ground");
        _jumpable = LayerMask.GetMask("Ground");
        _boxCastSize = new Vector2(boxCol.bounds.size.x * 0.8f, boxCol.bounds.size.y * 0.9f);
        _weapon.SetTarget(_col);
        _weapon.displayWeapon();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {

        Vector2 direction = (facingRight) ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(boxCol.bounds.center, direction, detectionDistance, layerMask);
        chasing = (hit.collider != null && hit.collider.CompareTag("Player"));
        if (chasing)
        {
            _weapon.weakFire();
        }

        if (!locked)
        {
            locked = true;
            StartCoroutine(patrolTimer());

        }

        if (facingRight)
        {


            rigidBody2D.AddForce(new Vector2(1f * ((chasing) ? chasingMoveSpeed : moveSpeed), 0f), ForceMode2D.Impulse);
        }
        else
        {
            rigidBody2D.AddForce(new Vector2(-1f * ((chasing) ? chasingMoveSpeed : moveSpeed), 0f), ForceMode2D.Impulse);

        }

    }
}

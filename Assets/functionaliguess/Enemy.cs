using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mobs
{
    [System.Serializable]
    public class Enemy : MonoBehaviour,IController
    {
        protected Rigidbody2D rigidBody2D;
        protected BoxCollider2D boxCol;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float chasingMoveSpeed;
        [SerializeField] protected float patrolLength;
        [SerializeField] protected float lifePoints;
        protected bool facingRight;
        protected bool locked;
        protected bool chasing;
        protected int layerMask;
        [SerializeField] protected float detectionDistance;
        protected LayerMask _jumpable;
        protected Vector2 _boxCastSize;
        private bool _usingWeapon;



        // Start is called before the first frame update



        protected IEnumerator patrolTimer()
        {
            yield return new WaitForSeconds(patrolLength);
            if (!chasing)
                Flip();
            locked = false;


        }

        public virtual void Flip()
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180, 0f);
        }

        public virtual bool IsGrounded()
        {
            RaycastHit2D _jumpPoint = Physics2D.BoxCast(boxCol.bounds.center, _boxCastSize, 0f, Vector2.down, 0.1f, _jumpable);
            return _jumpPoint.collider != null;
        }
        public bool IsPlayer()
        {
            return false;
        }
        public bool FacingRight()
        {
            return facingRight;
        }
        public virtual void damage(float damage)
        {
            lifePoints -= damage;
            Debug.Log("Jao ode:" + damage);
            if (lifePoints <= 0)
                die();
        }

        public virtual void die()
        {
            Destroy(gameObject);
        }

        public bool isUsingWeapon()
        {
            return _usingWeapon;
        }
    }
}

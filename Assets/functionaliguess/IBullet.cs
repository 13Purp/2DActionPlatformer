using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets
{


    public interface IBullet
    {
        public void Shoot(Rigidbody2D parentRigidbody, Vector3 firepointPosition, Quaternion firepointRotation, Vector2 direction, bool isGrounded);
        public void strongShoot(Rigidbody2D parentRigidbody, Vector3 firepointPosition, Quaternion firepointRotation, Vector2 direction, bool isGrounded);
    }
}

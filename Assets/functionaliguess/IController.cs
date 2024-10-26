using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{


    public interface IController
    {
        public void Flip();
        public bool IsPlayer();

        public bool FacingRight();
        public bool IsGrounded();

        public bool isUsingWeapon();
    }
}

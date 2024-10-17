using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Mobs;
using static Unity.Collections.AllocatorManager;
namespace Cache
{
    public class WorldCache 
    {
        private Dictionary<Collider2D, Rigidbody2D> ColRig;
        private Dictionary<Collider2D, Enemy> ColEn;
        private static readonly object Lock = new object();


        private static WorldCache Instance;
        private WorldCache() {

            ColRig = new Dictionary<Collider2D, Rigidbody2D>();
            ColEn= new Dictionary<Collider2D, Enemy>();

        }


        public static WorldCache GetInstance()
        {
            if (Instance == null)
            {
                lock (Lock)
                {


                    Instance = new WorldCache();

                }
            }
            return Instance;
        }


        public Dictionary<Collider2D, Rigidbody2D> GetColRigidBodyDict()
        {
            
            return ColRig;
        }

        public Dictionary<Collider2D, Enemy> GetColEnemyDict()
        {

            return ColEn;
        }

        public void AddColiderRig(Collider2D col,Rigidbody2D rig)
        {
            ColRig.Add(col, rig);
        }

        public void AddColiderEn(Collider2D col, Enemy enemy)
        {
            ColEn.Add(col, enemy);
        }

        public void RemoveColiderRig(Collider2D col)
        {
            ColRig.Remove(col);
        }
        public void RemoveColiderEn(Collider2D col)
        {
            ColEn.Remove(col);
        }

        public Rigidbody2D GetRigidbody2D(Collider2D col)
        {
            if (ColRig.ContainsKey(col))
                return ColRig[col];
            else
                return null;
        }

        public Enemy GetEnemy(Collider2D col)
        {
            if (ColEn.ContainsKey(col))
                return ColEn[col];
            else 
                return null;
        }

    }
}

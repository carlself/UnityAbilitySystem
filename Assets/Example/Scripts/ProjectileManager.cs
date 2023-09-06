using System;
using UnityEngine;

namespace Example
{
    
    public class ProjectileManager : MonoBehaviour
    {
        private static ProjectileManager s_instance;

        public static ProjectileManager Instance => s_instance;

        public Projectile projectilePrefab;
        
        private void Awake()
        {
            s_instance = this;
        }

        public Projectile SpawnProjectile(float speed,Vector3 originPos, RPGUnit target)
        {
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = originPos;
            projectile.MoveToTarget(speed, target);
            return projectile;
        }
    }
}
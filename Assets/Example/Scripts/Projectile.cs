using UnityEngine;

namespace Example
{
    public class Projectile : MonoBehaviour
    {
        public float damage;
        // private Vector3 m_TargetPos;
        // private float m_Speed;
        private Vector3 m_Velocity;
        private float m_LifeTime;
        private RPGUnit m_Target;
        public void MoveToTarget(float speed, RPGUnit target)
        {
            m_Target = target;
            Vector3 direction = target.headTransform.position - transform.position;
            m_LifeTime = direction.magnitude / speed;
            m_Velocity = direction.normalized * speed;
        }

        void Update()
        {
            if (m_LifeTime <= 0)
            {
                m_Target.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
            
            transform.position += Time.deltaTime * m_Velocity;
            m_LifeTime = m_LifeTime - Time.deltaTime;
        }
    }
}
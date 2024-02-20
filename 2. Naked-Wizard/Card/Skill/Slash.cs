using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Utility;

namespace LMS.Cards
{
    public class Slash : MonoBehaviour
    {
        private ParticleSystem ps;

        private float damage;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void Initialized(Vector3 pos, float randY, float randZ, float damage, string name)
        {
            transform.position = pos;
            transform.rotation = Quaternion.Euler(0f, randY, randZ);

            this.damage = damage; // 임시 설정

            Manager.GameManager.Instance.ExecuteCoroutine(ParticleUtil.ReturnParticle(ps, gameObject, ObjectPool.Instance.objectInfos[5], this, name)); // 파티클 자동 리턴
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Monster"))
            {
                if(other.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(damage, Vector3.zero);
            }
        }
    }
}


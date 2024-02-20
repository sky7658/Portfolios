using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Utility;

namespace LMS.Cards
{
    public class LionRoar : MonoBehaviour
    {
        private ParticleSystem ps;
        private float radius;
        private float damage;
        private void Awake()
        {
            radius = GetComponent<SphereCollider>().radius;
            ps = GetComponent<ParticleSystem>();
        }
        public void Initialized(Vector3 pos, float damage, string name)
        {
            transform.position = pos;

            this.damage = damage;
            Manager.GameManager.Instance.ExecuteCoroutine(ParticleUtil.ReturnParticle(ps, gameObject, ObjectPool.Instance.objectInfos[1], this, name)); // 파티클 자동 리턴
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Monster"))
            {
                var _dir = other.transform.position - transform.position; // 튕겨낼 방향 설정
                var _dis = Vector3.Distance(transform.position + _dir.normalized * radius, other.transform.position); // 목표 위치까지 얼마나 더 가야하는지 계산


                if (other.TryGetComponent(out Monster _monster))
                {
                    // 일반 몬스터
                    _monster.TakeDamage(damage, Vector3.zero);
                    Manager.GameManager.Instance.ExecuteCoroutine(SkillAction.BounceOut(_monster, _dir.normalized, _dis));
                }
                else
                {
                    // 보스의 경우
                    other?.GetComponent<IDamageable>().TakeDamage(damage, Vector3.zero);
                }
            }

        }
    }
}



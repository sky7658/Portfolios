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
            Manager.GameManager.Instance.ExecuteCoroutine(ParticleUtil.ReturnParticle(ps, gameObject, ObjectPool.Instance.objectInfos[1], this, name)); // ��ƼŬ �ڵ� ����
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Monster"))
            {
                var _dir = other.transform.position - transform.position; // ƨ�ܳ� ���� ����
                var _dis = Vector3.Distance(transform.position + _dir.normalized * radius, other.transform.position); // ��ǥ ��ġ���� �󸶳� �� �����ϴ��� ���


                if (other.TryGetComponent(out Monster _monster))
                {
                    // �Ϲ� ����
                    _monster.TakeDamage(damage, Vector3.zero);
                    Manager.GameManager.Instance.ExecuteCoroutine(SkillAction.BounceOut(_monster, _dir.normalized, _dis));
                }
                else
                {
                    // ������ ���
                    other?.GetComponent<IDamageable>().TakeDamage(damage, Vector3.zero);
                }
            }

        }
    }
}



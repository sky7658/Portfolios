using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Manager;
using LMS.Utility;

namespace LMS.Cards
{
    public class Meteors : MonoBehaviour
    {
        private ParticleSystem ps;

        public float colliderSize = 9f;
        private float damage;

        private float duration;
        private int takeCount;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void Initialized(GameObject obj, Vector3 pos, float damage, int takeCount, string name)
        {
            this.damage = damage;
            duration = 3.5f;
            this.takeCount = takeCount;

            transform.position = pos;

            var _targetPos = pos + new Vector3(0f, 3f, 0f);

            GameManager.Instance.ExecuteCoroutine(SkillAction.Teleport(obj, _targetPos, 0.2f, false, null, () => TeleportEffect(_targetPos)));
            GameManager.Instance.ExecuteCoroutine(SkillAction.RetentionTime(3f, () =>
            GameManager.Instance.ExecuteCoroutine(SkillAction.Teleport(obj, pos, 0.2f, true, null, () => TeleportEffect(pos)))));
            GameManager.Instance.ExecuteCoroutine(DealDamage(obj.transform.position));

            GameManager.Instance.ExecuteCoroutine(ParticleUtil.ReturnParticle(ps, gameObject, ObjectPool.Instance.objectInfos[2], this, name)); // 파티클 자동 리턴
        }

        private void TeleportEffect(Vector3 pos)
        {
            var _newEffect = ObjectPool.Instance.GetObject<ParticleSystem>("Teleport");
            UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[3], _newEffect.gameObject, true);
            _newEffect.transform.position = pos;
        }

        private IEnumerator DealDamage(Vector3 pos)
        {
            int _count = 0;
            yield return new WaitForSeconds(0.2f);

            while (_count < takeCount)
            {
                var hits = Physics.OverlapSphere(pos, colliderSize, LayerMask.GetMask("Water"));
                Debug.Log(hits.Length);

                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(damage, Vector3.zero);
                }

                _count++;
                yield return new WaitForSeconds(duration / takeCount);
            }

            yield break;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, colliderSize);
        }
    }
}


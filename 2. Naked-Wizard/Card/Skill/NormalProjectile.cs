using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Utility;

namespace LMS.Cards
{
    public class NormalProjectile : Projectile
    {
        private ParticleSystem ps;
        private Coroutine coroutine;
        protected override void Awake()
        {
            base.Awake();
            col = GetComponent<SphereCollider>();
            ps = GetComponent<ParticleSystem>();
        }
        public override void Initialized(Vector3 arrow, Vector3 pos, float speed, string prefName, float damage, CardInfo info = null)
        {
            base.Initialized(arrow, pos, speed, prefName, damage, info);
            coroutine = Manager.GameManager.Instance.ExecuteCoroutine(SkillAction.RetentionTime(1f, () => Release(this)));
        }
        protected override void OnTriggerEnter(Collider other) // ¼öÁ¤
        {
            if (other.CompareTag("Monster"))
            {
                if (coroutine != null)
                {
                    Manager.GameManager.Instance.StopCoroutine(coroutine);
                    coroutine = null;
                }
                Release(this);
                if (other.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(damage, Vector3.zero);
            }
        }
    }
}
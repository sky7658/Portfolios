using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Utility;

namespace LMS.Cards
{
    public class SkillEffect : MonoBehaviour
    {
        protected string name;
        protected CardInfo info;

        protected List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        protected virtual void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                particleSystems.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
                ParticleUtil.SetParticleLoop(particleSystems[i], true);
            }
        }

        public virtual void Initialized(Vector3 arrow, Vector3 pos, float speed, string prefName, float damage, CardInfo info = null)
        {
            if (info != null)
            {
                this.info = info;
                ParticleUtil.InitParticleColor(particleSystems, prefName, this.info.property);
            }
            else
            {
                ParticleUtil.InitParticleColor(particleSystems, prefName);
            }
            
        }
        protected IEnumerator DisableObject<T>(T obj)
        {
            int _particleCounts = 77; // ������ ���� �ʱ�ȭ

            while (_particleCounts > 0)
            {
                _particleCounts = 0;
                foreach (var p in particleSystems) { _particleCounts += p.particleCount; }
                yield return null;
            }

            // ���� ������Ʈ ��Ȱ��ȭ (����)
            ObjectPool.Instance.ReturnObject(obj, name);
            UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[4], gameObject);
            yield break;
        }
    }
}

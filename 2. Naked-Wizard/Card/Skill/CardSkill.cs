using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Utility;

namespace LMS.Cards
{
    public partial class CardSkill // CommonSkill
    {
        public static IEnumerator HpHeal(GameObject obj, Vector3 direction, CardInfo info)
        {
            Debug.Log("HpHeal이 되었습니다.");
            yield break;
        }
        public static IEnumerator SingleFire(GameObject obj, Vector3 direction, float damage) // 기본 공격
        {
            var _spell = ObjectPool.Instance.GetObject<NormalProjectile>("Effect");
            UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[4], _spell.gameObject, true);

            _spell.Initialized(direction, obj.transform.position + CardBase.characterHeight, 30f, "Effect", damage);
            yield break;
        }
        public static IEnumerator MultipleFire(GameObject obj, Vector3 direction, float damage) // 기본 특별 공격
        {
            float[] _deg = new float[3] { 0f, 22.5f, -22.5f };
            for (int i = 0; i < 3; i++)
            {
                var _spell = ObjectPool.Instance.GetObject<NormalProjectile>("Effect");
                UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[4], _spell.gameObject, true);

                _spell.transform.rotation = Quaternion.LookRotation(direction);

                var _curRot = _spell.transform.eulerAngles;
                var _y = _curRot.y + _deg[i];
                _spell.transform.rotation = Quaternion.Euler(_curRot.x, _y, _curRot.z);

                _spell.Initialized(_spell.transform.forward, obj.transform.position + CardBase.characterHeight, 30f, "Effect", damage);
            }
            yield break;
        }
    }

    public partial class CardSkill // AttackSkill
    {
        public static IEnumerator SprayFire(GameObject obj, Vector3 direction, CardInfo info)
        {
            for (int i = 0; i < CardBase.sprayCount[info.cardLevel]; i++)
            {
                var _spell = ObjectPool.Instance.GetObject<NormalProjectile>("Effect");
                UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[4], _spell.gameObject, true);

                _spell.transform.rotation = Quaternion.LookRotation(obj.transform.forward);

                var _randDeg = Random.Range(-5f, 5f);
                var _curRot = _spell.transform.eulerAngles;
                var _y = _curRot.y + _randDeg;
                _spell.transform.rotation = Quaternion.Euler(_curRot.x, _y, _curRot.z);

                //var _dir = obj.transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), 0f, 0f);
                _spell.Initialized(_spell.transform.forward, obj.transform.position + CardBase.characterHeight, 30f, "Effect", info.damage, info);
                yield return new WaitForSeconds(info.executeTime / CardBase.sprayCount[info.cardLevel]);
            }
            yield break;
        }

        public static IEnumerator LinoRoar(GameObject obj, Vector3 direction, CardInfo info)
        {
            var _spell = ObjectPool.Instance.GetObject<LionRoar>("LionRoar");
            UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[1], _spell.gameObject, true);
            _spell.Initialized(obj.transform.position, info.damage, info.name);
            yield break;
        }

        public static IEnumerator Meteors(GameObject obj, Vector3 direction, CardInfo info)
        {
            var _spell = ObjectPool.Instance.GetObject<Meteors>("Meteors");
            UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[2], _spell.gameObject, true);
            _spell.Initialized(obj, obj.transform.position, info.damage, CardBase.meteorsCount[info.cardLevel], info.name);
            yield break;
        }

        public static IEnumerator Slash(GameObject obj, Vector3 direction, CardInfo info)
        {
            Manager.GameManager.Instance.ExecuteCoroutine(SkillAction.RushObject(obj, direction, CardBase.slashesCount[info.cardLevel]));

            for (int i = 0; i < CardBase.slashesCount[info.cardLevel]; i++)
            {
                var _randY = Random.Range(0f, 360f);
                var _randZ = Random.Range(0f, 360f);

                var _spell = ObjectPool.Instance.GetObject<Slash>("Slashes");
                UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[5], _spell.gameObject, true);
                _spell.Initialized(obj.transform.position, _randY, _randZ, info.damage, info.name);

                yield return new WaitForSeconds(0.1f);
            }
            yield break;
        }
    }
}

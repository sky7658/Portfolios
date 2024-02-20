using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LMS.Cards
{
    public class SkillAction
    {
        /// <summary>
        /// 특정 시간 동안 오브젝트 Scale을 조절합니다.
        /// </summary>
        /// <param name="targetT"></param>
        /// <param name="scale"></param>
        /// <param name="executeTime"></param>
        /// <returns></returns>
        public static IEnumerator ScaleMotion(Transform targetT, Vector3 scale, float executeTime)
        {
            float _elapsed = 0f;
            while (_elapsed < executeTime)
            {
                _elapsed += Time.smoothDeltaTime;
                targetT.transform.localScale = new Vector3(_elapsed / executeTime * scale.x, _elapsed / executeTime * scale.y, _elapsed / executeTime * scale.z);
                yield return null;
            }

            targetT.transform.localScale = scale;

            yield break;
        }
        /// <summary>
        /// 특정 시간이 지난 이후 다음 함수를 실행합니다.
        /// </summary>
        /// <param name="endTime"></param>
        /// <param name="obj"></param>
        /// <param name="delegateFun"></param>
        /// <returns></returns>
        public static IEnumerator RetentionTime(float endTime, Action delegateFun = null)
        {
            float _elapsed = 0f;

            while (_elapsed < endTime)
            {
                _elapsed += Time.smoothDeltaTime;
                yield return null;
            }
            if (delegateFun != null) delegateFun();
            yield break;
        }

        public static IEnumerator BounceOut(Monster obj, Vector3 direction, float distance)
        {
            var _oriPos = obj.transform.position;
            var _mon = obj;
            var _body = obj.nav;

            if(_body.velocity == Vector3.zero)
            {
                float _randY = UnityEngine.Random.Range(0f, 360f);
                direction = Quaternion.Euler(0f, _randY, 0f) * Vector3.forward;
            }

            //Debug.Log("direction : " + direction);
            //Debug.Log("direction * 50 : " + direction * 50f);
            //Debug.Log("first velocity : " + _body.velocity);

            if (_body == null)
            {
                Debug.Log("RigidBody가 없습니다.");
                yield break;
            }
            while (_mon != null && Vector3.Distance(_oriPos, _mon.transform.position) < distance)
            {
                if (_mon == null || _body.velocity == Vector3.zero) yield break;
                _body.velocity = direction * 50f;
                //Debug.Log("second velocity : " + _body.velocity);
                yield return null;
            }

            if (_mon == null) yield break;

            _body.velocity = Vector3.zero;
            yield break;
        }

        public static IEnumerator Teleport(GameObject obj, Vector3 pos, float executeTime, bool rigd, Action startEffect = null, Action EndEffect = null)
        {
            float _elapsed = 0f;
            Rigidbody _rb = obj.GetComponent<Rigidbody>();
            _rb.useGravity = rigd;
            _rb.velocity = Vector3.zero;

            if(startEffect != null) startEffect(); // Effect를 사용

            while(_elapsed < executeTime) 
            { 
                _elapsed += Time.smoothDeltaTime;
                yield return null;
            }

            obj.transform.position = pos;

            if (EndEffect != null) EndEffect(); // Effect를 사용

            yield break;
        }

        public static IEnumerator RushObject(GameObject obj, Vector3 dir, int takeCount)
        {
            var _elapsed = 0f;
            var _body = obj.GetComponent<Rigidbody>();

            if (_body == null)
            {
                Debug.Log("RigidBody가 없습니다.");
                yield break;
            }
            while(_elapsed < takeCount * 0.1f)
            {
                _elapsed += Time.smoothDeltaTime;
                _body.velocity = dir * 20f;
                yield return null;
            }

            _body.velocity = Vector3.zero;
            yield break;
        }
    }
}

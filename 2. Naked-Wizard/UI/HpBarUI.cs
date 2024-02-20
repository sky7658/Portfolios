 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Manager;

namespace LMS.UI
{
    public class HpBarUI : MonoBehaviour
    {
        private Image frontHpBar;
        private Image backHpBar;

        private float currentHp;
        private float maxHp;

        private float frontBarSpeed;
        private float backBarSpeed;

        private Coroutine frontCoroutine;
        private Coroutine backCoroutine;
        private Coroutine hpUICoroutine;

        private bool isWorld;
        private void Awake() // 처음 생성시에만
        {
            frontHpBar = transform.GetChild(1).GetComponent<Image>();
            backHpBar = transform.GetChild(0).GetComponent<Image>();
        }

        public void Initialized(float hp, bool isWorld)
        {
            currentHp = hp;
            this.maxHp = hp;
            this.isWorld = isWorld;

            if (this.isWorld) gameObject.SetActive(false);

            frontBarSpeed = 12f;
            backBarSpeed = 5f;

            OffCoroutineAll();
        }

        /// <summary>
        /// 이 클래스에 존재하는 코루틴을 전부 종료시킵니다.
        /// </summary>
        public void OffCoroutineAll()
        {
            Utility.UtilFunction.OffCoroutine(frontCoroutine);
            Utility.UtilFunction.OffCoroutine(backCoroutine);
            Utility.UtilFunction.OffCoroutine(hpUICoroutine);
        }

        /// <summary>
        /// HpBar를 업데이트 해줍니다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="getDamage"></param>
        public void UpdateHpBar(float value, bool getDamage = true)
        {
            if (currentHp <= 0) return;

            Utility.UtilFunction.OffCoroutine(frontCoroutine);
            Utility.UtilFunction.OffCoroutine(backCoroutine);

            if (getDamage) currentHp -= value;
            else
            {
                currentHp += value;
                if(currentHp > maxHp) currentHp = maxHp;
            }

            if(currentHp <= 0 && isWorld) { gameObject.SetActive(false); return; }

            if (isWorld) DrawUI();

            frontCoroutine = GameManager.Instance.ExecuteCoroutine(HpUpdate(frontHpBar, frontBarSpeed));
            backCoroutine = GameManager.Instance.ExecuteCoroutine(HpUpdate(backHpBar, backBarSpeed));
        }

        /// <summary>
        /// HpBar를 일정 시간동안만 노출되게 해줍니다.
        /// </summary>
        private void DrawUI()
        {
            Utility.UtilFunction.OffCoroutine(hpUICoroutine);

            gameObject.SetActive(true);
            hpUICoroutine = GameManager.Instance.ExecuteCoroutine(DrawHpUI());
        }
        private IEnumerator DrawHpUI()
        {
            float _elapsed = 0f;
            while (_elapsed < 2f)
            {
                // 항상 카메라 방향으로 바라보기
                var _cam = Camera.main.transform;
                transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);

                _elapsed += Time.smoothDeltaTime;

                yield return null;
            }

            gameObject.SetActive(false);
            yield break;
        }

        private IEnumerator HpUpdate(Image hpBar, float speed)
        {
            float _value = currentHp / maxHp;
            if (_value < hpBar.fillAmount)
            {
                while (hpBar.fillAmount - _value > 0.0001f)
                {
                    hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, _value, speed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                while (_value - hpBar.fillAmount > 0.0001f)
                {
                    hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, _value, speed * Time.deltaTime);
                    yield return null;
                }
            }

            hpBar.fillAmount = _value;
            yield break;
        }
    }
}

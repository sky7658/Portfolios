using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Manager;

namespace LMS.UI
{
    public class ExpBarUI : MonoBehaviour
    {
        private Image frontExpBar;

        public float frontBarSpeed;

        private Coroutine frontCoroutine;

        private void Awake() // 처음 생성시에만
        {
            frontExpBar = transform.GetChild(0).GetComponent<Image>();
            Initialized();
        }

        public void Initialized()
        {
            frontBarSpeed = 12f;

            Utility.UtilFunction.OffCoroutine(frontCoroutine);
        }

        public void UpdateExpBar(float value)
        {
            Utility.UtilFunction.OffCoroutine(frontCoroutine);

            frontCoroutine = GameManager.Instance.ExecuteCoroutine(ExpUpdate(frontExpBar, frontBarSpeed, value));
        }

        private IEnumerator ExpUpdate(Image expBar, float speed, float value)
        {
            if (value < expBar.fillAmount)
            {
                while (expBar.fillAmount - value > 0.0001f)
                {
                    expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, value, speed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                while (value - expBar.fillAmount > 0.0001f)
                {
                    expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, value, speed * Time.deltaTime);
                    yield return null;
                }
            }

            expBar.fillAmount = value;
            yield break;
        }
    }
}
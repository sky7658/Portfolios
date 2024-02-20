using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Cards;

namespace LMS.UI
{
    public class CardUI
    {
        Vector3 leftLocalPos;
        Vector3 leftLocalRot;
        Vector3 rightLocalPos;
        Vector3 rightLocalRot;
        public CardUI()
        {
            leftLocalPos = new Vector3(-145, -145, 0);
            leftLocalRot = new Vector3(0, 0, 15);
            rightLocalPos = new Vector3(145, -145, 0);
            rightLocalRot = new Vector3(0, 0, -15);

            infoDefaultText = "Grade : {0}\nSpend Mp : {1}\nCount : {2}\nProperty : {3}\nInfo : {4}";
        }
        /// <summary>
        /// 핸드에 있는 카드의 위치를 정리하고 그 위치로 이동해줍니다.
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="handType"></param>
        public void CardAligment(List<Card> cards, bool handType)
        {
            if (cards.Count < 1) return; // 카드가 없을 경우 실행하지 않음

            float[] objLerps = new float[cards.Count];
            Vector3 _dis;

            if (handType) _dis = Vector3.zero;
            else _dis = -CardBase.handDistance;

            // 위치 리턴
            switch (cards.Count)
            {
                case 1:
                    objLerps = new float[] { 0.5f };
                    break;
                case 2:
                    objLerps = new float[] { 0.27f, 0.73f };
                    break;
                default:
                    float interval = 1f / (cards.Count - 1);
                    for (int i = 0; i < cards.Count; i++)
                    {
                        objLerps[i] = interval * i;
                    }
                    break;
            }

            for (int i = 0; i < cards.Count; i++)
            {
                var _targetPos = Vector3.Lerp(leftLocalPos, rightLocalPos, objLerps[i]);
                var _targetRot = Quaternion.identity;
                if (cards.Count > 2)
                {
                    float curve = Mathf.Sqrt(Mathf.Pow(1, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                    _targetPos.y += curve;
                    _targetRot = Quaternion.Slerp(Quaternion.Euler(leftLocalRot), Quaternion.Euler(rightLocalRot), objLerps[i]);
                }
                cards[i].MoveTo(_targetPos + _dis, _targetRot, 0.2f);
            }
        }

        private string infoDefaultText;
        public void UpdateInfo(Text text, CardInfo info = null)
        {
            if (text == null)
            {
                return;
            }
            if(info == null)
            {
                text.text = string.Format(infoDefaultText, "", "", "", "", "");
                return;
            }
            text.text = string.Format(infoDefaultText, info.grade, info.spendMP, info.count, info.property, info.name);
        } 
    }

    public class CardAction
    {
        /// <summary>
        /// 오브젝트의 위치와 스케일을 시간에 따라 변경시킵니다.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="targetPos"></param>
        /// <param name="targetRot"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator MoveToAction(GameObject obj, Vector3 targetPos, Quaternion targetRot, float duration, bool isPos = true, bool isRot = true)
        {
            Vector3 _startPos = obj.transform.localPosition;
            Quaternion _startRot = obj.transform.localRotation;

            float _elapsed = 0.0f;
            while (_elapsed < duration)
            {
                if (obj == null) yield break;
                _elapsed += Time.smoothDeltaTime;
                if (isPos) obj.transform.localPosition = Vector3.Lerp(_startPos, targetPos, _elapsed / duration);
                if (isRot) obj.transform.localRotation = Quaternion.Slerp(_startRot, targetRot, _elapsed / duration);
                yield return null;
            }

            if(obj == null) yield break;

            if (isPos) obj.transform.localPosition = targetPos;
            if (isRot) obj.transform.localRotation = targetRot;

            yield break;
        }

        /// <summary>
        /// 시간에 따른 카드 마스크의 스케일을 조절합니다.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static IEnumerator DelayAction(Card card)
        {
            float _t = 0f;

            while(_t < card.cardInfo.delayTime)
            {
                card.cardMask.transform.localScale = new Vector3(1f, (card.cardInfo.delayTime - _t) / card.cardInfo.delayTime, 1f);
                _t += Time.smoothDeltaTime;
                yield return null;
            }

            if (card == null) yield break;

            card.cardMask.transform.localScale = new Vector3(1f, 1f, 1f);
            card.cardMask.gameObject.SetActive(false);
            card.delayEnabled = false;

            yield break;
        }
        /// <summary>
        /// 카드를 선택/미선택 시 스케일을 조절합니다.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="isSelect"></param>
        /// <returns></returns>
        public static IEnumerator SelectAction(Card card, bool isSelect = false)
        {
            float _t = 0f;
            Vector3 _scale;

            if(!isSelect)
            {
                while(_t < 0.2f)
                {
                    if(card == null) yield break;
                    card.cardImg.transform.localScale = new Vector3(0.8f + _t, 0.8f + _t, 0.8f);
                    _t += Time.smoothDeltaTime;
                    yield return null;
                }
                _scale = new Vector3(1f, 1f, 0.8f);
            }
            else
            {
                while (_t < 0.2f)
                {
                    if (card == null) yield break;
                    card.cardImg.transform.localScale = new Vector3(1f - _t, 1f - _t, 0.8f);
                    _t += Time.smoothDeltaTime;
                    yield return null;
                }
                _scale = new Vector3(0.8f, 0.8f, 0.8f);
            }

            if (card == null) yield break;
            card.cardMask.transform.localScale = _scale;

            yield break;
        }
    }
}
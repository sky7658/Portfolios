using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.UI;
using LMS.Manager;

namespace LMS.Cards
{
    public class Card : MonoBehaviour
    {
        public CardInfo cardInfo;
        public RawImage cardImg { get; set; }
        public Image cardMask { get; set; }
        public Text levelText { get; set; }
        public Text nameText { get; set; }

        // 카드 스킬
        public bool delayEnabled { get; set; }
        public bool isHighlight { get; set; }
        public delegate IEnumerator ActionDelegate(GameObject obj, Vector3 direction, CardInfo info);
        private ActionDelegate skill { get; set; }

        private string textString = "{0}";

        private void Awake()
        {
            cardImg = GetComponent<RawImage>();
            cardMask = transform.GetChild(0).GetComponent<Image>();
            expBarUI = transform.GetChild(1).GetComponent<ExpBarUI>();
            levelText = transform.GetChild(2).GetComponent<Text>();
            nameText = transform.GetChild(3).GetComponent<Text>();
        }

        public void Initialized(string imgName, CardProperty property = CardProperty.NONE)
        {
            delayEnabled = false;
            isHighlight = false;

            cardImg.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            cardMask.transform.localScale = new Vector3(1f, 1f, 1f);

            transform.localPosition = CardBase.InitCardPos;

            SetCardImg(imgName);
            cardInfo = new CardInfo(1f, -1, Grade.EPIC, CardBase.skillTypes[imgName], 0, 0f, property);
            SetCardSkill();

            levelText.text = string.Format(textString, cardInfo.cardLevel + 1);
            nameText.text = string.Format(textString, cardInfo.name);

            cardMask.gameObject.SetActive(false);
        }

        private ExpBarUI expBarUI;
        /// <summary>
        /// Card의 경험치를 업데이트 해줍니다.
        /// </summary>
        /// <param name="value"></param>
        public void ExpUpdate(float value)
        {
            if (cardInfo.cardLevel == 4) return;

            cardInfo.currentExp += value;
            if(cardInfo.currentExp >= cardInfo.maxExp)
            {
                while(cardInfo.currentExp >= cardInfo.maxExp)
                {
                    cardInfo.cardLevel++;
                    cardInfo.currentExp -= cardInfo.maxExp;
                    if(cardInfo.cardLevel < 4) cardInfo.maxExp = CardBase.cardLevelMaxExp[cardInfo.cardLevel];
                }

                levelText.text = string.Format(textString, cardInfo.cardLevel + 1); // Level Text Update

                if (cardInfo.cardLevel == 4) cardInfo.currentExp = cardInfo.maxExp;

                cardInfo.damage = CardBase.cardLevelDamage[cardInfo.type][cardInfo.cardLevel];
            }

            expBarUI.UpdateExpBar(cardInfo.currentExp / cardInfo.maxExp);
        }

        private void SetCardSkill()
        {
            switch (cardInfo.type)
            {
                case SkillType.LIONROAR:
                    skill = CardSkill.LinoRoar;
                    break;
                case SkillType.METEORS:
                    skill = CardSkill.Meteors;
                    break;
                case SkillType.SLASHES:
                    skill = CardSkill.Slash;
                    break;
                case SkillType.SPRAY:
                    skill = CardSkill.SprayFire;
                    break;
                case SkillType.HEAL:
                    skill = CardSkill.HpHeal;
                    break;
            }
        }

        private void SetCardImg(string name)
        {
            cardImg.texture = GameManager.Instance.ResourceLoadImg(name);
            if(cardImg.texture == null)
            {
                Debug.Log("이미지가 없습니다.");
                return;
            }
        }

        public void HighlightTrigger()
        {
            GameManager.Instance.ExecuteCoroutine(CardAction.SelectAction(this, isHighlight));
            if (isHighlight) isHighlight = false;
            else isHighlight = true;
        }

        /// <summary>
        /// 카드의 스킬을 실행해주는 함수
        /// </summary>
        /// <param name="obj"> 스킬을 쓰는 대상</param>
        public void ExecuteSkill(GameObject obj, Vector3 direction)
        {
            delayEnabled = true;
            if(cardInfo.count - 1 != 0) // 카드를 사용했을 때 갯수가 남아있다면 실행
            {
                cardMask.gameObject.SetActive(true);
                GameManager.Instance.ExecuteCoroutine(CardAction.DelayAction(this));
            }
            GameManager.Instance.ExecuteCoroutine(skill(obj, direction, cardInfo));
        }

        /// <summary>
        /// 카드의 위치를 이동해주는 함수
        /// </summary>
        /// <param name="targetPos"> </param>
        /// <param name="targetRot"> </param>
        /// <param name="duration"> 실행 시간</param>
        public void MoveTo(Vector3 targetPos, Quaternion targetRot, float duration)
        {
            GameManager.Instance.ExecuteCoroutine(CardAction.MoveToAction(gameObject, targetPos, targetRot, duration));
        }
    }
}
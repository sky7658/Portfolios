using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Cards;
using System;

namespace LMS.UI
{
    public class PlayerUIManger
    {
        private CardHandler cHandler;
        private Text infoText;

        public PlayerUIManger()
        {
            cHandler = new CardHandler();
        }
        public void CreateInfoText()
        {
            //if(infoText == null)
            //{
            //    var newText = GameObject.Instantiate(Manager.GameManager.Instance.ResourceLoadObj("Text")).GetComponent<Text>();
            //    newText.transform.parent = Manager.DataManager.Instance.test;
            //    newText.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
            //    infoText = newText;
            //}
        }
        public int GetCardCount() => cHandler.GetCardCount();
        /// <summary>
        /// Card List에 카드를 추가합니다.
        /// </summary>
        public void PushCard(int index, CardProperty property = CardProperty.NONE) => cHandler.PushCard(index, property);
        /// <summary>
        /// Card List에 저장된 특정 카드를 삭제합니다.
        /// </summary>
        /// <param name="index"></param>
        public void PopCard() => cHandler.PopCard(infoText);
        /// <summary>
        /// 선택 된 카드를 사용합니다.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="direction"></param>
        public bool UseCard(GameObject obj, Vector3 direction) => cHandler.UseCard(obj, direction, infoText);
        /// <summary>
        /// Card List에 있는 특정 카드를 선택합니다.
        /// </summary>
        /// <param name="index"></param>
        public void SelectCard(int index) => cHandler.SelectCard(infoText, index);
        /// <summary>
        /// 선택된 카드의 레벨을 1업 해줍니다.
        /// </summary>
        public void SelectCardLevelUp() => cHandler.SelectCardLevelUp();
        /// <summary>
        /// Hand에 있는 Card List를 보이게 하거나 숨기게 해줍니다.
        /// </summary>
        public void SetHand() => cHandler.SetHand();

        public void ComboAttacks(GameObject obj, Action del) => cHandler.ComboAttacks(obj, del);

        public bool disableMovement => cHandler.disableMovement;
    }
}



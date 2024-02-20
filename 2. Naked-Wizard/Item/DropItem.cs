using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Manager;
using LMS.Utility;

namespace LMS.Item
{
    enum ItemType { LionRoar, Meteor, Slash, Spray, Potion, None }
    public class DropItem : MonoBehaviour
    {
        private RawImage image;
        private Texture[] textures;

        [SerializeField] private ItemType type;
        private Vector3 upPos, downPos;
        private bool arrow;

        private Coroutine bounceCoroutine;

        private void Awake()
        {
            image = transform.GetChild(0).GetComponent<RawImage>();
        }

        public void Initialized(Vector3 pos)
        {
            textures = new Texture[] { GameManager.Instance.ResourceLoadImg("HP"), GameManager.Instance.ResourceLoadImg("CardBack") };

            UtilFunction.OffCoroutine(bounceCoroutine);

            type = CreateItemInfo();

            if(type == ItemType.None)
            {
                ObjectPool.Instance.ReturnObject(this, "Item");
                UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[6], gameObject);
                return;
            }

            transform.position = pos + new Vector3(0f, 1f, 0f);

            upPos = transform.position + new Vector3(0f, 0.2f, 0f);
            downPos = transform.position + new Vector3(0f, -0.2f, 0f);

            bounceCoroutine = GameManager.Instance.ExecuteCoroutine(BounceObject());
        }

        private int ratio = 1;
        private int[] itemRatio = new int[3] { 4, 6, 10 }; // None, Potion, Card
        private int[] cardRatio = new int[4] { 3, 5, 7, 10 }; // LionRoar, Meteors, Slash, Spray
        private ItemType CreateItemInfo()
        {
            int _itemRand = Random.Range(0, ratio * 10);

            if(_itemRand < itemRatio[0])
            {
                return ItemType.None;
            }
            else if (_itemRand < itemRatio[1])
            {
                image.texture = textures[0];
                return ItemType.Potion;
            }
            else
            {
                int _cardRand = Random.Range(0, ratio * 100);

                image.texture = textures[1];

                if (_cardRand < cardRatio[0] * 10)
                {
                    return ItemType.LionRoar;
                }
                else if (_cardRand < cardRatio[1] * 10)
                {
                    return ItemType.Meteor;
                }
                else if (_cardRand < cardRatio[2] * 10)
                {
                    return ItemType.Slash;
                }
                else
                {
                    return ItemType.Spray;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                other.TryGetComponent(out Player _other);

                if(type == ItemType.Potion)
                {
                    if (_other.hp == _other.maxHp) return; // 체력이 100%라면 획득 X
                    _other.RecoveryHp(30f); // 임의 값 회복
                }
                else
                {
                    if (_other.playerUIManger.GetCardCount() == Cards.CardBase.maxCardCount) return; // 카드 갯수가 꽉찼다면 획득 X
                    _other.playerUIManger.PushCard((int)type);
                }

                UtilFunction.OffCoroutine(bounceCoroutine);
                ObjectPool.Instance.ReturnObject(this, "Item");
                UtilFunction.TurnOnOff(ObjectPool.Instance.objectInfos[6], gameObject);
            }
        }

        private IEnumerator BounceObject()
        {
            while(true)
            {
                if (arrow)
                {
                    GameManager.Instance.ExecuteCoroutine(UI.CardAction.MoveToAction(gameObject, upPos, transform.rotation, 1f, true, false));

                    var t = 0f;
                    while(t < 1f)
                    {
                        var _cam = Camera.main.transform;
                        transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);
                        t += Time.smoothDeltaTime;
                        yield return null;
                    }

                    arrow = false;
                }
                else
                {
                    GameManager.Instance.ExecuteCoroutine(UI.CardAction.MoveToAction(gameObject, downPos, transform.rotation, 1f, true, false));

                    var t = 0f;
                    while (t < 1f)
                    {
                        var _cam = Camera.main.transform;
                        transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);
                        t += Time.smoothDeltaTime;
                        yield return null;
                    }

                    arrow = true;
                }
                yield return null;
            }
        }
    }

}


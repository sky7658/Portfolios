using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Cards;
using LMS.Item;

namespace LMS.Utility
{
    [System.Serializable]
    public class ObjectInfo
    {
        public string name;
        public Transform ableParent;
        public Transform disableParent;
        public int count;
    }

    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        [SerializeField] public List<ObjectInfo> objectInfos = new List<ObjectInfo>();

        private Dictionary<string, object> pools = new Dictionary<string, object>();

        private void Start()
        {
            pools.Add(objectInfos[0].name, new GenericObjectPool<Card>(objectInfos[0]));
            pools.Add(objectInfos[1].name, new GenericObjectPool<LionRoar>(objectInfos[1]));
            pools.Add(objectInfos[2].name, new GenericObjectPool<Meteors>(objectInfos[2]));
            pools.Add(objectInfos[3].name, new GenericObjectPool<ParticleSystem>(objectInfos[3]));
            pools.Add(objectInfos[4].name, new GenericObjectPool<NormalProjectile>(objectInfos[4]));
            pools.Add(objectInfos[5].name, new GenericObjectPool<Slash>(objectInfos[5]));
            pools.Add(objectInfos[6].name, new GenericObjectPool<DropItem>(objectInfos[6]));
        }

        /// <summary>
        /// 해당 타입의 오브젝트를 받아옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T GetObject<T>(string name)
        {
            if (pools.TryGetValue(name, out var objectPool) && objectPool is GenericObjectPool<T> typedObjectPool)
            {
                return typedObjectPool.GetObject();
            }
            Debug.Log("오브젝트를 찾을 수 없습니다.");
            return default(T);
        }

        /// <summary>
        /// 해당 타입의 오브젝트를 리턴 합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnObj"></param>
        /// <param name="name"></param>
        public void ReturnObject<T>(T returnObj, string name)
        {
            if(pools.TryGetValue(name, out var objectPool) && objectPool is GenericObjectPool<T> typedObjectPool)
            {
                typedObjectPool.ReturnObject(returnObj);
                return;
            }
            Debug.Log("오브젝트를 찾을 수 없습니다.");
        }
    }

    public class GenericObjectPool<T>
    {
        private ObjectInfo info;
        private Queue<T> objectQueue = new Queue<T>();

        public GenericObjectPool(ObjectInfo info)
        {
            this.info = info;

            for(int i = 0; i < info.count; i++)
            {
                objectQueue.Enqueue(CreateObject<T>());
            }
        }

        private T CreateObject<T>()
        {
            var newObj = GameObject.Instantiate(Manager.GameManager.Instance.ResourceLoadObj(info.name));
            UtilFunction.TurnOnOff(info, newObj, false);

            return newObj.GetComponent<T>();
        }

        /// <summary>
        /// 오브젝트를 받아옵니다.
        /// </summary>
        /// <returns></returns>
        public T GetObject()
        {
            if(objectQueue.Count > 0)
                return objectQueue.Dequeue();
            else
                return CreateObject<T>();
        }

        /// <summary>
        /// 오브젝트를 리턴합니다.
        /// </summary>
        /// <param name="returnObj"></param>
        public void ReturnObject(T returnObj) => objectQueue.Enqueue(returnObj);
    }

    public class UtilFunction
    {
        public static void TurnOnOff(ObjectInfo info, GameObject obj, bool set = false)
        {
            obj.SetActive(set);
            if (set)
            {
                if (info.ableParent != null) obj.transform.SetParent(info.ableParent);
                else obj.transform.SetParent(null);
            }
            else
            {
                if (info.disableParent != null) obj.transform.SetParent(info.disableParent);
                else obj.transform.SetParent(null);
            }
        }

        public static void OffCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                Manager.GameManager.Instance.QuitCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}

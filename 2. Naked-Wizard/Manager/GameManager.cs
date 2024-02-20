using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LMS.Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;

                Initialized();

                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public static GameManager Instance
        {
            get
            {
                if(instance == null)
                {
                    return null;
                }
                return instance;
            }
        }

        private void Initialized()
        {
            ResourceLoadImgAll();
            ResourceLoadObjAll();
        }

        /// <summary>
        /// �ڷ�ƾ�� �������ִ� �Լ�
        /// </summary>
        /// <param name="coroutine"> �����ϰ� ���� IEnumerator</param>
        public Coroutine ExecuteCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void QuitCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        private Dictionary<string, Texture> dicImgs = new Dictionary<string, Texture>();
        private Texture[] imgs;
        private void ResourceLoadImgAll()
        {
            imgs = Resources.LoadAll<Texture>("Image");
            //object[] loadimgs = Resources.LoadAll<Texture>("Image");
            //foreach(object item in loadimgs)
            //{
            //    dicimgs.add(item.tostring(), item as texture);
            //}
        }

        private Dictionary<string, GameObject> dicObjs = new Dictionary<string, GameObject>();
        private GameObject[] objs;
        private void ResourceLoadObjAll()
        {
            objs = Resources.LoadAll<GameObject>("Prefabs");
            //object[] loadObjs = Resources.LoadAll<GameObject>("Prefabs");
            //foreach(object item in loadObjs)
            //{
            //    dicObjs.Add(item.ToString(), item as GameObject);
            //}
        }

        /// <summary>
        /// �̹����� �޾ƿ��ִ� �Լ�
        /// </summary>
        /// <param name="imgName"></param>
        /// <returns></returns>
        public Texture ResourceLoadImg(string imgName)
        {
            //if (!dicImgs.ContainsKey(imgName))
            //{
            //    Debug.Log("�����");
            //    return null;
            //}
            //var loadImg = dicImgs[imgName];
            //return loadImg;
            foreach(var img in imgs)
            {
                if (img.name == imgName) return img;
            }
            Debug.Log("ã�� �� ����");
            return null;
        }

        /// <summary>
        /// �������� �޾ƿ��ִ� �Լ�
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public GameObject ResourceLoadObj(string objName)
        {
            //if (!dicObjs.ContainsKey(objName))
            //{
            //    Debug.Log("�����");
            //    return null;
            //}
            //var loadObj = dicObjs[objName];
            //return loadObj;
            foreach(var obj in objs)
            {
                if (obj.name == objName) return obj;
            }

            Debug.Log("ã�� �� ����");
            return null;
        }
    }
}
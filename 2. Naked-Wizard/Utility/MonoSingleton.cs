using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Utility
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if(instance == null)
                {
                    instance = new GameObject("@" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                }
                return instance;
            }
        }
    }
}
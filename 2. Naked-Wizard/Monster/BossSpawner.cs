using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private GameObject spawnEffect;

    private void Start()
    {
        spawnEffect = LMS.Manager.GameManager.Instance.ResourceLoadObj("SpawnEffect");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var _effect = Instantiate(spawnEffect, boss.transform.position, Quaternion.identity);
            _effect.transform.localScale *= 3;
            boss.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
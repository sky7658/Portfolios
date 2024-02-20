using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Cards
{
    public abstract class Projectile : SkillEffect
    {
        protected float damage;
        
        protected Rigidbody rigidBody;
        protected Collider col;
        public override void Initialized(Vector3 arrow, Vector3 pos, float speed, string prefName, float damage, CardInfo info = null)
        {
            base.Initialized(arrow, pos, speed, prefName, damage, info);
            name = "Effect";
            transform.position = pos; // ������
            rigidBody.velocity = arrow * speed; // ���ư��� ����
            col.enabled = true;
            transform.rotation = Quaternion.LookRotation(arrow); // �ٶ󺸴� ����

            this.damage = damage;
        }
        protected override void Awake()
        {
            base.Awake();
            rigidBody = GetComponent<Rigidbody>();
        }

        protected abstract void OnTriggerEnter(Collider other);
        protected virtual void Release<T>(T obj)
        {
            col.enabled = false;
            rigidBody.velocity = Vector3.zero;
            Manager.GameManager.Instance.ExecuteCoroutine(DisableObject(obj));
        }
    }
}
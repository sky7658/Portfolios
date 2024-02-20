using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using LMS.Manager;
using LMS.Utility;
using LMS.UI;
using SHY.Enemy;

namespace LMS.Enemy
{
    public class Boss : MonoBehaviour, IDamageable
    {
        public BossStateMachine state;
        public Transform target;

        private float currentHp;
        private float maxHp;

        private float damage;
        private float skillDamage;
        private float attackDistance;
        private float rushDistance;
        private float attackCoolDown;
        private float skillCoolDown;

        private Coroutine attackCoroutine;
        private Coroutine skillCoroutine;
        private Coroutine attackCoolCoroutine;
        private Coroutine skillCoolCoroutine;

        public bool isAttackable { get; set; }
        public bool isRushable { get; set; }

        public Animator anim { get; set; }

        private HitBox hitBox;
        private HpBarUI hpBarUI;

        public NavMeshAgent nav { get; set; }
        [SerializeField] private BoxCollider boxCollider;
        public BossStateName BS;


        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            hpBarUI = transform.GetChild(0).GetChild(0).GetComponent<HpBarUI>();
            hitBox = transform.GetChild(1).GetComponent<HitBox>();
        }

        private void Start()
        {
            Initialized();
        }

        private void FixedUpdate()
        {
            state.UpdateState();
        }

        public void Initialized()
        {
            OffAllCoroutine();

            state = new BossStateMachine(BossStateName.Idle, new SHY.Enemy.Idle(), this);

            state.AddState(BossStateName.Move, new SHY.Enemy.Move());
            state.AddState(BossStateName.Attack, new SHY.Enemy.Attack());
            state.AddState(BossStateName.Rush, new Rush());
            state.AddState(BossStateName.Die, new Die());

            state.ChangeState(BossStateName.Move);

            maxHp = 1000f; // 임의
            currentHp = maxHp;

            damage = 10f; // 임의
            attackDistance = 8f;
            attackCoolDown = 3f; // 임의

            skillDamage = 40f; // 임의
            rushDistance = 15f;
            skillCoolDown = 20f;  // 임의

            isRushable = false;
            isAttackable = false;

            hpBarUI.Initialized(maxHp, true);
        }

        public void TakeDamage(float damage, Vector3 reactVec)
        {
            currentHp -= damage;
            hpBarUI.UpdateHpBar(damage);

            if(currentHp <= 0) // 죽었을 때
            {
                // 상태 업데이트
                state.ChangeState(BossStateName.Die);
                return;
            }
        }

        public void RecoveryHp(float value)
        {
            currentHp += value;
            if(currentHp > maxHp) currentHp = maxHp;
            hpBarUI.UpdateHpBar(value, false);
        }

        public float GetDamage()
        {
            return damage;
        }

        public void DecideState()
        {
            //일정 거리안에서 둘 다 쿨타임 찼을 떄
            var _distance = Vector3.Distance(target.position, transform.position);
            if(_distance < rushDistance && !isRushable)
            {
                state.ChangeState(BossStateName.Rush);
                return;
            }
            if(_distance < attackDistance && !isAttackable)
            {
                state.ChangeState(BossStateName.Attack);
                return;
            }
            if (_distance > attackDistance && state._state.stateName == BossStateName.Idle){
                state.ChangeState(BossStateName.Move);
                return;
            }
        }

        public void Attack()
        {
            if (isAttackable) return;

            isAttackable = true;

            UtilFunction.OffCoroutine(attackCoroutine);
            UtilFunction.OffCoroutine(attackCoolCoroutine);

            attackCoroutine = GameManager.Instance.ExecuteCoroutine(Swing());
            attackCoolCoroutine = GameManager.Instance.ExecuteCoroutine(CoolDown(() => isAttackable = false, attackCoolDown));
        }
        
        public void ExecuteSkill()
        {
            if (isRushable) return;

            isRushable = true;

            //hitBox.Initialized(transform, Color.red, 5f, 10f, 0.5f);

            UtilFunction.OffCoroutine(skillCoroutine);
            UtilFunction.OffCoroutine(skillCoolCoroutine);

            skillCoroutine = GameManager.Instance.ExecuteCoroutine(Rush());
            skillCoolCoroutine = GameManager.Instance.ExecuteCoroutine(CoolDown(() => isRushable = false, skillCoolDown));
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                float _damage = 0f;

                if (state._state.stateName == BossStateName.Attack) _damage = damage;
                else if (state._state.stateName == BossStateName.Rush) _damage = skillDamage;

                if (other.TryGetComponent(out IDamageable damageable))
                {
                    if (_damage == 0f) return;
                    damageable.TakeDamage(_damage, Vector3.zero);
                    OffAttack();
                }
            }
        }

        private void OffAllCoroutine()
        {
            UtilFunction.OffCoroutine(attackCoroutine);
            UtilFunction.OffCoroutine(skillCoroutine);
            UtilFunction.OffCoroutine(attackCoolCoroutine);
            UtilFunction.OffCoroutine(skillCoolCoroutine);
        }

        private IEnumerator Swing()
        {
            //isAttackable = false;
            yield return new WaitForSeconds(attackCoolDown);
            //isAttackable = true;
            yield break;
        }

        private IEnumerator CoolDown(Action action, float coolTime)
        {
            yield return new WaitForSeconds(coolTime);
            action?.Invoke();
            yield break;
        }

        private IEnumerator Rush()
        {
            float _elpased = 0f;

            while (_elpased < 1f)
            {
                _elpased += Time.smoothDeltaTime;
                nav.velocity = transform.forward * 20f;
                yield return null;
            }

            nav.velocity = Vector3.zero;
            // 상태 바꾸기
            ToIdle();
            yield break;
        }

        public void ToMove() => state.ChangeState(BossStateName.Move);
        public void ToIdle() => state.ChangeState(BossStateName.Idle);
        public void OnAttack() => boxCollider.enabled = true;
        public void OffAttack() => boxCollider.enabled = false;
    }
}

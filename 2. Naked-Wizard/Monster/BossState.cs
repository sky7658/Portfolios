using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Enemy;

// 상태 인터페이스 클래스

namespace SHY.Enemy 
{
    public enum BossStateName
    {
        Idle,
        Move,
        Attack,
        Rush,
        Die
    };

    public interface BossState
    {
        public BossStateName stateName { get; }
        public void Enter(Boss boss);
        public void Action(Boss boss);
        public void Exit(Boss boss);
    }
    public class Idle : BossState
    {
        float elapsed;
        public BossStateName stateName { get; } = BossStateName.Idle;
        public void Enter(Boss boss)
        {
            boss.nav.isStopped = true;
            elapsed = 0f;
        }
        public void Action(Boss boss)
        {
            boss.DecideState();
            elapsed += Time.smoothDeltaTime;

            Vector3 _directionToTarget = boss.target.transform.position - boss.transform.position;
            Quaternion _targetRoation = Quaternion.LookRotation(_directionToTarget);
            
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, _targetRoation, elapsed / 0.5f);
        }
        public void Exit(Boss boss) { }
    }

    public class Move : BossState
    {
        public BossStateName stateName { get; } = BossStateName.Move;
        public void Enter(Boss boss)
        {
            boss.anim.SetBool("isMove", true);
            boss.nav.isStopped = false;
        }
        public void Action(Boss boss)
        {
            boss.nav.SetDestination(boss.target.position);
            boss.DecideState();
        }
        public void Exit(Boss boss)
        {
            boss.anim.SetBool("isMove", false);
            boss.nav.isStopped = true;
        }
    }

    public class Attack : BossState
    {
        public BossStateName stateName { get; } = BossStateName.Attack;
        public void Enter(Boss boss)
        {
            boss.anim.SetBool("isMove", false);
            boss.anim.SetTrigger("doAttack");
            boss.nav.isStopped = true;
            boss.Attack();
        }
        public void Action(Boss boss) { }
        public void Exit(Boss boss){
            boss.nav.isStopped = false;
        }
    }

    public class Rush : BossState
    {
        public BossStateName stateName { get; } = BossStateName.Rush;
        public void Enter(Boss boss)
        {
            boss.anim.SetBool("isMove", false);
            boss.anim.SetTrigger("doRush");
            boss.nav.isStopped = true;
            boss.ExecuteSkill();
        }
        public void Action(Boss boss) { }
        public void Exit(Boss boss)
        {
            boss.nav.isStopped = false;
        }
    }

    public class Die : BossState
    {
        public BossStateName stateName { get; } = BossStateName.Die;
        public void Enter(Boss boss)
        {
            boss.anim.SetBool("isMove", false);
            boss.anim.SetTrigger("doDie");
            boss.nav.isStopped = true;
            FadeManager.Instance.LoadScene(0);
        }
        public void Action(Boss boss) { }
        public void Exit(Boss boss) { }
    }
}

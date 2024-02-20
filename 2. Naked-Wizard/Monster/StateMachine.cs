using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Enemy;

namespace SHY.Enemy
{
    public class BossStateMachine
    {
        public Boss boss;
        public BossState _state { get; private set; }
        private Dictionary<BossStateName,BossState> _states = new Dictionary<BossStateName, BossState>();

        public BossStateMachine(BossStateName stateName, BossState state, Boss boss)
        {
            this.boss = boss;
            _states.Add(stateName, state);
            _state = state;
        }

        public void AddState(BossStateName stateName, BossState state)
        {
            if (!_states.ContainsKey(stateName))
            {
                _states.Add(stateName, state);
            }
        }

        public bool GetState(BossStateName stateName)
        {
            if (_states[stateName] == _state)
                return true;

            return false;
        }

        public void DeleteState(BossStateName stateName)
        {
            if (_states.ContainsKey(stateName))
            {
                _states.Remove(stateName);
            }
        }

        public void ChangeState(BossStateName stateName)
        {
            if (_state == _states[stateName]){
                return;
            }
            _state?.Exit(boss);
            if (_states.TryGetValue(stateName, out BossState state))
            {
                _state = state;
                boss.BS = stateName;
            }
            _state?.Enter(boss);
        }

        public void UpdateState()
        {
            _state?.Action(boss);
        }
    }
}
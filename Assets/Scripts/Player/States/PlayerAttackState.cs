using Assets.Scripts.Statemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackState : State
    {
        PlayerController player;

        public PlayerAttackState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Attacking";
            subStateMachine = new StateMachine();
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
}
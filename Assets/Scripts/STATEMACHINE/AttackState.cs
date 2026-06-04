using UnityEngine;

namespace HSM
{
    public abstract class AttackState : State
    {
        protected AttackState(Entity entity) : base(entity)
        {
            
        }

        public abstract void StartAttack();
    }
}
using UnityEngine;

namespace HSM
{
    public abstract class State
    {
        public string stateName = "Default State Name";
        protected Entity entity;

        protected State(Entity entity)
        {
            this.entity = entity;
        }
        // The base for a working Hierarchical State Machine (HSM).
        internal StateMachine subStateMachine;
        protected State subState => subStateMachine?.currentState;

        // This function is called only once, when you change the Entity's state to this one.
        public abstract void Enter();

        // This function is called every frame, as long as the Entity is set to this state, or any of its sub-states.
        public abstract void Do();

        // This function is called only once, when you change the Entity's state from this one to another.
        public abstract void Exit();

        // This function is what you call from the Entity's Update() function. It will call the Do() function of this state,
        // and all of its sub-states, recursively.    
        public void RecursiveDo()
        {
            Do();
            subState?.RecursiveDo();
        }
        public void RecursiveExit()
        {
            subState?.RecursiveExit();
            Exit();
        
            // Clear the cached sub-state so it fires Enter() on the next visit!
            if(subStateMachine != null)
            {
                subStateMachine.currentState = null;
            }
        }
        public string RecursiveStateString()
        {
            string fullStateName = stateName;
            if(subState != null)        {
                fullStateName += "." + subState.RecursiveStateString();
            }
            return fullStateName;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Statemachine
{
    public class StateMachine
    {
        public State currentState;
        // Dictionary allows us to find states by their Class Type
        public Dictionary<Type, State> availableStates = new Dictionary<Type, State>();
        public void AddStates(params State[] states)
        {
            foreach (var s in states)
            {
                availableStates.Add(s.GetType(), s);
            }
        }

        public Exception ChangeStateTo<T>() where T : State
        {
            var type = typeof(T);
            if (!availableStates.ContainsKey(type)) return new Exception("State of type " + type + " not found on this machine.");

            currentState?.Exit();
            currentState = availableStates[type];
            currentState.Enter();
            return null;
        }
    }
}
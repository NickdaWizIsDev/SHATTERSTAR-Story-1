using System;
using System.Collections.Generic;

namespace HSM
{
    public class StateMachine
    {
        public State currentState;

        public State previousState;
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
            if (!availableStates.TryGetValue(type, out _)) throw new Exception("State of type " 
                                                                    + type + " not found on this machine.");
            if (availableStates[type] == currentState) return null;
            currentState?.RecursiveExit();
            previousState = currentState;
            currentState = availableStates[type];
            currentState.Enter();
            return null;
        }
    }
}
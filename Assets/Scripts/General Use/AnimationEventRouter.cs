using UnityEngine;

namespace General_Use
{
    public class AnimationEventRouter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour script;
        
        public void CallFunction(string methodName)
        {
            script.Invoke(methodName, 0f);
        }
    }
}
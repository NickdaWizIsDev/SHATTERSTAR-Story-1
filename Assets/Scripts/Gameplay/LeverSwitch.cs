using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class LeverSwitch : MonoBehaviour, IStrikeable
    {
        public UnityEvent onToggle;
        private bool isToggled;

        public void OnStrike(AttackType type)
        {
            isToggled = !isToggled;
            onToggle?.Invoke();
        
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
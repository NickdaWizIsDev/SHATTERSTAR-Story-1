using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class LeverSwitch : MonoBehaviour, IStrikeable
    {
        public SaveStateID saveState;
        public UnityEvent onToggle;
        private bool isToggled;

        private void Start()
        {
            if (saveState == null || !GameManager.Instance.activeStates.Contains(saveState)) return;
            isToggled = true;
            
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void OnStrike(AttackType type)
        {
            isToggled = !isToggled;
            onToggle?.Invoke();
        
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            if (saveState is null) return;
            if (isToggled) GameManager.Instance.activeStates.Add(saveState);
            else GameManager.Instance.activeStates.Remove(saveState);
        }
    }
}
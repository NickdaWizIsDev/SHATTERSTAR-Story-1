using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class LeverSwitch : MonoBehaviour, IStrikeable
    {
        [SerializeField] private SaveStateID saveState;
        [SerializeField] private UnityEvent onToggle;
        [SerializeField] private bool oneShot = true;
        private bool isToggled;

        private void Start()
        {
            if (saveState == null || !GameManager.Instance.activeStates.Contains(saveState)) return;
            isToggled = true;
            
            onToggle?.Invoke();
        }

        public void OnStrike(AttackType type)
        {
            if(isToggled && oneShot) return;
            isToggled = !isToggled;
            onToggle?.Invoke();

            if (TryGetComponent(out Animator animator))
            {
                animator.SetBool("isToggled", isToggled);
            }

            if (saveState is null) return;
            if (isToggled) GameManager.Instance.activeStates.Add(saveState);
            else GameManager.Instance.activeStates.Remove(saveState);
        }
    }
}
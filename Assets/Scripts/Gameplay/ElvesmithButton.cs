using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class ElvesmithButton : MonoBehaviour, IStrikeable
    {
        public UnityEvent onPress;
        private bool isPressed;

        public void OnStrike(AttackType type)
        {
            if (isPressed) return;

            if (type == AttackType.Gauntlet)
            {
                isPressed = true;
                onPress?.Invoke();

                transform.localScale = new Vector3(1, 0.5f, 1); 
            }
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class AreaTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEnterArea;
        [SerializeField] private UnityEvent onExitArea;
        private void OnTriggerEnter2D(Collider2D other)
        {
            onEnterArea?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            onExitArea?.Invoke();
        }
    }
}
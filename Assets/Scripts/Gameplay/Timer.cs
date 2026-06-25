using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class Timer : MonoBehaviour
    {
        private float internalTimer;
        [SerializeField] private UnityEvent onFinish; 
        public void StartTimer(float time)
        {
            internalTimer = time;
        }

        private void Update()
        {
            if (internalTimer <= 0) return;
            internalTimer -= Time.deltaTime;
            if (internalTimer <= 0)
            {
                onFinish?.Invoke();
            }
        }
    }
}
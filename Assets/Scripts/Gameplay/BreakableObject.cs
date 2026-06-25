using System;
using UnityEngine;

namespace Gameplay
{
    public class BreakableObject : MonoBehaviour, IDestroyable, IStrikeable
    {
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private GameObject destroyEffectPrefab;
        [SerializeField] private int hitPoints = 1;
        private int currentHitPoints;

        private void Start()
        {
            currentHitPoints = hitPoints;
        }

        public void Hit()
        {
            currentHitPoints -= 1;
            if (currentHitPoints <= 0)
            {
                Destroy();
                return;
            }
            if (hitEffectPrefab) Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        public void Destroy()
        {
            if (destroyEffectPrefab)
            {
                Instantiate(destroyEffectPrefab, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }

        public void OnStrike(AttackType type)
        {
            Hit();
        }
    }
}
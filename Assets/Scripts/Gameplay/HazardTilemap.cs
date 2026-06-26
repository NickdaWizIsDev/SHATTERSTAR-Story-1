using System;
using Player;
using UnityEngine;

namespace Gameplay
{
    public class HazardTilemap : MonoBehaviour, IStrikeable
    {
        [Header("Hazard Settings")]
        [SerializeField] private int hazardDamage = 10;
        
        public void OnStrike(AttackType type)
        {
            // Optional Juice: If I ever add in a "clink" sort of SFX, I should trigger it here.
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.attachedRigidbody == null ||
                !other.collider.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            
            player.DamageThis(hazardDamage);
        }
    }
}
using System;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Gameplay
{
    public class HazardObject : MonoBehaviour, IStrikeable
    {
        [Header("Hazard Settings")]
        [SerializeField] private int hazardDamage = 10;

        [SerializeField] private UnityEvent onContact;
        
        public void OnStrike(AttackType type)
        {
            // Optional Juice: If I ever add in a "clink" sort of SFX, I should trigger it here.
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.attachedRigidbody == null ||
                !other.collider.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            
            onContact?.Invoke();
            player.DamageThis(hazardDamage, GetComponent<Collider2D>().ClosestPoint(player.transform.position));
        }
    }
}
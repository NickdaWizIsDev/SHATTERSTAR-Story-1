using System.Collections.Generic;
using UnityEngine;
using Player; 

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Hitbox Settings")]
        [SerializeField] private Transform attackPoint; 
        [SerializeField] private Vector2 attackArea = new Vector2(2f, 1.5f);
        [SerializeField] private LayerMask targetLayers; 
        [SerializeField] private int attackDamage = 10;

        private bool isHitboxActive;
        private AttackType currentAttackType;
        
        // Remembers who we hit so we don't apply damage 60 times a second
        private HashSet<Collider2D> alreadyHitTargets = new HashSet<Collider2D>();

        // 1. The State Machine calls this the moment the attack button is pressed
        public void InitializeAttack(AttackType type = AttackType.Sword)
        {
            currentAttackType = type;
            alreadyHitTargets.Clear(); // Wipe the memory for the new swing!
            isHitboxActive = false;
        }

        // 2. Called by an Animation Event on the exact frame the swoosh starts
        public void OpenHitbox()
        {
            isHitboxActive = true;
        }

        // 3. Called by an Animation Event on the frame the swoosh ends
        public void CloseHitbox()
        {
            isHitboxActive = false;
        }

        private void Update()
        {
            if (!isHitboxActive) return;

            // Continuously check for collisions during the Active Frames
            var hitTargets = Physics2D.OverlapBoxAll(attackPoint.position, attackArea, 0f, targetLayers);

            foreach (var target in hitTargets)
            {
                // If we already hit this exact enemy during this swing, skip them
                if (alreadyHitTargets.Contains(target)) continue;

                alreadyHitTargets.Add(target); // Add them to the memory

                // Apply Damage
                var damageable = target.GetComponent<IDamageable>();
                damageable?.DamageThis(attackDamage);

                // Apply Interaction
                var strikeable = target.GetComponent<IStrikeable>();
                strikeable?.OnStrike(currentAttackType);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            // The box will turn red in the editor when it is actually lethal!
            Gizmos.color = isHitboxActive ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(attackPoint.position, attackArea);
        }
    }
}
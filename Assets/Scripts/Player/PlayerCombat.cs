using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Hitbox Settings")]
        [SerializeField] private PlayerAttackPoint attackPoint; 
        private Vector2 AttackArea => attackPoint.Area;
        [SerializeField] private LayerMask targetLayers; 
        [SerializeField] private int attackDamage = 10;
        
        [Header("FX")]
        [SerializeField] private GameObject basicHitFXPrefab;
        [SerializeField] private AudioResource basicHitSfx;

        [Header("Pogo Settings")]
        [SerializeField] private Rigidbody2D playerRb; // Assign the Player's Rigidbody in the Inspector!
        [SerializeField] private float pogoBounceForce = 12f;

        private bool isHitboxActive;
        private bool isPogoActive; // Tracks if the current swing can pogo
        private AttackType currentAttackType;
        
        private HashSet<Collider2D> alreadyHitTargets = new HashSet<Collider2D>();

        // Updated to accept the pogo flag
        public void InitializeAttack(AttackType type = AttackType.Sword, bool isPogo = false)
        {
            currentAttackType = type;
            alreadyHitTargets.Clear(); 
            isHitboxActive = false;
            isPogoActive = isPogo; // Store the flag for this specific swing
        }

        public void OpenHitbox()
        {
            isHitboxActive = true;
        }

        public void CloseHitbox()
        {
            isHitboxActive = false;
        }

        private void Update()
        {
            if (!isHitboxActive) return;

            var hitTargets = Physics2D.OverlapBoxAll(attackPoint.transform.position, AttackArea, 0f, targetLayers);

            foreach (var target in hitTargets)
            {
                if (alreadyHitTargets.Contains(target)) continue;

                alreadyHitTargets.Add(target); 

                var damageable = target.attachedRigidbody?.GetComponent<IDamageable>();
                damageable?.DamageThis(attackDamage);

                var strikeable = target.GetComponent<IStrikeable>();
                strikeable?.OnStrike(currentAttackType);

                if (damageable == null && strikeable == null) continue;
                else
                {
                    // --- POGO LOGIC TRIGGER ---
                    // If the pogo flag is true and we successfully hit a valid target, BOUNCE!
                    if (isPogoActive && playerRb)
                    {
                        playerRb.linearVelocityY =  pogoBounceForce;
                    }

                    var hitPoint = target.ClosestPoint(attackPoint.transform.position);
                    Debug.Log($"The player hit {target.name} for {attackDamage} damage.");
                    
                    if (basicHitFXPrefab is not null)
                    {
                        var angle = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                        var fX = Instantiate(basicHitFXPrefab, hitPoint, angle);
                    }
                }
                    
                GameManager.Instance.TriggerHitStop(0.05f);
                CameraEffects.Instance?.Shake(0.1f, 0.2f);
            }
        }

        private void OnDrawGizmos()
        {
            if (attackPoint == null) return;
            Gizmos.color = isHitboxActive ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(attackPoint.transform.position, AttackArea);
        }
    }
}
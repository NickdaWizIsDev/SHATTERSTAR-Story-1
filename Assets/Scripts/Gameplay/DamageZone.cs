using UnityEngine;
using Player;
using Managers;
using PrimeTween;
using System.Collections;

namespace Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageZone : MonoBehaviour
    {
        [Header("Zone Settings")]
        [SerializeField] private Transform respawnPos;

        [SerializeField] private int hazardDamage = 15;

        public void OnZoneEnter()
        {
            var player = GameManager.Instance.Player;
            
            // Stop the player from moving/falling further while the screen fades
            player.DisableInput();
            
            // Optional: Zero out velocity so they don't keep plummeting off-screen
            player.movement.body.linearVelocity = Vector2.zero; 

            // Deal the damage immediately
            player.DamageThis(hazardDamage);

            // Start the reset sequence
            StartCoroutine(ResetSequence(player));
        }

        private IEnumerator ResetSequence(PlayerController player)
        {
            // 1. Fade to black and wait for it to finish
            yield return UIManager.Instance.FadeToBlack().ToYieldInstruction();

            // 2. Teleport the player
            if (respawnPos != null)
            {
                player.transform.position = respawnPos.position;
            }
            else
            {
                Debug.LogWarning("DamageZone: No respawn position assigned!");
            }

            // 3. Start the fade back in (no need to wait for it, so no yield)
            UIManager.Instance.FadeFromBlack(0.4f);

            // 4. Give control back to the Player
            player.EnableInput();
        }
        
        private void OnDrawGizmos()
        {
            if (respawnPos == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(respawnPos.position, 0.5f);
            Gizmos.DrawLine(transform.position, respawnPos.position);
        }
    }
}
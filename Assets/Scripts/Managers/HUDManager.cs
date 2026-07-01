using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using Player;

namespace Managers 
{
    public class HUDManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController player;
        [SerializeField] private Image mainFill;
        [SerializeField] private Image ghostFill;

        [Header("Tween Settings")]
        [SerializeField] private float ghostDelay = 0.3f;
        [SerializeField] private float ghostDrainSpeed = 0.4f;

        private void OnEnable()
        {
            if (player != null)
            {
                player.OnHealthPctChanged += UpdateHealthBar;
            }
        }

        private void OnDisable()
        {
            if (player != null)
            {
                player.OnHealthPctChanged -= UpdateHealthBar;
            }
        }

        private void Start()
        {
            if (player is null) return;
            UpdateHealthBar(player.CurrentHealthPct);
                
            ghostFill.fillAmount = player.CurrentHealthPct;
        }

        private void UpdateHealthBar(float targetPct)
        {
            // 1. Instantly snap the main red/green bar down to show the damage
            mainFill.fillAmount = targetPct;

            // 2. Stop any existing tweens on the ghost bar so they don't overlap during a rapid combo
            Tween.StopAll(ghostFill);

            // 3. Tween the ghost bar smoothly after a short delay for that juicy visual impact
            Tween.UIFillAmount(ghostFill, targetPct, duration: ghostDrainSpeed, startDelay: ghostDelay);
        }
    }
}
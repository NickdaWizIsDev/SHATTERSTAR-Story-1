using System;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using Player;

namespace Managers
{
    public class HUDManager : MonoBehaviour
    {
        [Header("References")]
        private PlayerController player;
        [SerializeField] private Image healthBarFill;  // The front bar 
        [SerializeField] private Image healthBarGhost; // The back bar 

        [Header("Tween Settings")]
        [SerializeField] private float ghostDrainDuration = 0.5f;
        [SerializeField] private float ghostDrainDelay = 0.3f;

        private void Start()
        {
            player = GameManager.Instance.Player;
        }

        private void OnEnable()
        {
            if (player != null)
            {
                player.OnHealthPctChanged += UpdateHealthBar;
            }
        }

        private void OnDisable()
        {
            // Always unsubscribe to prevent memory leaks!
            if (player != null)
            {
                player.OnHealthPctChanged -= UpdateHealthBar;
            }
        }

        private void UpdateHealthBar(float currentHealthPct)
        {
            // 1. Instantly snap the main health bar to the new value
            healthBarFill.fillAmount = currentHealthPct;

            // 2. Stop any existing tweens on the ghost bar so rapid hits don't conflict
            Tween.StopAll(healthBarGhost);

            // 3. Tween the ghost bar down to match the new health percentage smoothly
            Tween.UIFillAmount(healthBarGhost, currentHealthPct, duration: ghostDrainDuration, startDelay: ghostDrainDelay, ease: Ease.OutQuad);
        }
    }
}
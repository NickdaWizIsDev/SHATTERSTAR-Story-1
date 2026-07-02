using System;
using System.Collections;
using Helpers;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public CanvasGroup HUD;
        public Image blackOverlay;

        [SerializeField] private SceneReference mainMenuScene;
        
        [Header("Sequence Screens")]
        [Tooltip("Assign the CanvasGroup containing your 'You Died / Back to Menu' button.")]
        public CanvasGroup deathScreenCanvas;
        [Tooltip("Assign the CanvasGroup containing your 'Demo Complete' message and button.")]
        public CanvasGroup endDemoCanvas;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            FadeFromBlack(0.75f);
            endDemoCanvas.gameObject.SetActive(false);
            deathScreenCanvas.gameObject.SetActive(false);
        }

        public Tween FadeToBlack(float duration = 0.2f)
        {
            return Tween.Alpha(blackOverlay, endValue: 1f, duration, useUnscaledTime: true);
        }

        public Tween FadeFromBlack(float duration = 0.2f, float delay = 0.2f)
        {
            return Tween.Alpha(blackOverlay, endValue: 0f, duration, startDelay: delay, useUnscaledTime: true);
        }

        public void DeathSequence()
        {
            // Freeze the game (optional, assuming player script already disabled their own movement)
            GameManager.Instance.timeScale = 0; 
            
            // Fade to black, THEN show the death screen menu
            FadeToBlack(1f).OnComplete(() =>
            {
                if (deathScreenCanvas == null) return;
                deathScreenCanvas.gameObject.SetActive(true);
                Tween.Alpha(deathScreenCanvas, endValue: 1f, duration: 0.5f, useUnscaledTime: true);
                deathScreenCanvas.interactable = true;
                deathScreenCanvas.blocksRaycasts = true;
            });
        }

        public void ResetGame()
        {
            // Reset levers and arena states, send player back to the main menu. 
            GameManager.Instance.timeScale = 1; 
            SceneManager.LoadScene(mainMenuScene.ScenePath, LoadSceneMode.Single);
        }

        public void EndDemoSequence()
        {
            // Triggered after killing the Construct. Fades to black, then shows the "Thanks for Playing" screen.
            GameManager.Instance.Player.DisableInput();
            FadeToBlack(2f).OnComplete(() =>
            {
                if (endDemoCanvas == null) return;
                endDemoCanvas.gameObject.SetActive(true);
                Tween.Alpha(endDemoCanvas, endValue: 1f, duration: 1f, useUnscaledTime: true);
                endDemoCanvas.interactable = true;
                endDemoCanvas.blocksRaycasts = true;
            });
        }
    }
}
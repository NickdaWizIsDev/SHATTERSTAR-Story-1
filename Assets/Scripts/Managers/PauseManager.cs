using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        [Header("Pause References")]
        [SerializeField] private CanvasGroup mainPauseCanvas;
        [SerializeField] private Button resume;
        [SerializeField] private Button options;
        [SerializeField] private Button menu;

        [SerializeField] private CanvasGroup optionsMenu;
        [SerializeField] private Button backToMenu;

        private void Awake()
        {
            resume?.onClick.AddListener(ResumeGame);
            options?.onClick.AddListener(OpenOptionsMenu);
            menu?.onClick.AddListener(OpenMainMenu);
            
            backToMenu?.onClick.AddListener(CloseOptionsMenu);
        }

        private void OnDisable()
        {
            resume?.onClick.RemoveListener(ResumeGame);
            options?.onClick.RemoveListener(OpenOptionsMenu);
            menu?.onClick.RemoveListener(OpenMainMenu);
            
            backToMenu?.onClick.RemoveListener(CloseOptionsMenu);
        }

        private void Start()
        {
            mainPauseCanvas.alpha = 0;
            mainPauseCanvas.blocksRaycasts = false;
            optionsMenu.blocksRaycasts = false;
        }

        public void PauseGame()
        {
            GameManager.Instance.timeScale = 0;
            mainPauseCanvas.alpha = 1;
            mainPauseCanvas.blocksRaycasts = true;
            GameManager.Instance.Player.DisableInput();
        }

        public void ResumeGame()
        {
            GameManager.Instance.timeScale = 1;
            mainPauseCanvas.alpha = 0;
            mainPauseCanvas.blocksRaycasts = false;
            GameManager.Instance.Player.EnableInput();
        }

        public void OpenOptionsMenu()
        {
            mainPauseCanvas.blocksRaycasts = false;
            mainPauseCanvas.alpha = 0;
            optionsMenu.blocksRaycasts = true;
            optionsMenu.alpha = 1;
        }

        public void CloseOptionsMenu()
        {
            optionsMenu.blocksRaycasts = false;
            optionsMenu.alpha = 0;
            mainPauseCanvas.blocksRaycasts = true;
            mainPauseCanvas.alpha = 1;
        }
        
        public void OpenMainMenu()
        {
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
    
    }
}
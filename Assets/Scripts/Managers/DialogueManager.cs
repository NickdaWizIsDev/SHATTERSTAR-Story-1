using System.Collections;
using UnityEngine;
using TMPro;
using Player;
using UnityEngine.Events;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("UI References")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;

        [Header("Settings")]
        public float typingSpeed = 0.04f;

        public bool IsPlaying { get; private set; }

        private string[] currentLines;
        private int currentLineIndex;
        private Coroutine typingCoroutine;
        private PlayerController currentPlayer;
        private bool isTyping;
        [HideInInspector] public UnityEvent onFinish;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            if (dialoguePanel != null) dialoguePanel.SetActive(false);
        }

        public void StartDialogue(string[] lines, PlayerController player)
        {
            IsPlaying = true;
            currentLines = lines;
            currentLineIndex = 0;
            currentPlayer = player;

            currentPlayer.DisableInput();

            currentPlayer.OnInteractEvent += HandleInput;

            dialoguePanel.SetActive(true);
            StartTyping();
        }

        private void HandleInput()
        {
            if (!IsPlaying) return;

            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < currentLines.Length)
                {
                    StartTyping();
                }
                else
                {
                    EndDialogue();
                }
            }
        }

        private void StartTyping()
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeLineRoutine());
        }

        private IEnumerator TypeLineRoutine()
        {
            isTyping = true;
            dialogueText.text = "";
            
            foreach (var c in currentLines[currentLineIndex].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
            
            isTyping = false;
        }

        private void EndDialogue()
        {
            IsPlaying = false;
            dialoguePanel.SetActive(false);

            currentPlayer.OnInteractEvent -= HandleInput;
            currentPlayer.EnableInput();
            onFinish?.Invoke();
        }
    }
}
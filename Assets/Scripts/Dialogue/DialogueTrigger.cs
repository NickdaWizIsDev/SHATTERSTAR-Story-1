using Managers;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Dialogue Content")]
        [TextArea(3, 5)]
        public string[] dialogueLines;

        [SerializeField] private bool autoTrigger;
        [SerializeField] private UnityEvent onFinish;
        
        private PlayerController playerInZone;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody == null ||
                !other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            playerInZone = player;
            if(autoTrigger) TriggerDialogue();
            else playerInZone.OnInteractEvent += TriggerDialogue;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (playerInZone == null || other.attachedRigidbody == null ||
                other.attachedRigidbody.gameObject != playerInZone.gameObject) return;
            playerInZone.OnInteractEvent -= TriggerDialogue;
            playerInZone = null;
        }

        private void TriggerDialogue()
        {
            if (DialogueManager.Instance != null && !DialogueManager.Instance.IsPlaying && playerInZone != null)
            {
                DialogueManager.Instance.StartDialogue(dialogueLines, playerInZone);
                DialogueManager.Instance.onFinish?.AddListener(() => onFinish?.Invoke());
            }
        }
    }
}
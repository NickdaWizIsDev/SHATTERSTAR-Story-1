using System;
using System.Collections;
using Helpers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

namespace Gameplay
{
    public class CenteredDoor : RoomExit
    {
        public enum Direction { Right, Left };
        public Direction autoWalkDirection; // Renamed to match your SideExit logic

        // We cache the player here so the StartTransition method knows who to move
        private PlayerController playerInZone;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isTransitioning) return;

            if (other.attachedRigidbody == null ||
                !other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            playerInZone = player;
                
            playerInZone.OnInteractEvent += StartTransition;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // If the player leaves the trigger box, sever the connection so they don't teleport later
            if (playerInZone == null || other.attachedRigidbody == null ||
                other.attachedRigidbody.gameObject != playerInZone.gameObject) return;
            playerInZone.OnInteractEvent -= StartTransition;
            playerInZone = null;
        }

        private void StartTransition()
        {
            if (isTransitioning || playerInZone == null) return;
            
            // Unsubscribe immediately so mashing the button doesn't run the coroutine twice
            playerInZone.OnInteractEvent -= StartTransition;
            
            StartCoroutine(TransitionRoutine(playerInZone));
        }

        private IEnumerator TransitionRoutine(PlayerController player)
        {
            isTransitioning = true;

            player.DisableInput();

            yield return UIManager.Instance.FadeToBlack(0.75f);

            player.movement.movementVector = Vector2.zero;
            player.movement.Stop();

            GameManager.Instance.nextSpawnExit = targetExitID;

            var currentScene = gameObject.scene.name;

            var loadOp = SceneManager.LoadSceneAsync(targetScene.ScenePath, LoadSceneMode.Additive);
            while (!loadOp.isDone)
            {
                yield return null;
            }

            SceneManager.UnloadSceneAsync(currentScene);
        }

        private void Start()
        {
            if (GameManager.Instance != null && GameManager.Instance.nextSpawnExit == exitID)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        private IEnumerator SpawnRoutine()
        {
            isTransitioning = true;
            var player = GameManager.Instance.Player;

            player.transform.position = spawnPoint.position;

            // Make the player take a small step out of the doorway
            var walkDir = autoWalkDirection == Direction.Right ? 0.2f : -0.2f;
            player.movement.movementVector = new Vector2(walkDir, 0);

            StartCoroutine(UIManager.Instance.FadeFromBlack(1f));

            yield return new WaitForSeconds(0.15f);

            player.movement.movementVector = Vector2.zero;
            player.movement.Stop();

            player.EnableInput();
            
            isTransitioning = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
        }
    }
}
using System;
using System.Collections;
using Helpers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

namespace Gameplay
{
    public class RoomDoor : MonoBehaviour
    {
        [Header("Door Connection")]
        public string doorName;
        public SceneReference targetScene;
        public string targetDoorName;

        [Header("Spawn Settings")]
        public Transform spawnPoint;

        public enum Direction {Right, Left};
        public Direction autoWalkDirection;

        private static bool isTransitioning = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isTransitioning) return;

            if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out PlayerController player))
            {
                StartCoroutine(TransitionRoutine(player));
            }
        }

        private IEnumerator TransitionRoutine(PlayerController player)
        {
            isTransitioning = true;

            player.DisableInput();
            var walkDir = autoWalkDirection == Direction.Right ? 0.5f : -0.5f;
            player.movement.movementVector = new Vector2(-walkDir, 0);

            yield return UIManager.Instance.FadeToBlack(0.5f);

            player.movement.movementVector = Vector2.zero;
            player.movement.Stop();

            GameManager.Instance.nextSpawnDoor = targetDoorName;

            string currentScene = gameObject.scene.name;

            AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetScene.ScenePath, LoadSceneMode.Additive);
            while (!loadOp.isDone)
            {
                yield return null;
            }

            SceneManager.UnloadSceneAsync(currentScene);
        }

        private void Start()
        {
            Debug.Log($"Door {doorName} Loaded");
            if (GameManager.Instance != null && GameManager.Instance.nextSpawnDoor == doorName)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        private IEnumerator SpawnRoutine()
        {
            isTransitioning = true;
            PlayerController player = GameManager.Instance.Player;

            player.transform.position = spawnPoint.position;

            var walkDir = autoWalkDirection == Direction.Right ? 0.5f : -0.5f;
            player.movement.movementVector = new Vector2(walkDir, 0);

            yield return UIManager.Instance.FadeFromBlack(0.5f);

            player.movement.movementVector = Vector2.zero;
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
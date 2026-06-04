using System;
using System.Collections;
using Helpers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

namespace Gameplay
{
    public class RoomSideExit : RoomExit
    {

        public enum Direction {Right, Left};
        public Direction autoWalkDirection;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isTransitioning) return;

            if (other.attachedRigidbody is not null && other.attachedRigidbody.TryGetComponent(out PlayerController player))
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

            GameManager.Instance.nextSpawnExit = targetExitID;

            var currentScene = gameObject.scene.name;

            var loadOp = SceneManager.LoadSceneAsync(targetScene.ScenePath, LoadSceneMode.Additive);
            while (loadOp != null && !loadOp.isDone)
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
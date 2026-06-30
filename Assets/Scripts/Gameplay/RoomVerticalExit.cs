using System.Collections;
using Helpers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class RoomVerticalExit : RoomExit
    {
        public enum VDirection {Up, Down};
        public enum HDirection {Right, Left};
        public VDirection exitDirection;
        public HDirection autoWalkDirection;

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

            yield return UIManager.Instance.FadeToBlack(0.2f);

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

            if (exitDirection == VDirection.Up)
            {
                var dir = autoWalkDirection == HDirection.Left ? -1 : 1;
                player.movement.movementVector = new Vector2(dir, 0);
            }

            UIManager.Instance.FadeFromBlack(1f);
            
            yield return new WaitForSeconds(0.15f);

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
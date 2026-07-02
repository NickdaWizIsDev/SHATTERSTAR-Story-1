using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class RoomManager : MonoBehaviour
    {
        [Header("Arena Info")] 
        [SerializeField] private bool roomHasArena;
        [SerializeField] private Arena arena;

        private void Start()
        {
            // Check if the player has already beaten this arena
            if (arena.arenaClearedState != null && GameManager.Instance.activeStates.Contains(arena.arenaClearedState))
            {
                // Arena already cleared! Disable the trigger, walls, and enemies.
                DisableArenaCompletely();
                return;
            }

            // If not cleared, prepare the arena
            arena.remainingEnemies = arena.arenaEnemies.Count;
            
            // Ensure barriers are down and enemies are hidden/disabled at the start
            foreach (var wall in arena.barrierWalls) wall.SetActive(false);
            foreach (var enemy in arena.arenaEnemies) enemy.gameObject.SetActive(false);
        }

        // This should be called by a simple trigger collider at the arena's entrance(s)
        public void TriggerArena()
        {
            if (arena.IsArenaActive || arena.remainingEnemies <= 0) return;

            arena.IsArenaActive = true;
            arena.onArenaStart?.Invoke(); // Good for playing dramatic music or camera shakes!

            // 1. Lock the doors
            foreach (var wall in arena.barrierWalls) wall.SetActive(true);

            // 2. Spawn the enemies and subscribe to their death events
            foreach (var enemy in arena.arenaEnemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.OnDeath += HandleEnemyDeath; 
            }
        }

        private void HandleEnemyDeath(Entity deadEnemy)
        {
            // Unsubscribe to prevent memory leaks
            deadEnemy.OnDeath -= HandleEnemyDeath;
            
            arena.remainingEnemies--;

            if (arena.remainingEnemies <= 0)
            {
                StartCoroutine(EndArenaRoutine());
            }
        }

        private IEnumerator EndArenaRoutine()
        {
            // Wait a brief moment for the last enemy's death animation to finish
            yield return new WaitForSeconds(1f);

            arena.IsArenaActive = false;
            arena.onArenaComplete?.Invoke(); // Good for playing a victory chime or dropping a reward

            // Open the doors
            foreach (var wall in arena.barrierWalls) wall.SetActive(false);

            // Save the state globally 
            if (arena.arenaClearedState is not null)
            {
                GameManager.Instance.activeStates.Add(arena.arenaClearedState);
            }
        }

        private void DisableArenaCompletely()
        {
            foreach (var wall in arena.barrierWalls) wall.SetActive(false);
            foreach (var enemy in arena.arenaEnemies)
            {
                if (enemy != null) enemy.gameObject.SetActive(false);
            }
            
            // Disable the trigger box
            if (TryGetComponent(out Collider2D col)) col.enabled = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // This and other behaviors present in this script should go to its own ArenaManger eventually
            // Standard check to see if the player walked in
            if (other.attachedRigidbody == null ||
                !other.attachedRigidbody.TryGetComponent<PlayerController>(out var player)) return;
            TriggerArena();
            // Disable the collider so it doesn't trigger multiple times
            GetComponent<Collider2D>().enabled = false;
        }

        public void DisablePlayerMovement()
        {
            GameManager.Instance.Player.DisableInput();
        }
        public void EnablePlayerMovement()
        {
            GameManager.Instance.Player.EnableInput();
        }
    }

    [Serializable]
    public class Arena
    {
        [Header("Arena Actors")]
        [Tooltip("The physical walls or gates that block the exits.")]
        public GameObject[] barrierWalls;
        [Tooltip("The enemies that should spawn/activate during the lockdown.")]
        public List<Entity> arenaEnemies;

        [Header("Events")]
        public UnityEvent onArenaStart;
        public UnityEvent onArenaComplete;

        internal bool IsArenaActive = false;
        [SerializeField] internal int remainingEnemies;
        
        [Tooltip("The unique ID for this arena. If this is in the GameManager's active states, the arena won't trigger again.")]
        public SaveStateID arenaClearedState; 
    }
}
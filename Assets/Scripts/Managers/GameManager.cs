using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Player;
using UnityEngine;
using Managers;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Range(0, 3)] public float timeScale = 1f;
    
    public PlayerController Player { get; private set; }
    public RoomExitID nextSpawnExit;
    public HashSet<SaveStateID> activeStates = new HashSet<SaveStateID>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public void InitializePlayer(PlayerController playerController)
    {
        Player = playerController;
    }
    
    public void TriggerHitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        var originalTimeScale = timeScale;
        timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        timeScale = originalTimeScale;
    }
}
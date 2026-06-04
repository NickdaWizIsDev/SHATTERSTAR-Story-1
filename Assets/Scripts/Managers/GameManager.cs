using Player;
using UnityEngine;
using Managers;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Range(0, 3)] public float timeScale = 1f;
    
    public PlayerController Player { get; private set; }
    public string nextSpawnDoor;

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
}
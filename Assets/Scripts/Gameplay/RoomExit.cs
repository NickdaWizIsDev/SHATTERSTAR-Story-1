using Helpers;
using UnityEngine;

namespace Gameplay
{
    public class RoomExit : MonoBehaviour
    {
        [Header("Exit Connection")]
        public RoomExitID exitID;
        public SceneReference targetScene;
        public RoomExitID targetExitID;

        [Header("Spawn Settings")]
        public Transform spawnPoint;

        protected static bool isTransitioning = false;
    }
}
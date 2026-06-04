using UnityEngine;
using UnityEditor;

namespace SHATTERDEV.EditorTools
{
    public static class LevelBuilderHelper
    {
        // Change these paths to match exactly where your prefabs live in your project!
        [SerializeField] private const string DoorPrefabPath = "Assets/Resources/Prefabs/RoomDoor.prefab";
        [SerializeField] private const string VerticalExitPrefabPath = "Assets/Resources/Prefabs/VerticalExit.prefab";
        [SerializeField] private const string GeodeGruntPrefabPath = "Assets/Prefabs/Enemies/GeodeGrunt.prefab";

        // Adds a right-click option in the Hierarchy window
        [MenuItem("GameObject/Shatterstar/Spawn Room Door", false, 10)]
        public static void SpawnRoomDoor(MenuCommand menuCommand)
        {
            SpawnPrefab(DoorPrefabPath, "RoomDoor", menuCommand);
        }

        [MenuItem("GameObject/Shatterstar/Spawn Room Exit", false, 10)]
        public static void SpawnRoomExit(MenuCommand menuCommand)
        {
            SpawnPrefab(VerticalExitPrefabPath, "RoomExit", menuCommand);
        }

        [MenuItem("GameObject/Shatterstar/Spawn Geode Grunt", false, 10)]
        public static void SpawnGeodeGrunt(MenuCommand menuCommand)
        {
            SpawnPrefab(GeodeGruntPrefabPath, "GeodeGrunt", menuCommand);
        }

        // The core method that handles the actual spawning, undo history, and selection
        private static void SpawnPrefab(string path, string defaultName, MenuCommand menuCommand)
        {
            // Load the actual prefab from your project files
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogError($"[Level Builder] Could not find prefab at {path}. Please check the path string!");
                return;
            }

            // Instantiate it while maintaining the prefab link!
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = defaultName;

            // If you right-clicked a specific folder or object in the Hierarchy, parent it to that
            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            // Register the action so you can press Ctrl+Z to undo it
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);

            // Automatically select the new object in the hierarchy so you can immediately move it
            Selection.activeObject = instance;
        }
    }
}
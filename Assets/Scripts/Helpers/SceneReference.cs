using UnityEngine;

namespace Helpers
{
    [System.Serializable]
    public struct SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        // This field is only visible in the editor so we can drag-and-drop the asset
        [SerializeField] private Object sceneAsset;
    
        private bool IsValidSceneAsset()
        {
            if (sceneAsset == null) return false;
            return sceneAsset is UnityEditor.SceneAsset;
        }
#endif

        // This string is what actually gets saved and used at runtime
        [SerializeField] private string scenePath;

        // Use this property to pass directly into SceneManager.LoadScene()
        public string ScenePath => scenePath;

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (IsValidSceneAsset())
            {
                scenePath = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
            }
            else if (sceneAsset == null)
            {
                scenePath = string.Empty;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            // This ensures that if the scene moves or is renamed, the Editor reference updates gracefully
            var t = this;
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (!string.IsNullOrEmpty(t.scenePath) && t.sceneAsset == null)
                {
                    t.sceneAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(t.scenePath);
                }
            };
#endif
        }
    }
}
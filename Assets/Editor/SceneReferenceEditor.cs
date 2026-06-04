#if UNITY_EDITOR
using Helpers;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferenceEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty sceneAssetProp = property.FindPropertyRelative("sceneAsset");
        SerializedProperty scenePathProp = property.FindPropertyRelative("scenePath");

        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.BeginChangeCheck();
        
        // Force the object picker to only accept SceneAssets
        Object newScene = EditorGUI.ObjectField(position, label, sceneAssetProp.objectReferenceValue, typeof(SceneAsset), false);

        if (EditorGUI.EndChangeCheck())
        {
            sceneAssetProp.objectReferenceValue = newScene;
            if (newScene != null)
            {
                scenePathProp.stringValue = AssetDatabase.GetAssetPath(newScene);
            }
            else
            {
                scenePathProp.stringValue = string.Empty;
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif
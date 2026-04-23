#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PotionBehaviour : MonoBehaviour, IConsumable
{
    public void Interact()
    {
        
    }

    public void Consume()
    {
        Debug.Log("Me he curado! Hurra!");
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PotionBehaviour))]
public class PotionBehaviourEditor : Editor 
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Drink Potion!")) ((IConsumable)target).Consume();
    }
}
#endif
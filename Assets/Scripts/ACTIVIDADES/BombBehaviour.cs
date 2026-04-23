#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class BombBehaviour : MonoBehaviour, IConsumable
{
    public void Interact()
    {
    }

    public void Consume()
    {
        Debug.Log("BOOM! No sé qué pensaste que iba a pasar.");
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(BombBehaviour))]
public class BombBehaviourEditor : Editor 
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("It's, uh, a bomb. Consume?")) ((IConsumable)target).Consume();
    }
}
#endif
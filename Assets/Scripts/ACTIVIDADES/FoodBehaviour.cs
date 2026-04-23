#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class FoodBehaviour : MonoBehaviour, IConsumable
{
    public void Interact()
    {
        
    }

    public void Consume()
    {
        Debug.Log("Ya no tengo hambre!");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FoodBehaviour))]
public class FoodBehaviourEditor : Editor 
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("CONSUME THE FLESH OF THE LORD")) ((IConsumable)target).Consume();
    }
}
#endif
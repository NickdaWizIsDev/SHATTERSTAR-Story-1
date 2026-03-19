using UnityEngine;
using UnityEngine.InputSystem;

public class DebugButtons : MonoBehaviour
{
    [SerializeField] private InputActionReference debugAction;
    [SerializeField] private GameObject debugMenu;
    private string[] debugLines = {
        "Spawneando 4 enemigos",
        "Devolviendo la vida al personaje",
        "Spawneando armas",
        "Matando al personaje"};
    private bool isDebugOn;

    private void OnEnable() 
	{
		debugAction.action.Enable();
        debugAction.action.canceled += ToggleDebugMenu;
    }


    private void OnDisable() 
	{
		debugAction.action.Disable();
        debugAction.action.canceled -= ToggleDebugMenu;
    }

    private void ToggleDebugMenu(InputAction.CallbackContext context) 
    {
        isDebugOn = !isDebugOn;
        debugMenu.SetActive(isDebugOn);
    }

    public void DebugButton(int index)
    {
        Debug.Log(debugLines[index]);
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private float shootRange = 10f;

    void OnEnable()
    {
        mousePosition.action.Enable();
        shootAction.action.Enable();
    }

    void OnDisable()
    {
        mousePosition.action.Disable();
        shootAction.action.Disable();
    }

    void Update()
    {
        if(shootAction.action.triggered)
        {
            Vector2 mousePos = mousePosition.action.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 direction = worldPos - transform.position;
            
            Debug.DrawRay(transform.position, direction.normalized * shootRange, Color.red, 1f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, shootableLayer);
            if (hit.collider != null)
            {
                Debug.Log("Hit something shootable!");
                hit.collider.gameObject.SetActive(false);
                Debug.DrawLine(transform.position, hit.point, Color.green, 1f);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryBehaviour : MonoBehaviour
{
    private List<IConsumable> inventoryItems = new List<IConsumable>();
    [SerializeField] private InputActionReference consumeItemAction;

    void OnEnable()
    {        
        consumeItemAction.action.Enable();
        consumeItemAction.action.started += ConsumeItem;
    }
    void OnDisable()
    {
        consumeItemAction.action.Disable();
        consumeItemAction.action.started -= ConsumeItem;
    }

    private void ConsumeItem(InputAction.CallbackContext context)
    {
        if(inventoryItems.Count > 0)
        {
            IConsumable item = inventoryItems[0];
            item.Consume();
            inventoryItems.RemoveAt(0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<IConsumable>(out IConsumable item))
        {
            inventoryItems.Add(item);
            collision.gameObject.SetActive(false);
            Debug.Log("Item added to inventory: " + item);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    // Fields
    [Header("Movement")]
    [SerializeField]
    private InputActionReference m_moveAction;

    [SerializeField]
    private float m_movementSpeed = 4;

    [SerializeField]
    private float m_movementSmoothTime = 0.1f;

    private Rigidbody2D m_rigidbody;

    private Vector2 m_smoothedMovement;
    private Vector2 m_movementDampVelocity;

    // Methods
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_moveAction.action.Enable();
    }

    private void OnDisable()
    {
        m_moveAction.action.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 rawInput = m_moveAction.action.ReadValue<Vector2>();
        
        m_smoothedMovement = Vector2.SmoothDamp(m_smoothedMovement, rawInput, ref m_movementDampVelocity, m_movementSmoothTime);
        m_rigidbody.linearVelocity = m_smoothedMovement * m_movementSpeed;
    }
}

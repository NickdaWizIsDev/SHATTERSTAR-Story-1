using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float velocity;
    [Range(0,1)]public float stopLerpValue;
    public Vector2 movement;

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if(movement != Vector2.zero) body.AddForce(movement * velocity);
        else body.linearVelocity = Vector2.Lerp(body.linearVelocity, Vector2.zero, stopLerpValue);
    }
}
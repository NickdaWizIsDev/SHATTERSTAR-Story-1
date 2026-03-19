using UnityEngine;

namespace Player
{
    public class PlayerController : Entity
    {
        [SerializeField] PlayerMovement movement;

        void FixedUpdate()
        {
            // Calling movement functions here, and not on the movement script, so I can later make it depend on the player's state
            if(movement.movementVector != Vector2.zero)
            {
                movement.Move();
            }
            else movement.Stop();
        }

    }
}

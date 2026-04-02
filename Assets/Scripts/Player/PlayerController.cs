using UnityEngine;

namespace Player
{
    public class PlayerController : Entity
    {
        [SerializeField] PlayerMovement movement;
        PlayerStates playerStates;

        void Awake()
        {
            playerStates = new PlayerStates(this);
        }

        void Start()
        {
            stateMachine.ChangeState(playerStates.IdleState);
        }
        void FixedUpdate()
        {
            // Calling movement functions here, and not on the movement script, so I can later make it depend on the player's state
            if(movement.movementVector != Vector2.zero)
            {
                movement.Move();
            }
            else movement.Stop();
        }

        void Update()
        {
            CurrentState.RecursiveDo();

            // PENDING: show the current state (if it has a substate, add "." and the substate's name)
        }
    }
}

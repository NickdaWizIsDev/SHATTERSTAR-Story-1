using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {        
        [SerializeField] internal Vector2 movementVector; 
        [SerializeField] internal float runVelocity;
        [SerializeField][Range(0,1)] internal float stopLerpValue;
        [SerializeField] internal float jumpForce;

        [Header("References")]
        [SerializeField] Rigidbody2D body;
        public void OnMove(InputValue moveValue)
        {
            movementVector = moveValue.Get<Vector2>();
        }

        void Update()
        {
            // Acceleration Curve


        }

        internal void Move()
        {            
            var vel = Mathf.Abs(body.linearVelocityX);

            // Turning logic
            if(Mathf.Sign(body.linearVelocityX) != Mathf.Sign(movementVector.x) && vel > 0.5f)
            {
                body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, movementVector.x * runVelocity, stopLerpValue);
                // runState = state.turning or something of the sort
                return;      
            }

            // Acceleration, divided into two sections (acceleration is slower after the velocity is half way to the target)
            if(vel < runVelocity/2) body.AddForceX(movementVector.x * runVelocity);
            else
            {
                var force = runVelocity - vel;
                body.AddForceX(movementVector.x * force);
            }
        }
        internal void Stop()
        {
            body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, 0, stopLerpValue);
        }

        public void OnJump(InputValue jumpValue)
        {
            body.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }
}

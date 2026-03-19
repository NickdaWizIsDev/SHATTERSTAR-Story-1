using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField] internal Vector2 movementVector;

        [Header("Horizontal Movement")]
        [SerializeField] internal float runVelocity;
        [SerializeField][Range(0, 1)] internal float stopLerpValue;

        [Header("Vertical Movement")]
        [SerializeField] internal float jumpForce;
        [SerializeField] internal float coyoteTimeWindow;
        [SerializeField] internal float jumpBufferTimeWindow;
        private bool buffering;
        private float jumpLastPressedTime;
        private float lastGroundedTime;

        [Header("References")]
        [SerializeField] Rigidbody2D body;
        [SerializeField] TouchingDirections touching;
        public void OnMove(InputValue moveValue)
        {
            movementVector = moveValue.Get<Vector2>();
        }

        void Update()
        {
            if (touching.Ground)
            {
                if (buffering)
                {
                    if (Time.time - jumpLastPressedTime <= jumpBufferTimeWindow)
                    {
                        Jump();
                    }
                }
                if (lastGroundedTime != 0) lastGroundedTime = 0;
            }
            else if (!touching.Ground && lastGroundedTime == 0)
            {
                lastGroundedTime = Time.time;
            }
        }

        internal void Move()
        {
            var vel = Mathf.Abs(body.linearVelocityX);

            // Turning logic
            if (Mathf.Sign(body.linearVelocityX) != Mathf.Sign(movementVector.x) && vel > 0.5f)
            {
                body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, movementVector.x * runVelocity, stopLerpValue);
                // runState = state.turning or something of the sort
                return;
            }

            // Acceleration, divided into two sections (acceleration is slower after the velocity is half way to the target)
            if (vel < runVelocity / 2) body.AddForceX(movementVector.x * runVelocity);
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
            if (jumpValue.isPressed)
            {
                if (touching.Ground)
                {
                    Jump();
                }
                else
                {
                    if (Time.time - lastGroundedTime <= coyoteTimeWindow) Jump();
                    else jumpLastPressedTime = Time.time;
                }
            }
        }

        private void Jump()
        {            
            body.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }
}

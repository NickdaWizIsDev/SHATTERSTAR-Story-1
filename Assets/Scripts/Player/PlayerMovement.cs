using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField] internal Vector2 movementVector;
        [SerializeField] internal Vector2 CurrentVelocity => body.linearVelocity;

        [Header("Horizontal Movement")]
        [SerializeField] internal float runVelocity;
        [SerializeField][Range(0, 1)] private float stopLerpValue;
        [SerializeField] internal float dashSpeed = 45f;

        [Header("Vertical Movement")]
        [SerializeField] private float jumpHeight = 4;
        [SerializeField] private float coyoteTimeWindow = .05f;
        [SerializeField] private float coyoteTimer;
        [SerializeField] private float jumpBufferTimeWindow = .1f;
        [SerializeField] private float jumpBufferTimer;
        [SerializeField] private float baseGravityScale = 2f;
        [SerializeField] private float apexGravityScale = .75f;
        [SerializeField] internal float fallGravityScale = 4f;
        private bool _wantsToJump;

        [Header("References")]
        [SerializeField] internal Rigidbody2D body;
        [SerializeField] internal TouchingDirections touching;
        internal PlayerController controller;

        private void Update()
        {
            // Coyote Time
            if (touching.Ground && coyoteTimer != coyoteTimeWindow)
            {
                coyoteTimer = coyoteTimeWindow;
            }
            else if (!touching.Ground)
            {
                coyoteTimer -= Time.deltaTime;
            }

            // Jump Buffering
            if (jumpBufferTimer > 0)
            {
                jumpBufferTimer -= Time.deltaTime;
            }
            if (jumpBufferTimer > 0f && touching.Ground)
            {
                // Consume the timer so we don't double jump, then jump
                jumpBufferTimer = 0f;
                _wantsToJump = true;
            }

            if(controller.stateMachine.currentState is PlayerDashingState) return;

            // Gravity
            if (body.linearVelocityY < 0)
            {
                body.gravityScale = fallGravityScale;
            }
            else if (body.linearVelocityY <= 0.5f && body.linearVelocityY >= -0.5f && !touching.Ground)
            {
                body.gravityScale = apexGravityScale;
            }
            else
            {
                body.gravityScale = baseGravityScale;
            }

        }

        private void FixedUpdate()
        {
            if(_wantsToJump)
            {
                Jump();
                _wantsToJump = false;
            }
        }

        #region Movement
        public void OnMove(InputAction.CallbackContext context)
        {
            movementVector = context.ReadValue<Vector2>();
        }
        internal void Move()
        {
            float targetVelocityX = movementVector.x * runVelocity;

            // Turning logic
            if (Mathf.Sign(body.linearVelocityX) != Mathf.Sign(movementVector.x) && Mathf.Abs(body.linearVelocityX) > 0.5f)
            {
                body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, targetVelocityX, stopLerpValue / 4);
                return;
            }

            // Apply less force in the air, so you don't accelerate as fast as when you're on the ground
            float accelerationRate = touching.Ground ? (runVelocity * 15f) : (runVelocity * 7.5f);
            
            body.linearVelocityX = Mathf.MoveTowards(body.linearVelocityX, targetVelocityX, accelerationRate * Time.fixedDeltaTime);
        }
        internal void Stop()
        {
            // You lose less speed while in mid air
            body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, 0, touching.Ground? stopLerpValue : stopLerpValue/8);
        }
        #endregion

        #region Jumping
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (touching.Ground)
                {
                    Jump();
                }
                else
                {
                    
                    if (coyoteTimer > 0) Jump();
                    else { jumpBufferTimer = jumpBufferTimeWindow; }
                }
            }
            else if (context.canceled && body.linearVelocityY > 0)
            {
                // Short hop logic
                body.linearVelocityY *= .5f;
            }
        }

        private void Jump()
        {            
            // 1. Calculate the actual gravity affecting this Rigidbody2D
            // Physics2D.gravity is -9.81. We multiply by baseGravityScale and make it positive.
            float gravity = Mathf.Abs(Physics2D.gravity.y * baseGravityScale);

            // 2. Kinematic formula (yippie physics): vi = sqrt(vf^2 + 2gh)
            float requiredVelocity = Mathf.Sqrt(
                Mathf.Pow(1.5f, 2) + (2f * gravity * jumpHeight + 0.5f)
            );
            
            // Zero out vertical velocity before jumping so falling doesn't weaken the jump
            body.linearVelocityY = 0f;
            float requiredForce = requiredVelocity * body.mass;
            body.AddForceY(requiredForce, ForceMode2D.Impulse);
        }
        #endregion
    }
}

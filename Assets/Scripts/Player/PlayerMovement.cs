using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField] internal Vector2 movementVector;

        [Header("Horizontal Movement")]
        [SerializeField] internal float runVelocity;
        [SerializeField][Range(0, 1)] internal float stopLerpValue;

        [Header("Vertical Movement")]
        [SerializeField] internal float jumpHeight = 4;
        [SerializeField] internal float coyoteTimeWindow = .05f;
        [SerializeField] private float coyoteTimer;
        [SerializeField] internal float jumpBufferTimeWindow = .1f;
        [SerializeField] private float jumpBufferTimer;
        [SerializeField] internal float baseGravityScale = 2f;
        [SerializeField] internal float apexGravityScale = .75f;
        [SerializeField] internal float fallGravityScale = 4f;

        [Header("References")]
        [SerializeField] internal Rigidbody2D body;
        [SerializeField] internal TouchingDirections touching;
        internal PlayerInputActions inputActions;

        void Awake()
        {
            inputActions = new PlayerInputActions();

            inputActions.Gameplay.Jump.started += OnJump;
            inputActions.Gameplay.Jump.canceled += OnJump;

            inputActions.Gameplay.Move.performed += OnMove;
            inputActions.Gameplay.Move.canceled += OnMove;
        }

        private void OnEnable() => inputActions.Gameplay.Enable();
        private void OnDisable() => inputActions.Gameplay.Disable();

        void Update()
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
                Jump();
            }

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

        #region Movement
        public void OnMove(InputAction.CallbackContext context)
        {
            movementVector = context.ReadValue<Vector2>();
        }
        internal void Move()
        {
            var vel = Mathf.Abs(body.linearVelocityX);

            // Turning logic
            if (Mathf.Sign(body.linearVelocityX) != Mathf.Sign(movementVector.x) && vel > 0.5f)
            {
                body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, movementVector.x * runVelocity, stopLerpValue/4);
                return;
            }

            // Acceleration, divided into two sections (acceleration is slower after the velocity is half way to the target)
            // Halving runVelocity so that the acceleration is a bit more noticeable, otherwise you just feel the slowdown as you approach max speed
            if (vel < runVelocity / 2) body.AddForceX(movementVector.x * runVelocity / 2);
            else
            {
                var force = runVelocity - vel * (touching.Ground ? 1 : .65f); // Apply less force in the air, so you don't accelerate as fast as when you're on the ground
                body.AddForceX(movementVector.x * force);
            }
        }
        internal void Stop()
        {
            // You lose less speed while in mid air
            body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, 0, touching.Ground? stopLerpValue : stopLerpValue/3);
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

using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField] internal Vector2 movementVector;
        internal Vector2 CurrentVelocity => body.linearVelocity;

        [Header("Horizontal Movement")]
        [SerializeField] internal float walkVelocity;
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
        private bool wantsToJump;

        [Header("References")]
        [SerializeField] internal Rigidbody2D body;
        [SerializeField] internal TouchingDirections touching;
        internal PlayerController Controller;

        private void Update()
        {
            switch (touching.Ground)
            {
                // Coyote Time
                case true when !Mathf.Approximately(coyoteTimer, coyoteTimeWindow):
                    coyoteTimer = coyoteTimeWindow;
                    break;
                case false:
                    coyoteTimer -= Time.deltaTime;
                    break;
            }

            // Jump Buffer
            if (jumpBufferTimer > 0)
            {
                jumpBufferTimer -= Time.deltaTime;
            }
            if (jumpBufferTimer > 0f && touching.Ground)
            {
                // Consume the timer so we don't double jump, then jump
                jumpBufferTimer = 0f;
                wantsToJump = true;
            }

            if(Controller.stateMachine.currentState is PlayerDashingState) return;

            // Gravity
            if (body.linearVelocityY < 0)
            {
                body.gravityScale = fallGravityScale;
            }
            else if (body.linearVelocityY is <= 0.5f and >= -0.5f && !touching.Ground)
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
            if (!wantsToJump) return;
            Jump();
            wantsToJump = false;
        }

        #region Movement
        public void OnMove(InputAction.CallbackContext context)
        {
            if (DialogueManager.Instance.IsPlaying) return;
            movementVector = context.ReadValue<Vector2>();
        }
        internal void Move()
        {
            if(!Controller.CanMove) return;
            
            var activeVelocity = Controller.isRunning ? runVelocity : walkVelocity;
            var targetVelocityX = movementVector.x * activeVelocity;

            // Turning logic
            if (!Mathf.Approximately(Mathf.Sign(body.linearVelocityX), Mathf.Sign(movementVector.x)) 
                && Mathf.Abs(body.linearVelocityX) > 0.5f)
            {
                body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, targetVelocityX, stopLerpValue / 4);
                return;
            }

            // Apply less force in the air, so you don't accelerate as fast as when you're on the ground
            var accelerationRate = touching.Ground ? (activeVelocity * 15f) : (activeVelocity * 7.5f);
            
            body.linearVelocityX = Mathf.MoveTowards(body.linearVelocityX, targetVelocityX, accelerationRate * Time.fixedDeltaTime);
        }
        internal void Stop()
        {
            // You lose less speed while in midair
            body.linearVelocityX = Mathf.Lerp(body.linearVelocityX, 0, touching.Ground? stopLerpValue : stopLerpValue/8);
        }
        #endregion

        #region Jumping
        public void OnJump(InputAction.CallbackContext context)
        {
            if (DialogueManager.Instance.IsPlaying || !Controller.CanMove) return;
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

        public void Jump()
        {            
            // 1. Calculate the actual gravity affecting this Rigidbody2D
            // Physics2D.gravity is -9.81. We multiply by baseGravityScale and make it positive.
            var gravity = Mathf.Abs(Physics2D.gravity.y * baseGravityScale);

            // 2. Kinematic formula: vi = sqrt(vf^2 + 2gh)
            var requiredVelocity = Mathf.Sqrt(
                Mathf.Pow(1.5f, 2) + (2f * gravity * jumpHeight + 0.5f)
            );
            
            // Zero out vertical velocity before jumping so falling doesn't weaken the jump
            body.linearVelocityY = 0f;
            var requiredForce = requiredVelocity * body.mass;
            body.AddForceY(requiredForce, ForceMode2D.Impulse);
        }
        #endregion
    }
}
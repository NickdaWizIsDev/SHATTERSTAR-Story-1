using System;
using Gameplay;
using PrimeTween;
using UnityEngine;
using HSM;
using UnityEngine.Audio;

namespace Enemies
{
    public class ElvesmithController : Entity, IDamageable
    {
        private static readonly int MColor = Shader.PropertyToID("m_Color");
        private static readonly int EmissionColor = Shader.PropertyToID("_Emission_Color");

        [Header("Boss Stats")]
        [SerializeField] internal int health = 30;
        [SerializeField] internal float moveSpeed = 4f;
        [SerializeField] internal float dashAttackRange = 6f;
        [SerializeField] internal float dashRecoveryTime = 1.5f;
        [SerializeField] internal float slamAttackRange = 2.5f;
        [SerializeField] internal float slamRecoveryTime = 0.5f;
        [SerializeField] internal int attacksBeforeStomp = 3;
        [SerializeField] internal float stompRecoveryTime = 0.7f;
        [SerializeField] internal float knockbackTime = 0.2f;
        internal float knockbackTimer;
        
        [Header("Dash Attempts & Cooldown")]
        [SerializeField] internal int maxDashAttempts = 2; // How many dashes before a long rest
        [SerializeField] internal float dashRefreshCooldown = 4f; // How long it takes to get dashes back
        internal int currentDashAttempts = 2;
        internal float nextDashTime; // Tracks when the next dash is legally allowed

        [Header("References")]
        internal Transform playerTarget;
        [SerializeField] internal Shockwave shockwavePrefab;
        [SerializeField] internal Transform shockwaveSpawnPoint;
        [SerializeField] internal Rigidbody2D body;
        [SerializeField] internal ElvesmithAnimations animations;
        [SerializeField] internal LayerMask obstacleLayer;
        [SerializeField] internal AudioResource hitSound;
        
        [Header("Juice")]
        [SerializeField] internal SpriteRenderer[] spriteRenderers;
        [SerializeField] private float knockbackForce = 6f;
        [SerializeField] private Color hitFlashColor = Color.red; 
        [SerializeField] private float hitFlashDuration = 0.1f;
        [ColorUsage(true, true)][SerializeField] internal Color attackGlowColor = Color.white;
        [SerializeField] private GameObject deathFXPrefab;
        
        private Color originalColor;
        internal Transform playerTransform;
        private MaterialPropertyBlock mpb;
        private Tween flashTween;
        
        internal int attackCounter = 0;
        internal float nextAttackTime = 0f;

        protected void Awake()
        {
            spriteRenderers ??= GetComponentsInChildren<SpriteRenderer>();
            body ??= GetComponent<Rigidbody2D>();
            mpb = new MaterialPropertyBlock();
            InitializeStateMachine();
        }

        private void Start()
        {
            if (playerTarget == null)
            {
                var player = GameManager.Instance.Player;
                if (player != null) playerTarget = player.transform;
            }

            if (spriteRenderers is { Length: > 0 } && spriteRenderers[0] != null) 
            {
                originalColor = spriteRenderers[0].sharedMaterial.HasProperty(MColor) 
                    ? spriteRenderers[0].sharedMaterial.GetColor(MColor) 
                    : Color.white;
            }
        }

        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            
            stateMachine.AddStates(
                new ElvesmithIdleState(this),
                new ElvesmithChaseState(this),
                new ElvesmithSlamState(this),
                new ElvesmithStompState(this),
                new ElvesmithDashState(this)
            );
            
            stateMachine.ChangeStateTo<ElvesmithIdleState>();
        }

        private void Update()
        {
            if (knockbackTimer > 0) knockbackTimer -= Time.deltaTime;
            
            CurrentState?.RecursiveDo();
        }

        internal void TriggerTelegraphGlow(float duration = 0.5f)
        {
            Tween.Custom(this, Color.black, attackGlowColor, duration, onValueChange: (target, val) =>
            {
                foreach (var sr in target.spriteRenderers)
                {
                    if (sr is null) return;
                    sr.GetPropertyBlock(mpb);
                    mpb.SetColor(EmissionColor, val);
                    sr.SetPropertyBlock(mpb);
                }
            }).OnComplete(()=> ToneDownGlow());
        }

        private void ToneDownGlow(float duration = 0.7f)
        {
            Tween.Custom(this, mpb.GetColor(EmissionColor), Color.black, duration, onValueChange: (target, val) =>
            {
                foreach (var sr in target.spriteRenderers)
                {
                    if (sr is null) return;
                    sr.GetPropertyBlock(mpb);
                    mpb.SetColor(EmissionColor, val);
                    sr.SetPropertyBlock(mpb);
                }
            });
        }

        internal void FacePlayer()
        {
            if (playerTarget == null) return;
            var direction = Mathf.Sign(playerTarget.position.x - transform.position.x);
            transform.localScale = new Vector3(direction, 1, 1);
        }

        public void DamageThis(int damage, Vector2 damageSourcePos = default)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
                return;
            }
        
            var knockbackDir = Vector2.right;
            
            if (damageSourcePos != default)
            {
                knockbackDir = ((Vector2)transform.position - damageSourcePos).normalized;
            }
            else if (playerTarget is not null)
            {
                knockbackDir = ((Vector2)transform.position - (Vector2)playerTarget.position).normalized;
            }

            knockbackDir.y = 0.1f; 
            
            if (body is not null)
            {
                body.linearVelocity = Vector2.zero;
                body.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                knockbackTimer = knockbackTime;
            }

            if (spriteRenderers is { Length: > 0 } && gameObject.activeInHierarchy)
            {
                flashTween.Stop();

                foreach (var sr in spriteRenderers)
                {
                    if (!sr) continue;
                    sr.GetPropertyBlock(mpb);
                    mpb.SetColor(MColor, hitFlashColor);
                    sr.SetPropertyBlock(mpb);
                }

                flashTween = Tween.Custom(hitFlashColor, originalColor, duration: hitFlashDuration, onValueChange: color =>
                {
                    foreach (var sr in spriteRenderers)
                    {
                        if (sr is null) continue;
                        sr.GetPropertyBlock(mpb);
                        mpb.SetColor(MColor, color);
                        sr.SetPropertyBlock(mpb);
                    }
                });
            }

            if (hitSound is not null)
            {
                PlaySFX(hitSound);
            }
        }

        private void Die()
        {
            flashTween.Stop();
            Tween.StopAll(this);
            OnDeath?.Invoke(this);
            gameObject.SetActive(false);
            Instantiate(deathFXPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.EndDemo();
        }
    }

    [Serializable]
    internal class ElvesmithAnimations
    {
        public AnimationClip IdleAnimation;
        public AnimationClip MoveAnimation;
        public AnimationClip SlamAnimation;
        public AnimationClip StompAnimation;
        public AnimationClip DashAnimation;
    }
}
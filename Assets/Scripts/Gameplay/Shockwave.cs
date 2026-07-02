using UnityEngine;
using Player;
using PrimeTween;

namespace Gameplay
{
    public class Shockwave : MonoBehaviour
    {
        [SerializeField] private float speed = 8f;
        [SerializeField] private int damage = 20;
        [SerializeField] private float lifetime = 3f;

        public Vector2 MoveDirection { get; set; }

        private void Start()
        {
            Tween.ScaleY(transform, 0.1f, 1f, 0.2f);
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(MoveDirection * (speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out PlayerController player))
            {
                player.DamageThis(damage, transform.position);
            }
        }
    }
}
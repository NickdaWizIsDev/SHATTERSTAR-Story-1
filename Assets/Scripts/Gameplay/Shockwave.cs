using UnityEngine;
using Player;

namespace Gameplay
{
    public class Shockwave : MonoBehaviour
    {
        [SerializeField] private float speed = 8f;
        [SerializeField] private int damage = 20;
        [SerializeField] private float lifetime = 3f;

        private Vector2 moveDirection;

        private void Start()
        {
            moveDirection = transform.right; 
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
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
using UnityEngine;
using Player;

namespace Enemies
{
    public class EnemyContactDamage : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;

        private void Start()
        {
            if (!enemyController)
            {
                enemyController = GetComponentInParent<EnemyController>();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (enemyController == null) return;

            if (other.attachedRigidbody == null ||
                !other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            
            player.DamageThis(enemyController.attackDamage, transform.position);
        }
    }
}
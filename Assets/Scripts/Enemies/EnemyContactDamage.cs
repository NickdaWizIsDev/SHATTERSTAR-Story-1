using UnityEngine;
using Player;

namespace Enemies
{
    public class EnemyContactDamage : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private int damage = 10;

        private void Start()
        {
            if (!enemyController)
            {
                enemyController = GetComponentInParent<EnemyController>();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.attachedRigidbody == null ||
                !other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            
            player.DamageThis(enemyController? enemyController.attackDamage : damage, transform.position);
        }
    }
}
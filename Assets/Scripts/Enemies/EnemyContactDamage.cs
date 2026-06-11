using UnityEngine;
using Player;

namespace Enemies
{
    public class EnemyContactDamage : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.attachedRigidbody == null ||
                !other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
            var damageable = player.GetComponent<IDamageable>();
            damageable?.DamageThis(enemyController.attackDamage);
        }
    }
}
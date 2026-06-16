using UnityEngine;

namespace Player
{
    public class PlayerAttackPoint : MonoBehaviour
    {
        [SerializeField] internal Vector2 Area = new(2f, 1.5f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Area);
        }
    }
}
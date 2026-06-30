using UnityEngine;
using Player;

public class PlayerAnimationEventRouter : MonoBehaviour
{
    [SerializeField] private PlayerCombat combatEngine;

    // In the Animation Window, add an event and select these functions!
    public void OpenHitbox() => combatEngine.OpenHitbox();
    public void CloseHitbox() => combatEngine.CloseHitbox();
}
using HSM;
using Managers;

namespace Player
{
    public class PlayerDeathState : State
    {
        private PlayerController player;
        public PlayerDeathState(PlayerController _player) : base(_player)
        {
            player = _player;
            stateName = "Dead";
        }

        public override void Enter()
        {
            player.animationManager.PlayAnimation(player.animations.DeathAnimation);
            player.PlaySFX(player.hitAudio);
            player.DisableInput();
            player.CanMove = false;
            player.isDead = true;
            UIManager.Instance.DeathSequence();
        }

        public override void Do()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
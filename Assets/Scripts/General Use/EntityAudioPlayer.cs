using UnityEngine;
using UnityEngine.Audio;

namespace General_Use
{
    public class EntityAudioPlayer : MonoBehaviour
    {
        [SerializeField] private Entity target;

        public void PlayResource(AudioResource res)
        {
            target.PlaySFX(res);
        }
    }
}
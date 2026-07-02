using System;
using System.Collections.Generic;
using UnityEngine;
using HSM;
using UnityEngine.Audio;

public class Entity : MonoBehaviour
{
    public AnimationManager animationManager;
    internal StateMachine stateMachine;
    
    [Header("Audio Pool")]
    [Tooltip("Add 2 or 3 AudioSources to the GameObject and assign them here.")]
    [SerializeField] internal AudioSource[] sources; 
    
    protected State CurrentState => stateMachine.currentState;
    public Action<Entity> OnDeath { get; set; }
    protected internal bool isDead;

    private List<State> availableStates = new List<State>();

    public void PlaySFX(AudioResource res)
    {
        if (sources == null || sources.Length == 0) return;

        // Look for an available, silent audio source
        foreach (var s in sources)
        {
            if (!s.isPlaying)
            {
                s.resource = res;
                s.Play();
                return;
            }
        }

        // Failsafe: If all sources are busy (e.g., crazy multi-hit combo), 
        // just override the oldest one (index 0) so the hit registers.
        sources[0].resource = res;
        sources[0].Play();
    }
}
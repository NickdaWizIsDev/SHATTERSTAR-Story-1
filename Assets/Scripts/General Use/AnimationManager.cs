using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // This script is meant to be used as a component of an Entity, and will be used by the States to play animations.
    // This is mainly because I lowkey hate Unity's Animator Controller. With this, I can just call PlayAnimation("AnimationClip")
    // and after checking prerequesites, it will play the animation. Same with setting bools, floats and ints. Godsend.

    // You need to set the Animator component of the Entity in the inspector.
    // This is for two main reasons. The first being that once the scene has like, idk, 40~ entities, running GetComponent on each
    // of them at Awake or Start is gonna freeze the goddamn game. The second, is that some entities will have Animator as a
    // component on the same GameObject. Some will have it as a direct children. Some, like the player, will have it two children
    // down. Instead of calling GetComponentInChildren, which is even worse than the basic version, we just set them manually.
    [SerializeField] private Animator animator;

    // Cache the current animation to see if we're already playing it.
    private string currentAnimation;

    // Making this take a clip reference so I can be certain the clip exists in the animator
    public void PlayAnimation(AnimationClip animationClip, bool interruptSelf = false)
    {
        if (currentAnimation == animationClip.name && !interruptSelf) return;
        animator.Play(animationClip.name);
    }

    // Make sure the bool I wanna set in the animator is called the same as the one I'm passing. For example, idk,
    // CanAttack could be a bool for a given Entity. The bool in it's AnimatorController should also be called
    // CanAttack, caps sensitive. Same applies for all the other variable setters and getters.

    public void SetBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
    }
    public bool GetBool(string boolName)
    {
        return animator.GetBool(boolName);
    }
    public void SetFloat(string floatName, float value)
    {
        animator.SetFloat(floatName, value);
    }
    public float GetFloat(string floatName)
    {
        return animator.GetFloat(floatName);
    }
    public void SetInt(string intName, int value)
    {
        animator.SetInteger(intName, value);
    }
    public int GetInt(string intName)
    {
        return animator.GetInteger(intName);
    }
    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }
}
using UnityEngine;

public interface IDamageable
{
    void DamageThis(int damage, Vector2 damageSourcePos = default);
}
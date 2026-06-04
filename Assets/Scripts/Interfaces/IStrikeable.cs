public enum AttackType
{
    Sword,
    Gauntlet,
    Ascent
}

public interface IStrikeable
{
    public void OnStrike(AttackType type);
}
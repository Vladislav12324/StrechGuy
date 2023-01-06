public sealed class InfinityMaxRopeLengthBonus : Bonus
{
    public override float GetModifiedMaxRopeLength(float maxLength)
    {
        BonusSelectionView.bonus_i = 2;
        return float.PositiveInfinity;
    }
}
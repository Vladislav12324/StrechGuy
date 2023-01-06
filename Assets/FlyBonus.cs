public sealed class FlyBonus : Bonus
{
    public override int CountMinConnectedJoints => 0;
    public override float GetModifiedMaxRopeLength(float maxLength)
    {
        BonusSelectionView.bonus_i = 1;
        return maxLength * 1.5f;
    }
}
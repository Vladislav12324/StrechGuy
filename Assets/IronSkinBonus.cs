public sealed class IronSkinBonus : Bonus
{
    public override bool Breakable => false; 
    public override float GetModifiedMaxRopeLength(float maxLength)
    {
        BonusSelectionView.bonus_i = 3;
        return maxLength;
    }
}
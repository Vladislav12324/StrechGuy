using UnityEngine;

public sealed class MultipliedMaxRopeLengthBonus : Bonus
{
    [SerializeField] private float _modificator = 2;
    
    public override float GetModifiedMaxRopeLength(float maxLength)
    {
        BonusSelectionView.bonus_i = 0;
        return maxLength * _modificator;
    }
}
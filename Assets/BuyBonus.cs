using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuyBonus : MonoBehaviour
{
    [SerializeField] public Bonus _bonus;

    private Button _button;
    public string id;
    public event Action<Bonus> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => BonusSelectionView.buy_id=id);
        _button.onClick.AddListener(() => Clicked?.Invoke(_bonus));
    }
}
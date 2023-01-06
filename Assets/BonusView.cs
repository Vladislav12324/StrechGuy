using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BonusView : MonoBehaviour
{
    [SerializeField] private Bonus _bonus;

    private Button _button;

    public event Action<Bonus> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => Clicked?.Invoke(_bonus));
    }
}
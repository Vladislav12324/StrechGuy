using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BonusView : MonoBehaviour
{
    [SerializeField] public Bonus _bonus;

    private Button _button;
    public bool buy;
    public string buy_id;
    public event Action<Bonus> Clicked;

    private void Awake()
    {
        if (buy == false)
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                Clicked?.Invoke(_bonus);
            });
        }
    }
}
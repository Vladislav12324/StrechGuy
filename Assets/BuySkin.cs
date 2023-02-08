using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuySkin : MonoBehaviour
{

    private Button _button;
    public string id;
    public event Action<Bonus> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => SkinChangeView.buy_id = id);
    }
}
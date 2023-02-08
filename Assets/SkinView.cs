using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class SkinView : MonoBehaviour
{
    public event Action<HeadSkin, int> Clicked;
    public GameObject buy_button;
    [SerializeField] private Image _image;
    
    private HeadSkin _skin;
    private Action<SkinView, HeadSkin>  _onClicked;
    private int _id;
    private void Start()
    {
        if (gameObject.GetComponent<Button>().interactable == false)
        {
            buy_button.SetActive(true);
            buy_button.GetComponent<BuySkin>().id = (_id-475).ToString();
        }
        else
            buy_button.SetActive(false);
    }
    public void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            _onClicked?.Invoke(this, _skin);
            Clicked?.Invoke(_skin, _id);
        });
    }

    public void AddOnClickedAction(Action<SkinView, HeadSkin> onClicked) => _onClicked = onClicked;
    public void RemoveOnClickedAction() => _onClicked = null;

    public SkinView Instantiate(Transform parent, Sprite sprite, HeadSkin skin, int id, bool interactable)
    {
        var skinView = Instantiate(this, parent);
        skinView._skin = skin;
        skinView._image.sprite = sprite;
        skinView._id = id;
        skinView.GetComponent<Button>().interactable = interactable;
        return skinView;
    }
}
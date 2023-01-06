using UnityEngine;
using UnityEngine.UI;

public class OptionsView : MonoBehaviour
{
    [SerializeField] private Button _sound;
    [SerializeField] private Button _vibration;
    [SerializeField] private GameAudio _gameAudio;
    [SerializeField] private GameVibration _gameVibration;

    public void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);

    private void Start()
    {
        _gameAudio = FindObjectOfType<GameAudio>();
        _gameVibration = FindObjectOfType<GameVibration>();
        
        SyncViewStatus(_gameAudio, _sound.image);
        SyncViewStatus(_gameVibration, _vibration.image);
        
        _sound.onClick.AddListener(() => ChangeStatus(_gameAudio, _sound.image));
        _vibration.onClick.AddListener(() => ChangeStatus(_gameVibration, _vibration.image));
    }

    private void ChangeStatus(IActivate activate, Image image)
    {
        if (activate.Status)
        {
            image.color = Color.grey;
            activate.Deactivate();
        }
        else
        {
            image.color = Color.white;
            activate.Activate();
        }
    }

    private void SyncViewStatus(IActivate activate, Image image)
    {
        image.color = activate.Status ? Color.white : Color.grey;
    }
}
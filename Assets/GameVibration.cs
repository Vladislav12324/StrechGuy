using System.Runtime.InteropServices;
using UnityEngine;

public class GameVibration : MonoBehaviour, IVibration, IActivate
{
    [DllImport("__Internal")]
    private static extern void VibrateEx();
    
    public bool Status { get; private set; } = true;

    private void Start()
    {
        Status = PlayerData.VibrationStatus;
    }

    public void Vibrate()
    {
        #if UNITY_ANDROID || UNITY_IOS
        if(Status)
            Handheld.Vibrate();
        #endif
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        if(Status)
            VibrateEx();
        #endif
    }
    
    public void Activate()
    {
        Status = PlayerData.VibrationStatus = true;
    }

    public void Deactivate()
    {
        Status = PlayerData.VibrationStatus = false;
    }
}
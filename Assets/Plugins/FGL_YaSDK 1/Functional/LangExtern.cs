using UnityEngine;
using System.Runtime.InteropServices;

public class LangExtern : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern string GetLang();
    
}
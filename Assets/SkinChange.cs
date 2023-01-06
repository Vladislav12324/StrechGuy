using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinChange : MonoBehaviour
{
    public void Enable()
    {
        SceneManager.LoadScene("SkinChange", LoadSceneMode.Additive);
    }
}
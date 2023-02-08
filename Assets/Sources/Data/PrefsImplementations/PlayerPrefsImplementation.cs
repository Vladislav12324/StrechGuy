using UnityEngine;

namespace Sources.Data
{
    public class PlayerPrefsImplementation : IPrefsImplementation
    {
        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public void Increment(string key)
        {
            SetInt(key, GetInt(key) + 1);
        }
    }
}
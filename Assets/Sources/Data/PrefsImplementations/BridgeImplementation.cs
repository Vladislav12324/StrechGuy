using System.Diagnostics;
using InstantGamesBridge;

namespace Sources.Data
{
    public class BridgeImplementation : IPrefsImplementation
    {
        public void SetInt(string key, int value)
        {
            throw new System.NotImplementedException();
        }

        public int GetInt(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Increment(string key)
        {
            Bridge.storage.Get(key, (isSuccess, value) =>
            {
                if(isSuccess == false)
                {
                    LogWarning($"Recursively trying to obtain value with key {key}");
                    Increment(key);
                    return;
                }

                var incremented = int.Parse(value) + 1;
                Bridge.storage.Set(key, incremented, success =>
                {
                    if(success == false)
                    {
                        LogWarning($"Value not incremented");
                    }
                });
            });
        }
        
        [Conditional("LOGGING")]
        private static void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning($"[{nameof(Prefs)}] {message}");
        }
    }
}
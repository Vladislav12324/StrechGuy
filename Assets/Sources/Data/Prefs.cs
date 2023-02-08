namespace Sources.Data
{
    public static class Prefs
    {
        private static readonly IPrefsImplementation _implementation = 
            new PlayerPrefsImplementation();
        
        public static int GetInt(string key)
        {
            return _implementation.GetInt(key);
        }

        public static void SetInt(string key, int value)
        {
            _implementation.SetInt(key, value);
        }

        public static void Increment(string integerKey)
        {
            _implementation.Increment(integerKey);
        }
    }
}
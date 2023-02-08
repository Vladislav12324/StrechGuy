namespace Sources.Data
{
    public interface IPrefsImplementation
    {
        void SetInt(string key, int value);
        int GetInt(string key);
        void Increment(string key);
    }
}
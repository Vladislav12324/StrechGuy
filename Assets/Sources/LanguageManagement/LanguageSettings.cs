using UnityEngine;

namespace Sources.LanguageManagement
{
    [CreateAssetMenu(fileName = "LanguageSettings", menuName = "Settings/Language", order = 0)]
    public class LanguageSettings : ScriptableObject
    {
        [Header("Override")]
        [SerializeField] private bool _languageOverriden;
        [SerializeField] private string _language;

        public bool IsLanguageOverriden => _languageOverriden;
        public string OverrideLanguage => _language;
    }
}
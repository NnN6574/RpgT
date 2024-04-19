using UnityEngine;

namespace Mistave.Client
{
    public class DataProvider
    {
        public static void Save(string key, string json)
        {
            SaveToPrefs(key, json);
        }
        public static bool TryLoad(string key, out string value)
        {
            return TryLoadFromPrefs(key, out value);
        }
        private static void SaveToPrefs(string key, string json)
        {
            PlayerPrefs.SetString(key, json);
        }
        private static bool TryLoadFromPrefs(string key, out string value)
        {
            var result = PlayerPrefs.HasKey(key);
            value = string.Empty;
            if (result)
            {
                value = PlayerPrefs.GetString(key);
            }
            return result;
        }
    }
}
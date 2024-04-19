using System.Linq;
using Client.Common;
using UnityEngine;
using Zenject;

namespace Mistave.Client.Data
{
    public class DataContainerService : IInitializable, ITickable
    {
        [SerializeField] private UnitySerializedDictionary<string, IDataContainer> _containers = new();

        private const float AUTO_SAVE_DELAY = 5;

        private float _timer;
        public void Initialize()
        {
            DataContainer.OnRegistration += Registration;
            DataContainer.OnSave += SaveContainer;
            Debug.Log($"Initialize");
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer >= AUTO_SAVE_DELAY)
            {
                Save();
                Debug.Log($"Auto save complite");
            }
        }
        public void Save()
        {
            var keys = _containers.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                SaveContainer(keys.ElementAt(i));
            }
            _timer = 0;
        }
        public void Load()
        {
            var keys = _containers.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                LoadContainer(keys.ElementAt(i));
            }
        }

        private void SaveContainer(string key)
        {
            DataProvider.Save(key, _containers[key].ToJson());
            Debug.Log($"Container saved. Key: {key}");

        }
        private void LoadContainer(string key)
        {
            DataProvider.TryLoad(key, out var value);

            _containers[key].FromJson(value);
            Debug.Log($"Container loaded. Key: {key}");

        }

        private void Registration(string key, IDataContainer container)
        {
            _containers.Add(key, container);
            Debug.Log($"Container registration complite. Key: {key}");
            LoadContainer(key);
        }
    }
}
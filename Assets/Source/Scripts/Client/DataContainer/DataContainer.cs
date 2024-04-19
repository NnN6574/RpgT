using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Mistave.Client.Data
{
    [Serializable]
    public class DataContainer : IDataContainer, IInitializable
    {
        public string Key => GetType().ToString();

        public static UnityAction<string, IDataContainer> OnRegistration;
        public static UnityAction<string> OnSave;

        public void Initialize()
        {
            OnRegistration?.Invoke(Key, this);
        }

        public virtual void Save()
        {
            OnSave?.Invoke(Key);
        }
        public virtual string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
        public virtual void FromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
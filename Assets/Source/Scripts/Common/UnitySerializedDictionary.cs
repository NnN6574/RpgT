using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Client.Common
{
    [Serializable]
    public class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
                                                           ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private List<TKey> keyData = new List<TKey>();
        [SerializeField, HideInInspector] private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => Deserialize();
        void ISerializationCallbackReceiver.OnBeforeSerialize() => Serialize();

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) => Deserialize();

        [OnSerializing]
        internal void OnSerializing(StreamingContext context) => Serialize();

        public UnitySerializedDictionary()
        {
            
        }
        
        public UnitySerializedDictionary(Dictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            
        }
        
        private void Deserialize()
        {
            Clear();
            for (var i = 0; i < keyData.Count && i < valueData.Count; i++)
            {
                this[keyData[i]] = valueData[i];
            }
        }

        private void Serialize()
        {
            keyData.Clear();
            valueData.Clear();

            foreach (var item in this)
            {
                keyData.Add(item.Key);
                valueData.Add(item.Value);
            }
        }
    }
}
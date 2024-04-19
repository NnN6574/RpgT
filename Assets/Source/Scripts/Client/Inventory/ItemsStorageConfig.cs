using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "ItemsStorageConfig", menuName = "Data/Configs/ItemsStorageConfig")]
    public class ItemsStorageConfig : ScriptableObject
    {
        [SerializeField] private List<ItemsStorageConfigSlot> _items;
        public List<ItemsStorageConfigSlot> Items => _items; 
    }

    [Serializable]
    public class ItemsStorageConfigSlot
    {
        [SerializeField] private EquipmentItemConfig _itemConfig;
        [SerializeField] private int _count;

        public EquipmentItemConfig ItemConfig => _itemConfig;
        public int Count => _count;
    }
}
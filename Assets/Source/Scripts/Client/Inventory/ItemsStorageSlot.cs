using System;
using UnityEngine;

namespace Mistave.Client.Data.Inventory
{
    [Serializable]
    public class ItemsStorageSlot
    {
        [SerializeField] protected int _count;
        
        public int Count => _count;
        public void Add(int count)
        {
            _count += count;
        }
        public bool TryGet(int count, out int remainder)
        {
            if (_count >= count)
            {
                _count -= count;
                remainder = _count;
                return true;
            }
            else
            {
                remainder = _count;
                return false;
            }
        }
    }
    [Serializable]
    public class EquipmentStorageSlot : ItemsStorageSlot
    {
        [SerializeField] private EquipmentItem _item;
        public EquipmentItem Item => _item;

        public EquipmentStorageSlot(EquipmentItem item, int count)
        {
            _item = item;
            _count = count;
        }

    }
    [Serializable]
    public class ConsumableStorageSlot : ItemsStorageSlot
    {
        [SerializeField] private ConsumableItem _item;
        public ConsumableItem Item => _item;

        public ConsumableStorageSlot(ConsumableItem item, int count)
        {
            _item = item;
            _count = count;
        }

    }
    [Serializable]
    public class MaterialStorageSlot : ItemsStorageSlot
    {
        [SerializeField] private MaterialItem _item;
        public MaterialItem Item => _item;

        public MaterialStorageSlot(MaterialItem item, int count)
        {
            _item = item;
            _count = count;
        }

    }
    [Serializable]
    public class QuestStorageSlot : ItemsStorageSlot
    {
        [SerializeField] private QuestItem _item;
        public QuestItem Item => _item;
        public QuestStorageSlot(QuestItem item, int count)
        {
            _item = item;
            _count = count;
        }
    }

}

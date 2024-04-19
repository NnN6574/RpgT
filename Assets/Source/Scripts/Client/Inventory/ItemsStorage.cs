using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Mistave.Client.Data.Inventory
{
    public class ItemsStorage : DataContainer
    {
        [Inject] SpritesLibrary _spritesLibrary;
        [Inject] ItemsStorageConfig _itemsStorageConfig;
        [SerializeField] private List<EquipmentStorageSlot> _equipmentSlots = new();
        [SerializeField] private List<MaterialStorageSlot> _materialSlots = new();
        [SerializeField] private List<ConsumableStorageSlot> _consumableSlots = new();
        [SerializeField] private List<QuestStorageSlot> _questSlots = new();
        public List<EquipmentStorageSlot> EquipmentSlots => _equipmentSlots;
        public List<MaterialStorageSlot> MaterialSlots => _materialSlots;
        public List<ConsumableStorageSlot> ConsumableSlots => _consumableSlots;
        public List<QuestStorageSlot> QuestSlots => _questSlots;

        public void AddItem(Item item, int count = 1)
        {
            AddItemToSlot(item, count);
            Save();
        }
        public bool TryGet(Item item, int count, out int remainder)
        {
            switch (item.ItemType)
            {
                case ItemType.Equipment:
                    {
                        var result = TryGetEquipment(item, count, out remainder); Save();
                        return result;
                    }
                case ItemType.Material:
                    {
                        var result = TryGetMaterialI(item, count, out remainder); Save();
                        return result;
                    }
                case ItemType.Consumable:
                    {
                        var result = TryGetConsumable(item, count, out remainder); Save();
                        return result;
                    }
                case ItemType.Quest:
                    {
                        var result = TryGetQuest(item, count, out remainder); Save();
                        return result;
                    }
                default:
                    {
                        remainder = 0;
                        return false;
                    }
            }
        }

        private bool TryGetEquipment(Item item, int count, out int remainder)
        {
            var slotIndex = _equipmentSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                remainder = 0;
                return false;
            }
            else
            {
                var result = _equipmentSlots[slotIndex].TryGet(count, out remainder);
                if (remainder <= 0)
                {
                    _equipmentSlots.RemoveAt(slotIndex);
                }
                return result;
            }
        }
        private bool TryGetMaterialI(Item item, int count, out int remainder)
        {
            var slotIndex = _materialSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                remainder = 0;
                return false;
            }
            else
            {
                var result = _materialSlots[slotIndex].TryGet(count, out remainder);
                if (remainder <= 0)
                {
                    _materialSlots.RemoveAt(slotIndex);
                }
                return result;
            }
        }
        private bool TryGetConsumable(Item item, int count, out int remainder)
        {
            var slotIndex = _consumableSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                remainder = 0;
                return false;
            }
            else
            {
                var result = _consumableSlots[slotIndex].TryGet(count, out remainder);
                if (remainder <= 0)
                {
                    _consumableSlots.RemoveAt(slotIndex);
                }
                return result;
            }
        }
        private bool TryGetQuest(Item item, int count, out int remainder)
        {
            var slotIndex = _questSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                remainder = 0;
                return false;
            }
            else
            {
                var result = _questSlots[slotIndex].TryGet(count, out remainder);
                if (remainder <= 0)
                {
                    _questSlots.RemoveAt(slotIndex);
                }
                return result;
            }
        }
     
        private void AddEquipmentItem(EquipmentItem item, int count = 1)
        {
            var slotIndex = _equipmentSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                _equipmentSlots.Add(new EquipmentStorageSlot(item, count));
            }
            else
            {
                _equipmentSlots[slotIndex].Add(count);
            }
        }
        private void AddMaterialItem(MaterialItem item, int count = 1)
        {
            var slotIndex = _materialSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                _materialSlots.Add(new MaterialStorageSlot(item, count));;
            }
            else
            {
                _materialSlots[slotIndex].Add(count);
            }
        }
        private void AddConsumableItem(ConsumableItem item, int count = 1)
        {
            var slotIndex = _consumableSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                _consumableSlots.Add(new ConsumableStorageSlot(item, count));
            }
            else
            {
                _consumableSlots[slotIndex].Add(count);
            }
        }
        private void AddQuestItem(QuestItem item, int count = 1)
        {
            var slotIndex = _questSlots.FindIndex(e => e.Item.Id == item.Id);
            if (slotIndex < 0)
            {
                _questSlots.Add(new QuestStorageSlot(item, count));
            }
            else
            {
                _questSlots[slotIndex].Add(count);
            }
        }
        public void AddItem(List<ItemsStorageSlot> storageSlots, Item item, int count = 1)
        {

        }
      
        private void AddItemToSlot(Item item, int count = 1)
        {
            switch (item.ItemType)
            {
                case ItemType.Equipment:
                    AddEquipmentItem((EquipmentItem)item, count);
                    break;
                case ItemType.Material:
                    AddMaterialItem((MaterialItem)item, count);
                    break;
                case ItemType.Consumable:
                    AddConsumableItem((ConsumableItem)item, count);
                    break;
                case ItemType.Quest:
                    AddQuestItem((QuestItem)item, count);
                    break;
            }
        }
        public override void FromJson(string json)
        {
            if (json == string.Empty)
            {
                for (int i = 0; i < _itemsStorageConfig.Items.Count; i++)
                {
                    var slotConfig = _itemsStorageConfig.Items[i];
                    AddItemToSlot(slotConfig.ItemConfig.Item, slotConfig.Count);
                }
            }
            else
            {
                base.FromJson(json);
                for (int i = 0; i < _equipmentSlots.Count; i++)
                {
                    var item = _equipmentSlots[i].Item;
                    item.SetIcon(_spritesLibrary.GetSprite(item.ItemBaseType));
                }
                for (int i = 0; i < _materialSlots.Count; i++)
                {
                    var item = _materialSlots[i].Item;
                    item.SetIcon(_spritesLibrary.GetSprite(item.ItemBaseType));
                }
                for (int i = 0; i < _consumableSlots.Count; i++)
                {
                    var item = _consumableSlots[i].Item;
                    item.SetIcon(_spritesLibrary.GetSprite(item.ItemBaseType));
                }
                for (int i = 0; i < _questSlots.Count; i++)
                {
                    var item = _questSlots[i].Item;
                    item.SetIcon(_spritesLibrary.GetSprite(item.ItemBaseType));
                }
            }
        }
    }
}
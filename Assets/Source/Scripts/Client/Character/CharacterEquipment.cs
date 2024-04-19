using System;
using System.Collections.Generic;
using Mistave.Client.Data;
using Mistave.Client.Data.Inventory;
using UnityEngine;
using Zenject;

namespace Mistave.Client.Character.Data
{
    [Serializable]
    public class CharacterEquipment : DataContainer
    {
        [Inject] SpritesLibrary _spritesLibrary;
        [Inject] CharacterEquipmentConfig _equipmentConfig;
        [SerializeField] List<CharacterEquipmentSlot> _equipmentSlots = new();

        public List<CharacterEquipmentSlot> EquipmentSlots => _equipmentSlots;

        public bool TryEquip(EquipmentItem equipmentItem)
        {
            var emptySlotIndex = _equipmentSlots.FindIndex(e => e.EquipmentType == equipmentItem.EquipmentType && e.IsEmpty);
            if (emptySlotIndex < 0)
            {
                return false;
            }
            _equipmentSlots[emptySlotIndex].SetItem(equipmentItem);
            Save();
            return true;
        }
        public bool TryUnequip(EquipmentItem equipmentItem)
        {
            var slot = GetSlot(equipmentItem);
            if (slot == null) return false;
            Save();

            return slot.TryRemoveItem();
        }
        public CharacterEquipmentSlot GetSlot(EquipmentItem equipmentItem)
        {
            var slotIndex = _equipmentSlots.FindIndex(e => e.Item == equipmentItem);
            if (slotIndex < 0)
            {
                return null;
            }
            return _equipmentSlots[slotIndex];
        }
        public bool Swap(EquipmentItem equipmentItem, EquipmentItem newItem)
        {
            var slot = GetSlot(equipmentItem);
            if (slot == null) return false;
            slot.SetItem(newItem);
            Save();
            return true;
        }

        public override void FromJson(string json)
        {
            if (json == string.Empty)
            {
                for (int i = 0; i < _equipmentConfig.CharacterEquipmentSlots.Count; i++)
                {
                    var slotConfig = _equipmentConfig.CharacterEquipmentSlots[i];
                    if (slotConfig.ItemConfig != null)
                    {
                        _equipmentSlots.Add(new(slotConfig.EquipmentType, slotConfig.ItemConfig.Item));
                    }
                    else
                    {
                        _equipmentSlots.Add(new(slotConfig.EquipmentType));
                    }
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
            }
        }
    }

    [Serializable]
    public class CharacterEquipmentSlot
    {
        [SerializeField] private bool _isEmpty;
        [SerializeField] private EquipmentType _equipmentType;
        [SerializeField] private EquipmentItem _item;
        public bool IsEmpty => _isEmpty;
        public EquipmentType EquipmentType => _equipmentType;
        public EquipmentItem Item => _item;
        
        public CharacterEquipmentSlot(EquipmentType equipmentType)
        {
            _equipmentType = equipmentType;
            _isEmpty = true;
        }
        public CharacterEquipmentSlot(EquipmentType equipmentType, EquipmentItem item)
        {
            _equipmentType = equipmentType;
            _item = item;
            _isEmpty = false;
        }
        public void SetItem(EquipmentItem item)
        {
            _item = item;
            _isEmpty = false;
        }
        public bool TryRemoveItem()
        {
            if (_item == null)
            {
                return false;
            }
            _isEmpty = true;
            _item = null;
            return true;
        }
    }
}

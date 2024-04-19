using System;
using System.Collections.Generic;
using Mistave.Client.Character.Data;
using Mistave.Client.Data.Inventory;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "CharacterEquipmentConfig", menuName = "Data/Configs/CharacterEquipmentConfig")]
    public class CharacterEquipmentConfig : DataContainerConfig
    {
        [SerializeField] private List<EquipmentSlotElement> _characterEquipmentSlots;
        public List<EquipmentSlotElement> CharacterEquipmentSlots => _characterEquipmentSlots;
    }
    [Serializable]
    public class EquipmentSlotElement
    {
        [SerializeField] private EquipmentType _equipmentType;
        [SerializeField] private EquipmentItemConfig _itemConfig;

        public EquipmentType EquipmentType => _equipmentType;
        public EquipmentItemConfig ItemConfig => _itemConfig;
    }
}
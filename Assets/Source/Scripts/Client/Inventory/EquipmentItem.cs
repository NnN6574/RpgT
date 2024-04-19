using System;
using System.Collections.Generic;
using Mistave.Client.Character.Data;
using Mistave.Client.Data.Entity;
using UnityEngine;

namespace Mistave.Client.Data.Inventory
{
    public enum EquipmentType
    {
        Weapon,
        OffHand,
        Helmet,
        Armor,
        Pants,
        Gloves,
        Boots,
        Ring,
        Earring,
        Necklace,
    }
    [Serializable]
    public class EquipmentItem : Item
    {
        [SerializeField] EquipmentType _equipmentType;
        [SerializeField] protected AttributesRequirement _requirement;
        [SerializeField] List<EntityPropertyModifier> _mainProperty;
        [SerializeField] List<EntityPropertyModifier> _additionalProperty;

        public EquipmentType EquipmentType => _equipmentType;
        public AttributesRequirement Requirement => _requirement;
        public List<EntityPropertyModifier> MainProperty => _mainProperty;
        public List<EntityPropertyModifier> AdditionalProperty => _additionalProperty;

        public EquipmentItem(Item item, Sprite icon, EquipmentType equipmentType, AttributesRequirement requirement, List<EntityPropertyModifier> mainProperty, List<EntityPropertyModifier> additionalProperty) : base(item, icon)
        {
            _equipmentType = equipmentType;
            _requirement = requirement;
            _mainProperty = mainProperty;
            _additionalProperty = additionalProperty;
        }
    }
}
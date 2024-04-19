using System;
using UnityEngine;

namespace Mistave.Client.Data.Entity
{
    public enum PropertyType
    {
        Strength,
        PhysicalAttack,
        PhysicalProtection,
        Health,
        HealthRegeneration,
        BlockChance,
        Block,
        Willpower,
        PhysicalCriticalAttack,
        Dexterity,
        SpeedAttack,
        Evasion,
        PhysicalCriticalAttackChance,
        PhysicalAccuracy,
        SpeedMovement,
        Lucky,
        Perception,
        Parrying,
        Intelligence,
        MagicalAttack,
        MagicalProtection,
        MagicalCriticalAttack,
        MagicalCriticalAttackChance,
        Mana,
        Casting,
        MagicalAccuracy,
        ManaRegeneration,
        Resists,
        ResistFire,
        ResistWater,
        ResistAir,
        ResistEath,
        ResistHoly,
        ResistDark,
        ResistPoison
    }
    public enum PropertyValueType
    {
        Value,
        Percent
    }

    [Serializable]
    public class EntityProperty
    {
        [SerializeField] protected PropertyType _propertyType;
        [SerializeField] protected PropertyValueType _valueType;
        [SerializeField] protected float _value;

        public PropertyType PropertyType => _propertyType;
        public PropertyValueType ValueType => _valueType;
        public float Value => _value;

        public EntityProperty(PropertyType propertyType, PropertyValueType valueType, float value)
        {
            _propertyType = propertyType;
            _valueType = valueType;
            _value = value;
        }

        public void SetValue(float value)
        {
            _value = value;
        }
    }
}
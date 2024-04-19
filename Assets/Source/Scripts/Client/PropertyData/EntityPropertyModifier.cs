using System;
using UnityEngine;

namespace Mistave.Client.Data.Entity
{
    public enum PropertyModifierType
    {
        Add,
        Increase,
        More,
        Remove,
        Degrease,
        Less
    }

    [Serializable]
    public class EntityPropertyModifier : EntityProperty
    {
        [SerializeField] private PropertyModifierType _modifierType;

        public EntityPropertyModifier(PropertyType propertyType, PropertyModifierType modifierType, PropertyValueType valueType, float value) : base(propertyType, valueType, value)
        {
            _propertyType = propertyType;
            _modifierType = modifierType;
            _valueType = valueType;
            _value = value;
        }

        public PropertyModifierType ModifierType => _modifierType;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Mistave.Client.Data;
using Mistave.Client.Data.Entity;

namespace Mistave.Client.Character.Data
{
    [Serializable]
    public class CharacterProperties : DataContainer
    {
        [Inject] private CharacterPropertiesConfig _propertiesConfig;

        [SerializeField] private List<EntityProperty> _properties;

        public override void FromJson(string json)
        {
            if (json == string.Empty)
            {
                _properties = _propertiesConfig.Properties;
            }
            else
            {
                base.FromJson(json);
            }
        }
        public EntityProperty this[PropertyType propertyType]
        {
            get => _properties.Find(e => e.PropertyType == propertyType);
            set
            {
                var property = _properties.Find(e => e.PropertyType == propertyType);
                property = value;
            }
        }

        public void AddProperty(EntityProperty newProperty)
        {
            var index = _properties.FindIndex(e => e.PropertyType == newProperty.PropertyType);
            if (index < 0)
            {
                _properties.Add(newProperty);
            }
            else
            {
                var property = _properties[index];
                SetAtributeValue(property, property.Value + newProperty.Value);
            }
        }
        public bool TryRemoveProperty(EntityProperty entityProperty)
        {
            var index = _properties.FindIndex(e => e.PropertyType == entityProperty.PropertyType);
            if (index < 0)
            {
                return false;
            }

            var property = _properties[index];
            if (property.Value >= entityProperty.Value)
            {
                SetAtributeValue(property, property.Value - entityProperty.Value);
                return true;
            }
            return false;

        }

        private void SetAtributeValue(EntityProperty property, float value)
        {
            if (value > 0)
            {
                property.SetValue(value);
            }
            else
            {
                _properties.Remove(property);
            }
            
            Save();
        }
    }
}
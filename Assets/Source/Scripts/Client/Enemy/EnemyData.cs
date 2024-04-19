using System;
using System.Collections.Generic;
using Mistave.Client.Data;
using Mistave.Client.Data.Entity;
using UnityEngine;

namespace Mistave.Client.Enemy
{
    public enum EnemyType
    {

    }
    [Serializable]
    public class EnemyData
    {
        [SerializeField] private string _title;
        [SerializeField] private Rank _rank;
        [SerializeField] private EnemyType _type;
        [SerializeField] private Sprite _icon;
        [SerializeField] private List<EntityAttribute> _attributes;
        [SerializeField] private PropertyConfig _propertyConfig;
        private List<EntityProperty> _properties;

        public string Title => _title;
        public Rank Rank => _rank;
        public EnemyType EnemyType => _type;
        public Sprite Icon => _icon;
        public List<EntityAttribute> Attributes => _attributes;
        public List<EntityProperty> Properties => _properties;

        public void UpdatePropertyes()
        {
            for (int i = 0; i < _propertyConfig.ConfigElements.Count; i++)
            {
                var configElement = _propertyConfig.ConfigElements[i];
                var attributeValue = _attributes.Find(e => e.Type == configElement.AtributeType).Value;
                var property = _properties.Find(e => e.PropertyType == configElement.PropertyType);
                property.SetValue(attributeValue * configElement.Multiplayer);
            }
        }
    }
}
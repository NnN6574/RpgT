using System;
using System.Collections.Generic;
using Mistave.Client.Data.Entity;
using UnityEngine;
namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "PropertyConfig", menuName = "Data/Configs/PropertyConfig")]
    public class PropertyConfig : ScriptableObject
    {
        [SerializeField] private List<PropertyConfigElement> _propertyConfigElements;

        public List<PropertyConfigElement> ConfigElements => _propertyConfigElements;
    }

    [Serializable]
    public class PropertyConfigElement
    {
        [SerializeField] private PropertyType _propertyType;
        [SerializeField] private EntityAtributeType  _atributeType;
        [SerializeField] private float _multiplayer;

        public PropertyType PropertyType => _propertyType;
        public EntityAtributeType AtributeType => _atributeType;
        public float Multiplayer => _multiplayer;
    }
}
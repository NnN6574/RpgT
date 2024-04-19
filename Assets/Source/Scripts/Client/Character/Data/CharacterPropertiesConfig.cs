using Mistave.Client.Data.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "CharacterPropertiesConfig", menuName = "Data/Configs/CharacterPropertiesConfig")]
    public class CharacterPropertiesConfig : DataContainerConfig
    {
        [SerializeField] private List<EntityProperty> _properties;
        public List<EntityProperty> Properties => _properties;
    }
}
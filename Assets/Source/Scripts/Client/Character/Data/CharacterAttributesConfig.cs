using Mistave.Client.Data.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "CharacterAtributesConfig", menuName = "Data/Configs/CharacterAtributesConfig")]
    public class CharacterAttributesConfig : DataContainerConfig
    {
        [SerializeField] private List<EntityAttribute> _atributes;
        public List<EntityAttribute> Atributes => _atributes;
    }
}
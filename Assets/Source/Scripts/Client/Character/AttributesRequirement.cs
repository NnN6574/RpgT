using System;
using System.Collections.Generic;
using Mistave.Client.Data.Entity;
using UnityEngine;
namespace Mistave.Client.Character.Data
{
    [Serializable]
    public class AttributesRequirement
    {
        [SerializeField] private List<AttributeRequirement> _attributesRequirement;
        public List<AttributeRequirement> Requirements => _attributesRequirement;

        public AttributeRequirement this[EntityAtributeType characterAtributeType]
        {
            get => _attributesRequirement.Find(e => e.Type == characterAtributeType);
        }

        public static bool operator >=(AttributesRequirement a, CharacterAttributes b)
        {
            var greater—ount = 0;
            var equal—ount = 0;
            for (int i = 0; i < a._attributesRequirement.Count; i++)
            {
                var requirement = a._attributesRequirement[i];
                if (requirement.Value > b[requirement.Type].Value)
                {
                    greater—ount++;
                }
                else if (requirement.Value == b[requirement.Type].Value)
                {
                    equal—ount++;
                }
            }
            return greater—ount != 0 || equal—ount == a._attributesRequirement.Count;
        }
        public static bool operator <=(AttributesRequirement a, CharacterAttributes b)
        {
            var count = 0;
            for (int i = 0; i < a._attributesRequirement.Count; i++)
            {
                var requirement = a._attributesRequirement[i];
                if (requirement.Value <= b[requirement.Type].Value)
                {
                    count++;
                }
            }
            return count == a._attributesRequirement.Count;
        }
    }

    [Serializable]
    public class AttributeRequirement
    {
        [SerializeField] private EntityAtributeType _type;
        [SerializeField] private int _value;

        public EntityAtributeType Type => _type;
        public int Value => _value;
    }
}
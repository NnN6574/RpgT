using System;
using UnityEngine;

namespace Mistave.Client.Data.Entity
{
    public enum EntityAtributeType
    {
        Strenght,
        Dexterity,
        Intelligence
    }
 
    [Serializable]
    public class EntityAttribute
    {
        [SerializeField] private EntityAtributeType _type;
        [SerializeField] private int _value;
        [SerializeField] private float _experience;

        public EntityAtributeType Type => _type;
        public int Value => _value;
        public float Experience => _experience;

        public EntityAttribute(EntityAtributeType type, int value)
        {
            _type = type;
            _value = value;
            _experience = 0;
        }

        public void SetValue(int value)
        {
            if (value <= 0)
            {
                _value = 1;
                return;
            }
            _value = value;
        }    
        public void SetExperience(float value)
        {
            if (value < 0)
            {
                _experience = 0;
                return;
            }
            _experience = value;
        }
    }
}
using System;
using System.Collections.Generic;
using Mistave.Client.Data;
using Mistave.Client.Data.Entity;
using UnityEngine;
using Zenject;

namespace Mistave.Client.Character.Data
{
    [Serializable]
    public class CharacterAttributes : DataContainer
    {     
        [Inject] private CharacterAttributesConfig  _attributesConfig;
        [SerializeField] private List<EntityAttribute> _atributes;
        public EntityAttribute this[EntityAtributeType EntityAtributeType]
        {
            get => _atributes.Find(e => e.Type == EntityAtributeType);
        }
        public int GetValue(EntityAtributeType atributeType)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            return atribute.Value;
        }
        public float GetExperience(EntityAtributeType atributeType)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            return atribute.Experience;
        }
        public void SetAtributeValue(EntityAtributeType atributeType, int value)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            SetAtributeValue(atribute, value);
        }
        public void AddAtributeValue(EntityAtributeType atributeType, int value)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            SetAtributeValue(atribute, atribute.Value + value);
        }
        public void RemoveAtributeValue(EntityAtributeType atributeType, int value)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            SetAtributeValue(atribute, atribute.Value - value);
        }

        public void SetAtributeExperience(EntityAtributeType atributeType, float value)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            SetAtributeExperience(atribute, value);
        }
        public void AddAtributeExperience(EntityAtributeType atributeType, float value)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            SetAtributeExperience(atribute, atribute.Experience + value);
        }
        public void RemoveAtributeExperience(EntityAtributeType atributeType, float value)
        {
            var atribute = _atributes.Find(e => e.Type == atributeType);
            SetAtributeExperience(atribute, atribute.Experience - value);
        }

        public override void FromJson(string json)
        {
            if (json == string.Empty)
            {
                _atributes = _attributesConfig.Atributes;
            }
            else
            {
                base.FromJson(json);
            }
        }

        private void SetAtributeValue(EntityAttribute atribute, int value)
        {
            atribute.SetValue(value);
            Save();
        }
        private void SetAtributeExperience(EntityAttribute atribute, float value)
        {
            atribute.SetExperience(value);
            Save();
        }    
    }
}
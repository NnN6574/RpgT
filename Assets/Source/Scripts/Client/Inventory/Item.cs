using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mistave.Client.Data.Inventory
{
    public enum ItemType
    {
        Equipment,
        Material,
        Consumable,
        Quest
    }
    [Serializable]
    public class Item
    {
        [SerializeField] protected ItemType _itemType;
        [SerializeField] protected string _id;
        [SerializeField] protected Rank _rank;
        [SerializeField] protected ItemBaseType _itemBaseType;
        [SerializeField] protected List<PropertyTag> _tags;
        [SerializeField] protected string _title;
        
        protected Sprite _icon;//TODO need icon id and icons library

        public ItemType ItemType => _itemType;
        public string Id => _id;
        public Rank Rank => _rank;
        public ItemBaseType ItemBaseType => _itemBaseType;
        public List<PropertyTag> Tags => _tags;
        public string Title => _title;
        public Sprite Icon => _icon;
        public void SetIcon(Sprite sprite)
        {
            _icon = sprite;
        }
        public Item (Item item, Sprite icon)
        {
            _itemType = item.ItemType;
            _id = item.Id;
            _itemBaseType = item.ItemBaseType;
            _rank = item.Rank;
            _title = item.Title;
            _icon = icon;
        }
    }
}
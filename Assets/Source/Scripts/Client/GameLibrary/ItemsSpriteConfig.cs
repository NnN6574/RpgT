using System;
using System.Collections.Generic;
using Mistave.Client.Data.Inventory;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "ItemsSpriteConfig", menuName = "Data/Library/ItemsSpriteConfig")]
    public class ItemsSpriteConfig : ScriptableObject
    {
        [SerializeField] private List<ItemSpriteElement> _itemsSpriteElements;

        public Sprite GetSprite(ItemBaseType itemBaseType)
        {
            return _itemsSpriteElements.Find(e => e.ItemBaseType == itemBaseType).Sprite;
        }
    }
    [Serializable]
    public class ItemSpriteElement
    {
        [SerializeField] private ItemBaseType _itemBaseType;
        [SerializeField] private Sprite _sprite;

        public ItemBaseType ItemBaseType => _itemBaseType;
        public Sprite Sprite => _sprite;
    }
}

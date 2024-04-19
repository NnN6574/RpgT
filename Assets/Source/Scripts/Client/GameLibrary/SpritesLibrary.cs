using Mistave.Client.Data.Inventory;
using UnityEngine;
using Zenject;

namespace Mistave.Client.Data
{
    public class SpritesLibrary
    {
        [Inject] private ItemsSpriteConfig _itemSpritesContainer;

        public Sprite GetSprite(ItemBaseType itemBaseType)
        {
            return _itemSpritesContainer.GetSprite(itemBaseType);
        }
    }
}
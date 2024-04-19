using Mistave.Client.Data;
using UnityEngine;
using Zenject;

public class LibraryInstaller : MonoInstaller
{
    [SerializeField] private ItemsSpriteConfig _itemsSpriteConfig;
    public override void InstallBindings()
    {
        Container.BindInstance(_itemsSpriteConfig).AsSingle().NonLazy();

        Container.Bind<SpritesLibrary>().AsSingle().NonLazy();
    }
}
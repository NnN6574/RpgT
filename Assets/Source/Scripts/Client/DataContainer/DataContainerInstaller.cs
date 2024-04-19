using Mistave.Client.Character.Data;
using Mistave.Client.Data;
using Mistave.Client.Data.Inventory;
using UnityEngine;
using Zenject;

public class DataContainerInstaller : MonoInstaller<DataContainerInstaller>
{
    [SerializeField] private CharacterPropertiesConfig _characterPropertiesConfig;
    [SerializeField] private CharacterEquipmentConfig _characterEquipmentConfig;
    [SerializeField] private CharacterAttributesConfig _characterAttributesConfig;
    [SerializeField] private ItemsStorageConfig _itemsStorageConfig;
    public override void InstallBindings()
    {      
        Container.BindInstance(_characterAttributesConfig).AsSingle();
        Container.BindInstance(_characterEquipmentConfig).AsSingle();
        Container.BindInstance(_characterPropertiesConfig).AsSingle();
        Container.BindInstance(_itemsStorageConfig).AsSingle();

        Container.BindInterfacesAndSelfTo<CharacterAttributes>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CharacterEquipment>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CharacterProperties>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ItemsStorage>().AsSingle().NonLazy();
    }
}
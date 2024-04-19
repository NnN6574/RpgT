using Mistave.Client.Data;
using Zenject;

public class GameServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DataContainerService>().AsSingle().NonLazy();
    }
}
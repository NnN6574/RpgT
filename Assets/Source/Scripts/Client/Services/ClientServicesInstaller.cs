using Mistave.Client.Data.Property;
using Zenject;

public class ClientServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PropertyCalculator>().AsSingle().NonLazy();
    }
}
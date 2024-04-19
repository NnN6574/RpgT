using NaughtyAttributes;
using Network.Scripts.NetworkCore.Data;
using UnityEngine;
using Zenject;

namespace Network.Scripts.NetworkCore.Installer
{
    public class NetworkInstaller : MonoInstaller
    {
        private const string GROUP_MONITORING = "Debug monitoring";

        [SerializeField] private ProviderNetworking _providerNetworking;
        [SerializeField] private AutoConnectNetworking _autoConnectNetworking;
        [SerializeField] private NetworkSettingConfig _networkSettingConfig;
        [SerializeField, BoxGroup(GROUP_MONITORING)] private bool _isEnableMonitoring;
        [SerializeField, BoxGroup(GROUP_MONITORING)] private GameObject _monitoring;
        
        public override void InstallBindings()
        {
            Container.Bind<NetworkServerStatus>().AsSingle().WithArguments(_networkSettingConfig);
            Container.Bind<NetworkClientStatus>().AsSingle().WithArguments(_networkSettingConfig);

            var networkManager =
            Container.InstantiatePrefabForComponent<ProviderNetworking>(_providerNetworking);
            Container.Bind<ProviderNetworking>().FromInstance(networkManager).AsSingle();
            
            var autoConnection = Container.InstantiatePrefabForComponent<AutoConnectNetworking>(_autoConnectNetworking);
            Container.Bind<AutoConnectNetworking>().FromInstance(autoConnection).AsSingle();
            
            if (_isEnableMonitoring)
            {
                var monitoring = Instantiate(_monitoring);
                DontDestroyOnLoad(monitoring);
            }
        }
    }
}
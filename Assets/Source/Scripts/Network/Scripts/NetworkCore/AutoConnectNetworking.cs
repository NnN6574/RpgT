using System;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Network.Scripts.NetworkCore
{
    public class AutoConnectNetworking : MonoBehaviour
    {
        public event Action OnJoinToHost;
        
        [SerializeField] private bool _isEnableAutoConnection = true;
        
        [Inject] private ProviderNetworking _providerNetworking;

        private NetworkDiscovery _networkDiscovery;
        

        [Inject]
        private void Init()
        {
            _networkDiscovery = GetComponent<NetworkDiscovery>();
            _networkDiscovery.transport = _providerNetworking.GetComponent<TelepathyTransport>();
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void StartFindHost()
        {
            _networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
            _networkDiscovery.StartDiscovery();
        }

        private void CompleteAutoConnection()
        {
            _networkDiscovery.OnServerFound.RemoveAllListeners();
            _networkDiscovery.StopDiscovery();
        }

        private void OnDiscoveredServer(ServerResponse info)
        {
            _providerNetworking.StartClient(info.uri);
            CompleteAutoConnection();
            OnJoinToHost?.Invoke();
            Debug.Log("Connection to Client");
        }


        private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name != "EquipmentScene") return;

            SceneManager.sceneLoaded -= OnSceneLoad;
        }
        
        
        public void LaunchHost()
        {
            NetworkManager.singleton.StartHost();
            _networkDiscovery.AdvertiseServer();
        }

        public void FindToJoinHost()
        {
            if (!_isEnableAutoConnection) return;

            StartFindHost();
        }
    }
}

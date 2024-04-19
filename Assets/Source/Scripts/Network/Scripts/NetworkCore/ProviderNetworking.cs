using System;
using DG.Tweening;
using Mirror;
using UnityEngine;
using Zenject;

namespace Network.Scripts.NetworkCore
{
    public class ProviderNetworking : NetworkManager
    {
        //MIRROR
        //MIRROR_57_0_OR_NEWER
        //MIRROR_58_0_OR_NEWER
        //MIRROR_65_0_OR_NEWER
        //MIRROR_66_0_OR_NEWER
        //MIRROR_2022_9_OR_NEWER
        //MIRROR_2022_10_OR_NEWER
        //MIRROR_70_0_OR_NEWER
        //MIRROR_71_0_OR_NEWER
        //MIRROR_73_OR_NEWER
        //MIRROR_78_OR_NEWER
        //

        public event Action<NetworkConnectionToClient> OnHandleServerDisconnect;
        public event Action<NetworkConnectionToClient> OnHandleServerConnect;

        [SerializeField] private NetworkPrefabInstaller _networkPrefabInstaller;
        
        [Inject] private NetworkClientStatus _networkClientStatus;
        [Inject] private NetworkServerStatus _networkServerStatus;

        private NetworkPrefabInstaller _cachedNetworkPrefabInstaller;
        
        public override void OnClientConnect()
        {
            _networkClientStatus.Ready();
            // NetworkClient.Send(new NetworkStartClient());
            // Debug.Log("Client Connected");
            // NetworkClient.RegisterHandler<NetworkSyncEntitiesInRoomMessage>(UpdateClientPlayersDataInRoom);
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
           // NetworkServer.RegisterHandler<NetworkStartClient>(SpawnPrefabInstaller);
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            Debug.Log($"Connection ready? = {conn.isReady}");
            OnHandleServerConnect?.Invoke(conn);
            Debug.Log($"Server to connection {conn.address}");
            Debug.Log($"Count connection on server = {NetworkServer.connections.Count}");
            Debug.Log("######################################");
        }

        public override void OnClientDisconnect()
        {
            Debug.Log($"Client to disconnection");
            base.OnClientDisconnect();
            DOTween.KillAll();
            _networkClientStatus.Dispose();
            _networkServerStatus.Dispose();
            OnHandleServerDisconnect?.Invoke(null);
        }

        // [Server]
        // private void SpawnPrefabInstaller(NetworkConnectionToClient conn,NetworkStartClient msg)
        // {
        //     var cachedNetworkPrefabInstaller = Instantiate(_networkPrefabInstaller);
        //     cachedNetworkPrefabInstaller.NetworkConnectionToClient = conn;
        //     cachedNetworkPrefabInstaller.ProviderNetworking = this;
        //     NetworkServer.Spawn(cachedNetworkPrefabInstaller.gameObject, conn);
        // }
        //
        // private void UpdateClientPlayersDataInRoom(NetworkSyncEntitiesInRoomMessage msg)
        // {
        //     _networkClientStatus.UpdatePlayersDataInRoom();
        // }
    }
}

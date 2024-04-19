using System;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Network.Scripts.NetworkCore
{
    public class NetworkPrefabInstaller : NetworkBehaviour
    {
        public static NetworkPrefabInstaller Instance;
        public NetworkConnectionToClient NetworkConnectionToClient;
        public ProviderNetworking ProviderNetworking;
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
         
            DontDestroyOnLoad(gameObject);
        }

        public void SpawnSwitchAuthor(uint assetId)
        {
            if (isServer)
            {
                ServerSpawn(assetId, netId);
            }
            else
            {
                CmdSpawn(assetId, netId);
            }
        }

        private NetworkIdentity GetObject(uint assetId)
        {
            foreach (var prefab in ProviderNetworking.spawnPrefabs)
            {
                var net = prefab.GetComponent<NetworkIdentity>();
                if (net.assetId == assetId)
                {
                    return net;
                }
            }

            return null;
        }
        
        private NetworkConnectionToClient GetNetworkLinkObject(uint netId)
        {
            var networkPrefabInstallers = FindObjectsOfType<NetworkPrefabInstaller>();
            foreach (var networkPrefabInstaller in networkPrefabInstallers)
            {
                if (networkPrefabInstaller.netId == netId)
                {
                    return networkPrefabInstaller.connectionToClient;
                }
            }

            return null;
        }

        private void SpawnObject(uint assetId, uint netId)
        {
            var link = GetNetworkLinkObject(netId);

            if (link == null)
            {
                Debug.LogWarning("Not link");
                return;
            }
            
            var refObj = GetObject(assetId);
            if (refObj == null)
            {
                Debug.LogWarning("Null ref");
                return;
            }
            
            var obj = Instantiate(refObj);
            NetworkServer.Spawn(obj.gameObject, link);
        }

        [TargetRpc]
        private void RpcSpawnObject(NetworkConnectionToClient connectionToClient, NetworkIdentity networkIdentity)
        {
            NetworkServer.Spawn(networkIdentity.gameObject, connectionToClient);
        }

        [Server]
        private void ServerSpawn(uint assetId, uint netId)
        {
            SpawnObject(assetId, netId);
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawn(uint assetId, uint netID)
        {
            SpawnObject(assetId, netID);
        }
        
        [Server]
        public void ServerUnSpawn<T>() where T : Object
        {
            var spawned = FindObjectsOfType<T>();

            foreach (var t in spawned)
            {
                NetworkServer.Destroy(t.GameObject());
            }
        }
    }
}
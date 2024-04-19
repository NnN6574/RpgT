using System.Collections.Generic;
using Mirror;
using Network.Scripts.NetworkCore.Data;

namespace Network.Scripts.NetworkCore
{
    public class NetworkServerStatus : NetworkStatus
    {
        public NetworkServerStatus(NetworkSettingConfig networkSettingConfig)
        {
            _networkSettingConfig = networkSettingConfig;
        }
        
        private NetworkSettingConfig _networkSettingConfig;
        
        public bool IsStartGame { get; private set; }
        
        public bool IsAllReadyClients
        {
            get
            {
                int countConnection = Connections.Count;
                int countReady = 0;

                foreach (var connection in Connections)
                {
                    if (connection.Value.isReady)
                    {
                        countReady++;
                    }
                }

                return countReady == countConnection;
            }
        }
        
        public NetworkSettingConfig NetworkSettingConfig => _networkSettingConfig;
        
        public int CountConnection => NetworkServer.connections.Count;

        public int CountConnectionInRoom { get; set; }
        public Dictionary<int, NetworkConnectionToClient> Connections => NetworkServer.connections;
        public void StartGame() => IsStartGame = true;

        public void EndGame() => IsStartGame = false;

        public override void Dispose()
        {
            base.Dispose();
            Connections.Clear();
            IsStartGame = false;
            CountConnectionInRoom = 0;
        }
    }
}
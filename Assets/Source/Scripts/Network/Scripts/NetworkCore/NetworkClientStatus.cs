using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Network.Scripts.NetworkCore.Data;
using Object = UnityEngine.Object;

namespace Network.Scripts.NetworkCore
{
    public class NetworkClientStatus : NetworkStatus
    {

        public event Action UpdateListPlayers;
        public NetworkClientStatus(NetworkSettingConfig networkSettingConfig)
        {
            _networkSettingConfig = networkSettingConfig;
        }
        
        private NetworkSettingConfig _networkSettingConfig;
        public NetworkSettingConfig NetworkSettingConfig => _networkSettingConfig;

        public NetworkIdentity PlayerIdentity => NetworkClient.localPlayer;

        public int CountPlayersInRoom { get; set;}
        public static bool IsStartedGame { get; set; }

        public static List<NetworkIdentity> EntitiesInRoom = new List<NetworkIdentity>();
        public static List<NetworkIdentity> PlayersInRoom = new List<NetworkIdentity>();


        public override void Dispose()
        {
            base.Dispose();
            EntitiesInRoom.Clear();
            PlayersInRoom.Clear();
            IsStartedGame = false;
        }

        public void UpdatePlayersDataInRoom()
        {
            EntitiesInRoom.Clear();
            PlayersInRoom.Clear();
            // EntitiesInRoom.AddRange(Object.FindObjectsOfType<Player>()
            //     .Select(item => item.GetComponent<NetworkIdentity>()).ToList());
            // EntitiesInRoom.AddRange(Object.FindObjectsOfType<NetworkAIController>()
            //     .Select(item => item.GetComponent<NetworkIdentity>()).ToList());
            // PlayersInRoom.AddRange(Object.FindObjectsOfType<Player>()
            //     .Select(item => item.GetComponent<NetworkIdentity>()).ToList());
            
            UpdateListPlayers?.Invoke();
        }
        
        public int GetIndexPlayerData(NetworkIdentity networkIdentity)
        {
            var players = PlayersInRoom;
            for (int i = 0; i < players.Count; i++)
            {
                if (networkIdentity.netId == players[i].netId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
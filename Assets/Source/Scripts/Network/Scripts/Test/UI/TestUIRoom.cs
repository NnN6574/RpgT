using System;
using System.Collections.Generic;
using Mirror;
using Network.Scripts.NetworkCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Network.Scripts.Test.UI
{
    public class TestUIRoom : NetworkBehaviour
    {
        public enum TypeSelectedMap
        {
            Prev,
            Next,
        }
        
        [Serializable]
        public struct VisualMap
        {
            public GameObject View;
            public GameObject Background;
        }
        
        [SerializeField] private VisualMap[] _visualMaps;
        [SerializeField] private Transform _content;
        [SerializeField] private TextMeshProUGUI _textReady;
        [SerializeField] private Button _buttonReady;
        [SerializeField] private Button _buttonPlay;
        [SerializeField] private Button _buttonLeft;
        [SerializeField] private Button _buttonRight;
        [SerializeField] private GameObject _serverMap;
        [SerializeField] private GameObject _clientMap;

        private NetworkServerStatus _networkServerStatus;
        private NetworkClientStatus _networkClientStatus;

        private List<GameObject> _listGenMap = new List<GameObject>();

        private bool IsServer => _networkClientStatus.IsCaseServer;

        [SyncVar(hook = nameof(SyncSelectedMap))]
        private int _serverIndexSelectMap = -1;

        private int _indexSelectMap = 0;

        private void SwitchAuthor(TypeSelectedMap typeSelectedMap)
        {
            if (!IsServer) return;

            switch (typeSelectedMap)
            {
                case TypeSelectedMap.Prev:
                    ServerLeftHandle();
                    break;
                case TypeSelectedMap.Next:
                    ServerRightHandle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeSelectedMap), typeSelectedMap, null);
            }
        }

        [Server]
        private void ServerLeftHandle()
        {
            _indexSelectMap--;

            if (_indexSelectMap < 0)
            {
                _indexSelectMap = _visualMaps.Length-1;
            }
            
            _serverIndexSelectMap =  _indexSelectMap;
          //  NetworkServer.SendToAll(new NetworkSyncIndexMapMessage() {IndexMap = _indexSelectMap});
        }

        [Server]
        private void ServerRightHandle()
        {
            _indexSelectMap++;

            if (_indexSelectMap >= _visualMaps.Length)
            {
                _indexSelectMap = 0;
            }

            _serverIndexSelectMap = _indexSelectMap;
            //NetworkServer.SendToAll(new NetworkSyncIndexMapMessage() {IndexMap = _indexSelectMap});
        }
        
        private void SyncSelectedMap(int oldValue, int newValue)
        {
            _indexSelectMap = newValue;

            for (int i = 0; i < _listGenMap.Count; i++)
            {
                Destroy(_listGenMap[i].gameObject);
            }
            
            _listGenMap.Clear();
            
            _listGenMap.Add(Instantiate(_visualMaps[_indexSelectMap].View, _content));
            _listGenMap.Add(Instantiate(_visualMaps[_indexSelectMap].Background,_content));
        }

        private void Start()
        {
            // DontDestroyOnLoad(gameObject);
            // if (!isOwned)
            // {
            //     gameObject.SetActive(false);
            //     return;
            // }
            //
            // var installer = FindObjectOfType<NetworkRoomSceneInstaller>();
            //
            // if (installer == null)
            // {
            //     Debug.LogWarning("Installer null");
            //     return;
            // }
            //
            // Init(installer.NetworkServerStatus, installer.NetworkClientStatus,
            //     installer.NetworkGameStateMachine, installer.GameLevelsLoader);
            //
            // ClientReady();
            //
            // _buttonReady.onClick.AddListener(ClientReady);
            // _buttonPlay.onClick.AddListener(StartGame);
            //
            //
            // NetworkClient.RegisterHandler<NetworkSyncIndexMapMessage>(OnNetworkSendIndexMapMessage);
            // NetworkClient.RegisterHandler<NetworkSendConnectionsInRoom>(OnNetworkSendConnectionsInRoom);
            //
            // _buttonLeft.onClick.AddListener(() => SwitchAuthor(TypeSelectedMap.Prev));
            // _buttonRight.onClick.AddListener(() => SwitchAuthor(TypeSelectedMap.Next));
            //
            // _buttonLeft.gameObject.SetActive(IsServer);
            // _buttonRight.gameObject.SetActive(IsServer);
            //
            // _serverMap.gameObject.SetActive(IsServer);
            // _clientMap.gameObject.SetActive(!IsServer);
            //
            // if (IsServer)
            // {
            //     _serverIndexSelectMap = _indexSelectMap;
            // }
            //
            // SyncSelectedMap(-1,  _indexSelectMap);
        }

        // private void OnNetworkSendConnectionsInRoom(NetworkSendConnectionsInRoom msg)
        // {
        //     _networkServerStatus.CountConnectionInRoom = msg.Count;
        // }
        //
        // private void OnNetworkSendIndexMapMessage(NetworkSyncIndexMapMessage msg)
        // {
        //     if(isServer) return;
        //     
        //     SyncSelectedMap(-1, msg.IndexMap);
        // }

        // private void ClientReady()
        // {
        //     _networkClientStatus.Ready();
        //     _buttonReady.gameObject.SetActive(false);
        //     string server = "Server ready to start.\nwait all players to ready...";
        //     string client = "Ready...\nWait server to start game...";
        //     string endText = _networkClientStatus.TypeConnection == TypeConnection.Host ? server : client;
        //     string text = $"<color=green>{endText}</color>";
        //     _textReady.text = _networkClientStatus.IsReady ? text : "Not Ready";
        // }
        //
        // private void StartGame()
        // {
        //     if (_networkServerStatus.IsAllReadyClients)
        //     {
        //         var index =_indexSelectMap;
        //
        //         var msgStartLevel = new NetworkLoadLevelMessage()
        //         {
        //             IndexLevel = index
        //         };
        //
        //         var msgCountConnection = new NetworkSendConnectionsInRoom()
        //         {
        //             Count = NetworkServer.connections.Count
        //         };
        //         NetworkPrefabInstaller.Instance.ServerUnSpawn<TestUIRoom>();
        //         
        //         NetworkServer.SendToAll(msgStartLevel);
        //         NetworkServer.SendToAll(msgCountConnection);
        //     }
        // }
        //
        // private void OnMessageLoadLevel(NetworkLoadLevelMessage levelMessage)
        // {
        //     _networkGameStateMachine.Enter<NetworkLoadingLevelState, int>(levelMessage.IndexLevel);
        //     NetworkClient.DestroyAllClientObjects();
        // }
        //
        // public void Init(NetworkServerStatus networkServerStatus
        //     , NetworkClientStatus networkClientStatus
        //     , NetworkGameStateMachine networkGameStateMachine
        //     , GameLevelsLoader gameLevelsLoader)
        // {
        //     _networkGameStateMachine = networkGameStateMachine;
        //     _gameLevelsLoader = gameLevelsLoader;
        //     _networkClientStatus = networkClientStatus;
        //     _networkServerStatus = networkServerStatus;
        //     _buttonPlay.gameObject.SetActive(_networkClientStatus.TypeConnection == TypeConnection.Host);
        //     _textReady.text = _networkClientStatus.IsReady ? "Ready" : "Not Ready";
        //     
        //     NetworkClient.RegisterHandler<NetworkLoadLevelMessage>(OnMessageLoadLevel);
        // }

    }
}
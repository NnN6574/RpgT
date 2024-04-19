using Network.Scripts.NetworkCore;
using UnityEngine;
using UnityEngine.UI;

namespace Network.Scripts.Test.UI
{
    public class TestUISwitchRuleRoom : MonoBehaviour
    {
        [SerializeField] private Button _buttonHost;
        [SerializeField] private Button _buttonAutoJoin;
        [SerializeField] private GameObject _groupSwitchModeMultiplayer;
        [SerializeField] private GameObject _objConnection;

        private NetworkStatus _networkStatus;
        
     //   private NetworkGameStateMachine _networkGameStateMachine;
        
        private void Construct()
        {
            _buttonHost.onClick.AddListener(Host);
            _buttonAutoJoin.onClick.AddListener(AutoJoin);
        }

        private void Host()
        {
            _networkStatus.SetHost();
           // _networkGameStateMachine.Enter<NetworkLoadingRoomState>();
        }

        private void AutoJoin()
        {
            _networkStatus.SetClient();
            _groupSwitchModeMultiplayer.gameObject.SetActive(false);
            _objConnection.SetActive(true);
            //_networkGameStateMachine.Enter<NetworkLoadingRoomState>();
        }

        // public void Init(NetworkStatus networkStatus, NetworkGameStateMachine networkGameStateMachine)
        // {
        //    // _networkGameStateMachine = networkGameStateMachine;
        //     _networkStatus = networkStatus;
        //     _buttonHost.onClick.AddListener(Host);
        //     _buttonAutoJoin.onClick.AddListener(AutoJoin);
        // }
    }
}
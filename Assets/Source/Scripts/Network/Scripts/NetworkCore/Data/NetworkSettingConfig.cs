using NaughtyAttributes;
using Network.Scripts.NetworkCore.Constant;
using UnityEngine;

namespace Network.Scripts.NetworkCore.Data
{
    [CreateAssetMenu(menuName = NetworkScriptableObjectPath.PathNetworkSetting)]
    public class NetworkSettingConfig : ScriptableObject
    {
        [SerializeField, Scene] private string _roomScene;
        [SerializeField, Scene] private string _switchRuleRoomScene;

        public string RoomScene => _roomScene;
        public string SwitchRuleRoomScene => _switchRuleRoomScene;
    }
}
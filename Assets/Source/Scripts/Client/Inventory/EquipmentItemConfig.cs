using Mistave.Client.Data.Inventory;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "EquipmentItemConfig", menuName = "Data/Configs/EquipmentItemConfig")]
    public class EquipmentItemConfig : ScriptableObject
    {
        [SerializeField] private EquipmentItem _item;
        public EquipmentItem Item => _item;
    }
}
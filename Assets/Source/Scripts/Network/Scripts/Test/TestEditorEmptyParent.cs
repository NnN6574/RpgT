using UnityEngine;

namespace Network.Scripts.Test
{
    public class TestEditorEmptyParent: MonoBehaviour
    {
        [NaughtyAttributes.Button("Test")]
        public void CreateEmptyParent()
        {
            GameObject parent = new GameObject($"Parent_x{transform.position.x}_z{transform.position.z}");
            parent.transform.parent = transform;
        }
    }
}
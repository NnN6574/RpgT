using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mistave.Client.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
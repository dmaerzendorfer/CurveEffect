using UnityEngine;

namespace _Project.Scripts.Runtime.Manager
{
    public class OnLoadHooker : MonoBehaviour
    {
        // Runs before a scene gets loaded
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadMain()
        {
            GameObject managers = GameObject.Instantiate(Resources.Load("Managers")) as GameObject;
            GameObject.DontDestroyOnLoad(managers);
        }
        // You can choose to add any "Service" component to the Main prefab.
        // Examples are: Input, Saving, Sound, Config, Asset Bundles, Advertisements
    }
}

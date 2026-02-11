using _Project.Scripts.Runtime.Manager.Audio;
using _Project.Scripts.Runtime.Utility;

namespace _Project.Scripts.Runtime.Manager
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public SceneChanger _sc;
        public AudioManager _am;

        private void Start()
        {
            if (!_sc)
            {
                _sc = SceneChanger.Instance;
            }

            if (!_am)
            {
                _am = AudioManager.Instance;
            }
            
        }
    }
}
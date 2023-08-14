// --------------------------------------------------------- 
// Singleton.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// ---------------------------------------------------------

using UnityEngine;
namespace System
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        #region variable

        public static T Instance { get; private set; }

        #endregion
        #region method

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}

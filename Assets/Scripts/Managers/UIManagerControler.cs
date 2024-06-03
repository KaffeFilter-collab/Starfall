using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    public class UIManagerControler : MonoBehaviour
    {
    
        public static UIManagerControler Instance {get; private set; }
        public UIDocument uiDocument;
    
        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }
    
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        #region Timerbar
        
        

        #endregion

        #region Inventory

    

        #endregion

        #region Settings

    

        #endregion
    }
}
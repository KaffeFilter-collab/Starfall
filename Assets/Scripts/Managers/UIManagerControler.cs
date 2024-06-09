using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;



namespace Managers
{
    public class UIManagerControler : MonoBehaviour
    {

        public static UIManagerControler Instance { get; private set; }
        public UIDocument uiDocument;

        [SerializeField] private VisualTreeAsset standartOverlay;
        [SerializeField] private VisualTreeAsset inventory;
        [SerializeField] private VisualTreeAsset menu;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
            uiDocument = GetComponent<UIDocument>();
            GetInventoryRefrences();
            GetSettingsRefrences();
            GetInInventoryCodeRefrences();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public bool CheckCurrentUI(VisualTreeAsset check)
        {
            if (uiDocument.visualTreeAsset == check) return true;
            return false;
        }

        #region Timerbar



        #endregion

        #region Inventory

        private const string N_Inventorybutton = "Inventory";
        private Button inventoryButton;

        //In inventory
        private const string N_InventoryInInventoryButton = "InventoryInInventory";
        private Button inventoryInInventoryButton;

        public void GetInventoryRefrences()
        {
            inventoryButton = uiDocument.rootVisualElement.Q<Button>(N_Inventorybutton);
            inventoryButton.clicked += InventoryButtonOnClicked;
        }

        public void GetInInventoryRefrences()
        {
            inventoryInInventoryButton = uiDocument.rootVisualElement.Q<Button>(N_InventoryInInventoryButton);
            inventoryInInventoryButton.clicked += InventoryInInventoryButtonOnClicked;
        }

        private void InventoryButtonOnClicked()
        {
            Debug.Log("button works");
            InventoryToggle();
        }

        private void InventoryInInventoryButtonOnClicked()
        {
            InventoryToggle();
        }


        public void InventoryToggle()
        {
            if (CheckCurrentUI(inventory))
            {
                uiDocument.visualTreeAsset = standartOverlay;
                GetInventoryRefrences();
                GetSettingsRefrences();
            }
            else
            {
                uiDocument.visualTreeAsset = inventory;
                GetInInventoryRefrences();
                GetSettingsRefrences();
            }
        }

        #region InInventoryCode

        public List<InventorySlot> InventoryItems = new List<InventorySlot>();
        private VisualElement m_Root;
        private VisualElement m_SlotContainer;
        private static VisualElement m_GhostIcon;

        private void GetInInventoryCodeRefrences()
        {
            m_Root = GetComponent<UIDocument>().rootVisualElement;
            m_GhostIcon = m_Root.Query<VisualElement>("GhostIcon");
            m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");
            GameController.OnInventoryChanged += GameController_OnInventoryChanged;
            for (int i = 0; i < 20; i++)
            {
                InventorySlot item = new InventorySlot();
                InventoryItems.Add(item);
                //m_SlotContainer.Add(item);
            }
        }

        public void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
            {
                //Loop through each item and if it has been picked up, add it to the next empty slot
                foreach (string item in itemGuid)
                {
                    if (change == InventoryChangeType.Pickup)
                    {
                        var emptySlot = InventoryItems.FirstOrDefault(x => x.ItemGuid.Equals(""));
                    
                        if (emptySlot != null)
                        {
                            emptySlot.HoldItem(GameController.GetItemByGuid(item));
                        }
                    }
                }
            }
        

    #endregion
        #endregion

        #region Settings

        private const string N_SettingsButton = "Menu";
        private Button settingsButton;
        
        //InSettings


        private const string N_ResumeButton = "Resume";
        private const string N_QuitButton = "Quit";
        private const string N_inSettingsButton = "inSettingsButton";
        private Button inSettingsButton;
        private Button resumeButton;
        private Button quitButton;
        
        public void GetSettingsRefrences()
        {
            settingsButton = uiDocument.rootVisualElement.Q<Button>(N_SettingsButton);
            settingsButton.clicked +=OnSettingsButtonClicked ;
        }

        public void GetInSettingsRefrences()
        {
            resumeButton = uiDocument.rootVisualElement.Q<Button>(N_ResumeButton);
            resumeButton.clicked += OnSettingsButtonClicked;
            quitButton = uiDocument.rootVisualElement.Q<Button>(N_QuitButton);
            quitButton.clicked += OnQuitButtonClicked;
        }

        private void OnSettingsButtonClicked()
        {
            if (CheckCurrentUI(menu))
            {
                uiDocument.visualTreeAsset = standartOverlay;
                GetSettingsRefrences();
                GetInventoryRefrences();
            }
            else
            {
                uiDocument.visualTreeAsset = menu;
                GetInSettingsRefrences();
            }
        }
        
        private void OnQuitButtonClicked()
        {
            SceneManager.LoadScene(0);
        }

        #endregion
    }
}
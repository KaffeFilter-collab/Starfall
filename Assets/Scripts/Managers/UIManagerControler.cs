using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;


namespace Managers
{
    public class UIManagerControler : MonoBehaviour
    {

        public static UIManagerControler Instance { get; private set; }
        private UIDocument uiDocument;
        private VisualElement root;
        private UiEnum CurrentPanel;
        [SerializeField] private UiEnum StartPanel;
        
        //Ui enum
        public enum UiEnum
        {
            Enviorment,
            MainMenu,
            NormalGame,
            PauseMenu
        }
        
        

        private List<VisualElement> Panels;
        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
            //End of thingy
            
            
            uiDocument = GetComponent<UIDocument>();
            root = uiDocument.rootVisualElement;
            GetSetupPanels();
            GetMainMenuRefrences();
            GetInventoryRefrences();
            GetSettingsRefrences();
            GetInSettingsRefrences();
            ChangePanel(StartPanel);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void GetSetupPanels() {
            Panels = root.Query<VisualElement>(className: "panel").ToList();
        }

        public void ChangePanel(UiEnum newUI) {
            Panels[(int)CurrentPanel].style.display = DisplayStyle.None;
            Panels[(int) newUI].style.display = DisplayStyle.Flex;
            CurrentPanel = newUI;
        }

        public void SetEnviorment(Texture2D texture)
        {
            Panels[(int)UiEnum.Enviorment].style.backgroundImage = new StyleBackground(texture);
        }
        
        #region Timerbar



        #endregion

        #region Inventory

        private VisualElement inventory;
        
        public void GetInventoryRefrences()
        {
            inventory = Panels[(int)UiEnum.NormalGame].Q<VisualElement>("InventoryOverlay");
            Panels[(int)UiEnum.NormalGame].Q<Button>("Inventory").clicked += InventoryToggle;
            m_SlotContainer = Panels[(int)UiEnum.NormalGame].Q<VisualElement>(className:"slotsContainer");
            GameController.OnInventoryChanged += OnInventoryChanged;
            for (int i = 0; i < 20; i++)
            {
                InventorySlot item = new InventorySlot();
                InventoryItems.Add(item);
                m_SlotContainer.Add(item);
            }
        }
        
        
        public void InventoryToggle()
        {
            inventory.style.visibility = inventory.style.visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        #region InInventoryCode

        public List<InventorySlot> InventoryItems = new List<InventorySlot>();
        private VisualElement m_SlotContainer;
        

        public void OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
        {
            Debug.Log("inventory is being changed");
            //Loop through each item and if it has been picked up, add it to the next empty slot
            int i = 0;
            foreach (string item in itemGuid)
            {
                InventorySlot emptySlot = null;
                foreach (var slot in InventoryItems)
                {
                    Debug.Log(item);
                    if (slot.ItemGuid != null && slot.ItemGuid.Equals(""))
                    {
                        emptySlot = slot;
                        break;
                    }
                }
                Debug.Log(emptySlot);
                i++;
                if (emptySlot != null)
                {
                    emptySlot.HoldItem(GameController.GetItemByGuid(item));
                }
            }
        }
        

            #endregion
        #endregion

        #region PausMenu
        public void GetSettingsRefrences()
        {
            Panels[(int)UiEnum.NormalGame].Q<Button>("Menu").clicked += OnSettingsButtonClicked;
        }

        public void GetInSettingsRefrences()
        {
            Panels[(int)UiEnum.PauseMenu].Q<Button>("Resume").clicked += OnSettingsButtonClicked;
            Panels[(int)UiEnum.PauseMenu].Q<Button>("Quit").clicked += OnQuitButtonClicked; 
        }

        private void OnSettingsButtonClicked()
        {
            if (CurrentPanel != UiEnum.PauseMenu)
            {
                ChangePanel(UiEnum.PauseMenu);
            }
            else
            {
                ChangePanel(UiEnum.NormalGame);
            }
        }
        
        private void OnQuitButtonClicked()
        {
            ChangePanel(UiEnum.MainMenu);
            SceneManager.LoadScene(0);
        }

        #endregion

        #region MainMenu

        public void GetMainMenuRefrences()
        {
            Panels[(int)UiEnum.MainMenu].Q<Button>("NewGame").clicked += StartNewGame;
            Panels[(int)UiEnum.MainMenu].Q<Button>("Quit").clicked += EndGame;
        }

        public void StartNewGame()
        {
            ChangePanel(UiEnum.NormalGame);
            SceneManager.LoadScene(1);
        }

        public void EndGame()
        {
            Application.Quit();
        }

        #endregion
    }
}
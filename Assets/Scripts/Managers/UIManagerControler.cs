using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;



namespace Managers
{
    public class UIManagerControler : MonoBehaviour
    {
    
        public static UIManagerControler Instance {get; private set; }
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
            }
            else
            {
                uiDocument.visualTreeAsset = inventory;
                GetInInventoryRefrences();
            }
        }

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
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using Manager;
using Unity.VisualScripting;
using UnityEditor;


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
            Intro,
            NormalGame,
            PauseMenu,
            Dialogue,
            Options
        }



        private List<VisualElement> Panels;

        //DialougeSystem
        [SerializeField] private UIManagerControler m_ChoiceControllerPrefab;
        [SerializeField] private DialogueChannel m_DialogueChannel;

        private DialogueNode m_NextNode = null;

        [SerializeField]
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
            GetArrowRefrences();
            GetInventoryRefrences();
            GetSettingsRefrences();
            GetInSettingsRefrences();
            GetItemRefrences();
            GetDialogueRefrences();
            GetSoundSettings();
            ChangePanel(StartPanel);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void GetSetupPanels()
        {
            Panels = root.Query<VisualElement>(className: "panel").ToList();
        }

        public void ChangePanel(UiEnum newUI)
        {
            Panels[(int)CurrentPanel].style.display = DisplayStyle.None;
            Panels[(int)newUI].style.display = DisplayStyle.Flex;
            CurrentPanel = newUI;
        }

        public void SetEnviorment(Texture2D texture)
        {
            Panels[(int)UiEnum.Enviorment].style.visibility = Visibility.Visible;
            Panels[(int)UiEnum.Enviorment].style.display = DisplayStyle.Flex;
            Panels[(int)UiEnum.Enviorment].style.backgroundImage = new StyleBackground(texture);
        }

        #region Timerbar



        #endregion

        #region tempArrows

        public int currentRoom;

        public void GetArrowRefrences()
        {
            Panels[(int)UiEnum.NormalGame].Q<Button>("ArrowLeft").clicked += GoLeft;
            Panels[(int)UiEnum.NormalGame].Q<Button>("ArrowRight").clicked += GoRight;
        }

        public void GoLeft()
        {
            if (currentRoom != 0) GameController.Instance.EnviormentChange(currentRoom--);
        }

        public void GoRight()
        {
            if (currentRoom != 5) GameController.Instance.EnviormentChange(currentRoom++);
        }

        #endregion

        #region Inventory

        private VisualElement inventory;

        public void GetInventoryRefrences()
        {
            inventory = Panels[(int)UiEnum.NormalGame].Q<VisualElement>("InventoryOverlay");
            Panels[(int)UiEnum.NormalGame].Q<Button>("Inventory").clicked += InventoryToggle;
            m_SlotContainer = Panels[(int)UiEnum.NormalGame].Q<VisualElement>(className: "slotsContainer");
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
            SceneManager.LoadScene(1);
            ChangePanel(UiEnum.NormalGame);
        }

        public void EndGame()
        {
            Application.Quit();
        }

        #endregion
        
        #region items

        public void GetItemRefrences()
        {
            List<Button> items = Panels[(int)UiEnum.NormalGame].Query<Button>(className: "item").ToList();
            foreach (var button in items)
            {
                button.AddToClassList("duBistEs");
                button.clicked += ItemsOnClicked;
            }
        }

        public void ItemsOnClicked()
        {
            Button button = Panels[(int)UiEnum.NormalGame].Q<Button>(className: "duBistEs");
            StartCoroutine(GotoInventory(button));
            // todo: move to end of coroutine

        }

        IEnumerator GotoInventory(VisualElement item)
        {
            Debug.Log(item.resolvedStyle.top);
            Vector2 currentpostion = new Vector2(item.resolvedStyle.left, item.resolvedStyle.top);
            while (currentpostion.x <= 7.915752f && currentpostion.y >= -4.330247f)
            {
                currentpostion = Vector3.LerpUnclamped(currentpostion, new Vector3(8.37f, -4.55f, -1), 0.05f);
                ;
                item.style.top = currentpostion.x;
                item.style.left = currentpostion.y;
                yield return new WaitForSeconds(0.02f);
            }

            Debug.Log("Angekommen =)");
            item.parent.Remove(item);
        }

        #endregion
        
        #region Dialogue

        private void GetDialogueRefrences()
        {
            Panels[(int)UiEnum.Dialogue].Q<Button>("YesButton").clicked += OnSettingsButtonClicked;
            Panels[(int)UiEnum.Dialogue].Q<Button>("NoButton").clicked += OnSettingsButtonClicked;
        }

        #endregion

        #region Soundsettings

        private Slider _master;
        private Slider _music;
        private Slider _sfx;

        public void GetSoundSettings()
        {
            _master = Panels[(int)UiEnum.Options].Q<Slider>("MasterSound");
            _music = Panels[(int)UiEnum.Options].Q<Slider>("MusicSound");
            _sfx = Panels[(int)UiEnum.Options].Q<Slider>("SFXSound");
            _master.RegisterCallback<ChangeEvent<float>>((evt) => AudioManager.Instance.SetVolume(AudioManager.MixerGroups.MasterVolume, evt.newValue));
            _music.RegisterCallback<ChangeEvent<float>>((evt) => AudioManager.Instance.SetVolume(AudioManager.MixerGroups.MusicVolume, evt.newValue));
            _sfx.RegisterCallback<ChangeEvent<float>>((evt) => AudioManager.Instance.SetVolume(AudioManager.MixerGroups.SfxVolume, evt.newValue));
        }


         #endregion
    }

}

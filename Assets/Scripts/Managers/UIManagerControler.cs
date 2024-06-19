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
            Items,
            MainMenu,
            Intro,
            NormalGame,
            PauseMenu,
            Dialogue,
            Options,
            Codepad,
        }
        
        [SerializeField]private List<ChoiceDialogueNode> FirstNodes;
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
            GetTimebar();
            GetItemsForItemChanges();
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

        private void Update()
        {
            if (Time.time == Time.time + beginingtime)
            {
                timer.text = "100";
            }
                
        }

        private VisualElement bridge;
        private VisualElement firsthallway;
        private VisualElement secondHallway;
        private VisualElement captain;
        private VisualElement coCaptain;
        private VisualElement crewRoom;
        private VisualElement loungeRoom;
        private VisualElement medbay;
        private VisualElement oxygenRoomBook;


        private Button _notworking;
        private Button _working;
        private Button _doorLounge;
        private Button _doorMedbay;
        private Button _doorClosed;
        private Button _doorBridge;
        private Button _doorCrew;
        private Button _doorCoCaptainRoom;
        private Button _doorCaptain;
        private Button _doorHallway;
        private Button _interactableHeroin;
        private Button _interactableNote;
        private Button _interactableLocker;
        private Button _interactableLoungeNotes;
        private Button _interactable;
        private Button _interactableMedBayLocker;
        private Button _interactableBook;
        private Button _interactableCodePad;

        private List<VisualElement> OverarchingItemElements;
        public void GetItemsForItemChanges()
        {
            bridge = Panels[(int)UiEnum.Items].Q<VisualElement>("Bridge");
            firsthallway = Panels[(int)UiEnum.Items].Q<VisualElement>("1Hallway");
            secondHallway = Panels[(int)UiEnum.Items].Q<VisualElement>("2Hallway");
            captain = Panels[(int)UiEnum.Items].Q<VisualElement>("Captian");
            coCaptain = Panels[(int)UiEnum.Items].Q<VisualElement>("CoCaptain");
            crewRoom = Panels[(int)UiEnum.Items].Q<VisualElement>("CrewRoom");
            loungeRoom = Panels[(int)UiEnum.Items].Q<VisualElement>("LoungeRoom");
            medbay= Panels[(int)UiEnum.Items].Q<VisualElement>("Medbay");
            oxygenRoomBook = Panels[(int)UiEnum.Items].Q<VisualElement>("OxygenRoomBook");

            OverarchingItemElements = Panels[(int)UiEnum.Items].Query<VisualElement>(classes: "Container").ToList();
                
            _notworking = Panels[(int)UiEnum.Items].Q<Button>("NotworkingButton");
            _working = Panels[(int)UiEnum.Items].Q<Button>("WorkingButton");
            _doorLounge = Panels[(int)UiEnum.Items].Q<Button>("DoorLounge");
            _doorMedbay = Panels[(int)UiEnum.Items].Q<Button>("DoorMedbay");
            _doorClosed = Panels[(int)UiEnum.Items].Q<Button>("DoorClosed");
            _doorBridge = Panels[(int)UiEnum.Items].Q<Button>("BrdigeGate");
            _doorCrew = Panels[(int)UiEnum.Items].Q<Button>("CrewDoor");
            _doorCoCaptainRoom = Panels[(int)UiEnum.Items].Q<Button>("CoCaptainDoor");
            _doorCaptain = Panels[(int)UiEnum.Items].Q<Button>("CaptainDoor");
            _doorHallway = Panels[(int)UiEnum.Items].Q<Button>("Hallway1Door");
            _interactableHeroin = Panels[(int)UiEnum.Items].Q<Button>("Heroin");
            _interactableNote = Panels[(int)UiEnum.Items].Q<Button>("Note");
            _interactableLocker = Panels[(int)UiEnum.Items].Q<Button>("Locker");
            _interactableLoungeNotes = Panels[(int)UiEnum.Items].Q<Button>("LoungeNotes");
            _interactableMedBayLocker = Panels[(int)UiEnum.Items].Q<Button>("MedbayLocker");
            _interactableBook = Panels[(int)UiEnum.Items].Q<Button>("Book");
            _interactableCodePad = Panels[(int)UiEnum.Items].Q<Button>("CodePad");

            _notworking.clicked += NotworkingOnclicked;
            _working.clicked += WorkingOnclicked;
            _doorLounge.clicked += DoorLoungeOnclicked;
            _doorMedbay.clicked += DoorMedbayOnclicked;
            _doorClosed.clicked += DoorClosedOnclicked;
            _doorBridge.clicked += DoorBridgeOnclicked;
            _doorCrew.clicked += DoorCrewOnclicked;
            _doorCoCaptainRoom.clicked += DoorCoCaptainRoomOnclicked;
            _doorCaptain.clicked += DoorCaptainOnclicked;
            _doorHallway.clicked += DoorHallwayOnclicked;
            _interactableHeroin.clicked += InteractableHeroinOnclicked;
            _interactableNote.clicked += InteractableNoteOnclicked;
            _interactableLocker.clicked += InteractableLockerOnclicked;
            _interactableLoungeNotes.clicked += InteractableLoungeNotesOnclicked;
            _interactableMedBayLocker.clicked += InteractableMedBayLockerOnclicked;
            _interactableBook.clicked += InteractableBookOnclicked;
            _interactableCodePad.clicked += InteractableCodePadOnclicked;
        }

        #region CBT

        

        private void InteractableCodePadOnclicked()
        {
            throw new NotImplementedException();
        }

        private void InteractableBookOnclicked()
        {
            throw new NotImplementedException();
        }

        private void InteractableMedBayLockerOnclicked()
        {
            throw new NotImplementedException();
        }

        private void InteractableLoungeNotesOnclicked()
        {
            throw new NotImplementedException();
        }

        private void InteractableLockerOnclicked()
        {
            throw new NotImplementedException();
        }

        private void InteractableNoteOnclicked()
        {
            throw new NotImplementedException();
        }

        private void InteractableHeroinOnclicked()
        {
            throw new NotImplementedException();
        }

        private void DoorHallwayOnclicked()
        {
            GameController.Instance.EnviormentChange(1);
        }

        private void DoorCaptainOnclicked()
        {
            GameController.Instance.EnviormentChange(3);
        }

        private void DoorCoCaptainRoomOnclicked()
        {
            GameController.Instance.EnviormentChange(4);
        }

        private void DoorCrewOnclicked()
        {
            GameController.Instance.EnviormentChange(5);
        }

        private void DoorBridgeOnclicked()
        {
            GameController.Instance.EnviormentChange(0);

        }

        private void DoorClosedOnclicked()
        {
            throw new NotImplementedException();
        }

        private void DoorMedbayOnclicked()
        {
            GameController.Instance.EnviormentChange(7);
        }

        private void DoorLoungeOnclicked()
        {
            GameController.Instance.EnviormentChange(6);
        }

        private void WorkingOnclicked()
        {
            throw new NotImplementedException();
        }

        private void NotworkingOnclicked()
        {
            throw new NotImplementedException();
        }

        #endregion


        public void SetEnviorment(Texture2D texture , int foritem)
        {
            Panels[(int)UiEnum.Enviorment].style.visibility = Visibility.Visible;
            Panels[(int)UiEnum.Enviorment].style.display = DisplayStyle.Flex;
            Panels[(int)UiEnum.Enviorment].style.backgroundImage = new StyleBackground(texture);

            Panels[(int)UiEnum.Items].style.visibility = Visibility.Visible;
            Panels[(int)UiEnum.Items].style.display = DisplayStyle.Flex;
            for (int i = 0; i < OverarchingItemElements.Count; i++)
            {
                OverarchingItemElements[i].style.display = DisplayStyle.None;
                OverarchingItemElements[i].style.visibility = Visibility.Hidden;
            }
            
            switch (foritem)
            {
                case 0:
                    bridge.style.visibility = Visibility.Visible;
                    _notworking.style.display = DisplayStyle.Flex;
                    _working.style.display = DisplayStyle.Flex;

                    _notworking.style.visibility = Visibility.Visible;
                    _working.style.visibility = Visibility.Visible;
                    break;
                case 1:
                    firsthallway.style.display = DisplayStyle.Flex;
                    firsthallway.style.visibility = Visibility.Visible;
                    Debug.Log("2");
                    _doorMedbay.style.display = DisplayStyle.Flex;
                    _doorClosed.style.display = DisplayStyle.Flex;
                    _doorBridge.style.display = DisplayStyle.Flex;
                    _doorLounge.style.display = DisplayStyle.Flex;
                    
                    _doorMedbay.style.visibility = Visibility.Visible;
                    _doorClosed.style.visibility = Visibility.Visible ;
                    _doorBridge.style.visibility = Visibility.Visible;
                    _doorLounge.style.visibility = Visibility.Visible;
                    break;
                case 2:
                    secondHallway.style.visibility = Visibility.Visible;
                    Debug.Log("3");
                    _doorCrew.style.display = DisplayStyle.Flex;
                    _doorCaptain.style.display = DisplayStyle.Flex;
                    firsthallway.style.display = DisplayStyle.Flex;
                    _doorCoCaptainRoom.style.display = DisplayStyle.Flex;
                    
                    _doorCrew.style.visibility = Visibility.Visible;
                    _doorCaptain.style.visibility = Visibility.Visible;
                    firsthallway.style.visibility = Visibility.Visible;
                    _doorCoCaptainRoom.style.visibility = Visibility.Visible;
                    break;
                case 3:
                    _interactableHeroin.style.visibility = Visibility.Visible;
                    _interactableHeroin.style.display = DisplayStyle.Flex;
                    captain.style.visibility = Visibility.Visible;
                    break;
                case 4:
                    Debug.Log("5");
                    _interactableNote.style.visibility = Visibility.Visible;
                    _interactableNote.style.display = DisplayStyle.Flex;

                    coCaptain.style.visibility = Visibility.Visible;
                    break;
                case 5:
                    Debug.Log("6");
                    _interactableLocker.style.visibility = Visibility.Visible;
                    _interactableLocker.style.display = DisplayStyle.Flex;
                    crewRoom.style.visibility = Visibility.Visible;
                    break;
                case 6:
                    Debug.Log("7");
                    _interactableLoungeNotes.style.visibility = Visibility.Visible;
                    _interactableLoungeNotes.style.display = DisplayStyle.Flex;
                    loungeRoom.style.visibility = Visibility.Visible;
                    break;
                case 7:
                    Debug.Log("8");
                    _interactableMedBayLocker.style.visibility = Visibility.Visible;
                    _interactableMedBayLocker.style.display = DisplayStyle.Flex;
                    medbay.style.visibility = Visibility.Visible;
                    break;
                case 8:
                    Debug.Log("9");
                    _interactableBook.style.visibility = Visibility.Visible;
                    _interactableBook.style.display = DisplayStyle.Flex;
                    _interactableCodePad.style.display = DisplayStyle.Flex;
                    _interactableCodePad.style.visibility = Visibility.Visible;
                    oxygenRoomBook.style.visibility = Visibility.Visible;
                    break;
            }
        }

        #region Timerbar

        private float beginingtime;
        private Label timer;
        public void GetTimebar()
        {
            timer = Panels[(int)UiEnum.Enviorment].Q<Label>("Timer");
            timer.text = "999";
        }

        private void GetDeltaTime()
        {
            beginingtime = Time.time;
        }

        #endregion

        #region tempArrows

        public int currentRoom;

        public void GetArrowRefrences()
        {
            Panels[(int)UiEnum.NormalGame].Q<Button>("ArrowLeft").clicked += ()=>GoLeft(2);
            Panels[(int)UiEnum.NormalGame].Q<Button>("ArrowRight").clicked += GoRight;
        }

        public void GoLeft(int test)
        {
            if (currentRoom != 0) GameController.Instance.EnviormentChange(2);
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
            Debug.Log(Panels[(int)UiEnum.NormalGame].Q<Button>("Inventory"));
        }


        public void InventoryToggle()
        {
            Debug.Log("InventoryToggle");
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
            GetDeltaTime();
            GameController.Instance.EnviormentChange((0));
            ChangePanel(UiEnum.NormalGame);
        }

        public void EndGame()
        {
            Application.Quit();
        }

        #endregion
        
        #region items
        
        private List<VisualElement> buttonContainer;
        private List<Button> items;
        public void GetItemRefrences()
        {
            buttonContainer = Panels[(int)UiEnum.Items].Query<VisualElement>(className: "Container").ToList();
            items = Panels[(int)UiEnum.Items].Query<Button>(className: "Interactable").ToList();
            for (int i=0;i <items.Count;i++)
            {
                items[i].AddToClassList("duBistEs");
                items[i].clicked += ItemsOnClicked;
                items[i].style.visibility = Visibility.Hidden;
            }
        }

        public void EnabledItems(int itemindex)
        {
            for (int i=0;i <items.Count;i++)
            {
                items[i].parent.style.visibility = Visibility.Hidden;
            }
        }

        public void ItemsOnClicked()
        {
            Debug.Log("clicked");
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
                Debug.Log("ItemInWhile"+ currentpostion);
                currentpostion = Vector3.LerpUnclamped(currentpostion, new Vector3(8.37f, -4.55f, -1), 0.05f);
                item.style.top = currentpostion.x;
                item.style.left = currentpostion.y;
                yield return new WaitForSeconds(0.02f);
            }
            Debug.Log("Angekommen =)");
            item.parent.Remove(item);
        }
        #endregion
        
        #region Dialogue

        private Label dialogueText;
        private void GetDialogueRefrences()
        {
            DialogueSequencer.OnDialogueStart += OnDialogueStart;
            Panels[(int)UiEnum.Dialogue].Q<Button>("YesButton").clicked += DialogueYes;
            Panels[(int)UiEnum.Dialogue].Q<Button>("NoButton").clicked += DialogueNo;
            dialogueText = Panels[(int)UiEnum.Dialogue].Q<Label>("Dialogue");
        }

        private void DialogueYes()
        {
            
        }

        private void DialogueNo()
        {
            
        }
        private void OnDialogueStart(Dialogue dialogue)
        {
            ChangePanel(UiEnum.Dialogue);
            string text = dialogue.FirstNode.DialogueLine.Text;
            dialogueText.text = text;
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

[Serializable]
public class ItemDetails
{
    public string Name;
    public string GUID;
    public Sprite Icon;
    public bool CanDrop;
}

public enum InventoryChangeType
{
    Pickup,
    Drop
}
public delegate void OnInventoryChangedDelegate(string[] itemGuid, InventoryChangeType change);

/// <summary>
/// Generates and controls access to the Item Database and Inventory Data
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    public List<Sprite> IconSprites;
    private static Dictionary<string, ItemDetails> m_ItemDatabase = new Dictionary<string, ItemDetails>();
    private List<ItemDetails> m_PlayerInventory = new List<ItemDetails>();
    public static event OnInventoryChangedDelegate OnInventoryChanged = delegate { };
    public static GameController Instance { get; private set; }
    [SerializeField]
    private List<Texture2D> enviormentList;

    private enum enviormentStateEnum
    {
        Bridge,
        Hallway1,
        Hallway2,
        CoCaptain,
        Oxygenroom,
        OxygenroomWithEnergy
    }

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

    private void Start()
    {
        m_PlayerInventory.AddRange(m_ItemDatabase.Values);
        OnInventoryChanged?.Invoke(m_PlayerInventory.Select(x=> x.GUID).ToArray(), InventoryChangeType.Pickup);
    }

    public void EnviormentChange(int enviormentnumber)
    {
        Debug.Log(enviormentnumber);
        UIManagerControler.Instance.SetEnviorment(enviormentList[enviormentnumber]);
    }

    /// <summary>
    /// Populate the database
    /// </summary>
    public void PopulateDatabase()
    {
        m_ItemDatabase.Add("8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA", new ItemDetails()
        {
            Name = "OxygenPlant",
            GUID = "8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("prototyping_noteWallLeoRoommitCode")),
            CanDrop = false
        });

        m_ItemDatabase.Add("992D3386-B743-4CD3-9BB7-0234A057C265", new ItemDetails()
        {
            Name = "Health Potion",
            GUID = "992D3386-B743-4CD3-9BB7-0234A057C265",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion")),
            CanDrop = true
        });

        m_ItemDatabase.Add("1B9C6CAA-754E-412D-91BF-37F22C9A0E7B", new ItemDetails()
        {
            Name = "Bottle of Poison",
            GUID = "1B9C6CAA-754E-412D-91BF-37F22C9A0E7B",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("poison")),
            CanDrop = true
        });
        Debug.Log("datbase populated");
    }

    /// <summary>
    /// Retrieve item details based on the GUID
    /// </summary>
    /// <param name="guid">ID to look up</param>
    /// <returns>Item details</returns>
    public static ItemDetails GetItemByGuid(string guid)
    {
        if (m_ItemDatabase.ContainsKey(guid))
        {
            return m_ItemDatabase[guid];
        }
    
        return null;
    }
    public static void AddItemToDatabase(ItemDetails item)
    {
        if (!m_ItemDatabase.ContainsKey(item.GUID))
        {
            m_ItemDatabase.Add(item.GUID, item);
            Debug.Log("Item {item.Name} added to the database.");
            OnInventoryChanged?.Invoke(new string[] { item.GUID }, InventoryChangeType.Pickup);
            Debug.Log(new string[] { item.GUID });
        }
        else
        {
            Debug.Log("already exists");
        }
    }

}

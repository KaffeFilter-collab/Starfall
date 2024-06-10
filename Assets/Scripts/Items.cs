using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Items : MonoBehaviour
{
     OnInventoryChangedDelegate onInventoryChangedDelegate;
     private Vector3 currentpostion;
     [SerializeField] private GameController gameController;
     public List<Sprite> IconSprites;
     private void Awake()
     {
         currentpostion = transform.position;
     }

     private void OnMouseDown()
    {
        Debug.Log("yes");
        AddNewItem();
        StartCoroutine(GotoInventory());
    }
    void AddNewItem()
    {
        Debug.Log("added itemdetails");
        ItemDetails newItem = new ItemDetails()
        {
            Name = "Magic Wand",
            GUID = System.Guid.NewGuid().ToString(),
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("wand")),
            CanDrop = true
        };

        GameController.AddItemToDatabase(newItem);
    }


    IEnumerator GotoInventory()
    {
        while (transform.position != new Vector3(7.915752f,-4.330247f,-1))
        {
            transform.position = Vector3.LerpUnclamped(currentpostion,new Vector3(8.37f,-4.55f,-1),0.05f);;
            currentpostion = transform.position;
            yield return new WaitForSeconds(0.02f);
        }
       Destroy(this);
    }
}

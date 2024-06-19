using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class Items : MonoBehaviour
{
    [SerializeField];
    [SerializeField] private string itemName;
     private Vector3 currentpostion;
     [SerializeField] private GameController gameController;
     public Sprite icon;
     private bool clickable;
     private void Awake()
     {
         clickable = true;
         currentpostion = transform.position;
         icon = GetComponent<SpriteRenderer>().sprite;
     }

     private void OnMouseDown()
    {
        if (clickable)
        {
        AddNewItem(icon);
        StartCoroutine(GotoInventory());
        }
    }
    void AddNewItem(Sprite icon)
    {
        Debug.Log("added itemdetails");
        ItemDetails newItem = new ItemDetails()
        {
            Name = itemName,
            GUID = System.Guid.NewGuid().ToString(),
            Icon = icon,
            
        };
        Debug.Log(newItem.GUID + " GuidInItems");
        GameController.AddItemToDatabase(newItem);
    }


    IEnumerator GotoInventory()
    {
        clickable = false;
        while (transform.position.x <= 7.915752f && transform.position.y >= -4.330247f)
        {
            transform.position = Vector3.LerpUnclamped(currentpostion,new Vector3(8.37f,-4.55f,-1),0.05f);;
            currentpostion = transform.position;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }
}

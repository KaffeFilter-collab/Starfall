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
     private Vector3 currentpostion;
     [SerializeField] private GameController gameController;
     public List<Sprite> IconSprites;
     public Sprite icon;
     private void Awake()
     {
         currentpostion = transform.position;
         icon = GetComponent<SpriteRenderer>().sprite;
     }

     private void OnMouseDown()
    {
        AddNewItem(icon);
        StartCoroutine(GotoInventory());
    }
    void AddNewItem(Sprite icon)
    {
        Debug.Log("added itemdetails");
        ItemDetails newItem = new ItemDetails()
        {
            Name = "Magic Wand",
            GUID = System.Guid.NewGuid().ToString(),
            Icon = icon,
            CanDrop = true
        };
        Debug.Log(newItem.GUID + " GuidInItems");
        GameController.AddItemToDatabase(newItem);
    }


    IEnumerator GotoInventory()
    {
        while (transform.position.x <= 7.915752f && transform.position.y >= -4.330247f)
        {
            transform.position = Vector3.LerpUnclamped(currentpostion,new Vector3(8.37f,-4.55f,-1),0.05f);;
            currentpostion = transform.position;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using UnityEngine;


public class Items : MonoBehaviour
{

     private Vector3 currentpostion;

     private void Awake()
     {
         currentpostion = transform.position;
     }

     private void OnMouseDown()
    {
        Debug.Log("yes");
        StartCoroutine(GotoInventory());
        
    }

    IEnumerator GotoInventory()
    {
        while (transform.position != new Vector3(8.37f,-4.55f,-1))
        {
            transform.position = Vector3.LerpUnclamped(currentpostion,new Vector3(8.37f,-4.55f,-1),0.05f);;
            currentpostion = transform.position;
            yield return new WaitForSeconds(0.02f);
        }
    }
}

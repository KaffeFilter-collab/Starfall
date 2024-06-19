using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Objects/ItemHolder")]
public class ItemHolder : ScriptableObject
{
    [SerializeField] private GameObject[] _itemsArray;
}
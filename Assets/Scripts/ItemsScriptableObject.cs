using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Objects/ItemsScriptableObject")]
public class ItemsScriptableObject : ScriptableObject
{
    [SerializeField] private ChoiceDialogueNode _dialogue;
    [SerializeField] private ItemDetails _guidDetails;
}
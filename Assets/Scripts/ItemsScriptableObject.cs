using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Objects/ItemsScriptableObject")]
public class ItemsScriptableObject : ScriptableObject
{
    [SerializeField] private ChoiceDialogueNode _dialogueNode;
    [SerializeField] private ItemDetails _itemDetails;
}
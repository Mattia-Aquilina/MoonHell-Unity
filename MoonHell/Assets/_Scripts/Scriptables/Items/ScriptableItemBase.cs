using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che raccoglie i parametri comuni a tutti gli item
/// </summary>
[CreateAssetMenu(fileName = "item", menuName ="Scriptable/item")]
public class ScriptableItemBase : ScriptableObject 
{
    //per i menu
    [SerializeField] private string _descrizione;
    [SerializeField] private string _nome;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Sprite _menuIcon;
    public string Descrizione => _descrizione;
    public Sprite MenuIcon => _menuIcon;
    public string Nome => _nome;
    public ItemType ItemType => _itemType;

    // override object.Equals
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che tiene traccia dell'inventario del player e genera eventi che permettono di ricalcolare le statistiche ogni qualvolta viene modifcato l'equipaggiamento
/// </summary>
public class InventoryManager : StaticInstance<InventoryManager>
{
    /// <summary>
    /// Item list in inventory
    /// </summary>
    [SerializeField] public List<ScriptableItemBase> ItemList { get; private set; }
    /// <summary>
    /// Evento che viene generato ogni volta che l'inventario viene modificato, utile alla classe player per attivare/disattivare le classi 
    /// corrette
    /// </summary>
    public event Action InventoryChanged;
    /// <summary>
    /// Lista degli artefatti correntemente equipaggiati
    /// </summary>
    [SerializeField] public List<ScriptableArtifact> ArtifactList { get; private set; }

    public List<SpiritSchool> ActiveSchools { get; private set; } = new();

    public const int MaxInventorySize = 10;
    public const int MaxEquipmentSlot = 6;


    void Awake()
    {
        base.Awake();
        ItemList = new();
        ArtifactList = new();
    }
    /// <summary>
    /// Aggiunge un'oggetto all'inventario. Ritorna una FullInventoryException in caso di inventario pieno
    /// </summary>
    /// <param name="item">Oggetto da aggiungere all'inventario</param>
    public void AddItemInInventory(ScriptableItemBase item) 
    {
        if (ItemList.Count == MaxInventorySize) throw new FullInventoryException();
        ItemList.Add(item); 
    }

    /// <summary>
    /// Rimuove un'oggetto dall'inventario 
    /// </summary>
    /// <param name="item">Oggetto da rimuovere</param>
    public void RemoveItemFormInventory(ScriptableItemBase item)
    {
        if (ItemList.Contains(item)) ItemList.Remove(item);

    }

    /// <summary>
    /// Equipaggia un'oggetto nell lista degli artefatti.
    /// </summary>
    /// <param name="artifact">Artifatto da equipaggiare</param>
    public void EquipItem(ScriptableArtifact artifact)
    {
        if (ArtifactList.Count == MaxEquipmentSlot) throw new FullInventoryException();

        ArtifactList.Add(artifact);
        ItemList.Remove(artifact);


        Debug.Log("Item equipped, Current equip size: " + ArtifactList.Count + " Current inv size: " + ItemList.Count);

        foreach (SpiritSchool type in artifact.spiritTypes())
            if (!ActiveSchools.Contains(type))
                ActiveSchools.Add(type);

        InventoryChanged?.Invoke();
    }
    
    /// <summary>
    /// Disequipaggia l'artefatto passato in input. In caso di inventario pieno, e quindi di impossibilità di posizionare l'oggetto nella lista di oggetti,
    /// ritorna una FullInventoryException
    /// </summary>
    /// <param name="artifact"></param>
    public void UnEquipItem(ScriptableArtifact artifact)
    {
        if (ArtifactList.Contains(artifact) && ItemList.Count <= MaxInventorySize)
        {
            ArtifactList.Remove(artifact);
            ItemList.Add(artifact);


            Debug.Log("Item unequipped, Current equip size: " + ArtifactList.Count + " Current inv size: " + ItemList.Count);
        }
        else
            throw new FullInventoryException();

        foreach (SpiritSchool type in artifact.spiritTypes())
            if (ActiveSchools.Contains(type))
                ActiveSchools.Remove(type);

        InventoryChanged?.Invoke();
    }
}

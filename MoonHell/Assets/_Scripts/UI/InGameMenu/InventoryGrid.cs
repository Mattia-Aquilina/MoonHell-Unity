using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    /// <summary>
    /// Prefab dell'elemento draggabile del menu
    /// </summary>
    [SerializeField] private InventorySlot elemPrefab;
    private InventorySlot[] InventorySlots = new InventorySlot[InventoryManager.MaxInventorySize];

    /// <summary>
    /// La lista degli ultimi item visualizzati nel menu
    /// </summary>
    private List<ScriptableItemBase> _LastItemInInventory;

    /// <summary>
    /// Il metodo awake instanzia tutti gli elementi grafici associati al'inventario
    /// </summary>
    private void Awake()
    {
        _LastItemInInventory = InventoryManager.Instance.ItemList;
        for (int i = 0; i < InventoryManager.MaxInventorySize; i++)
        {
            InventorySlots[i] = Instantiate(elemPrefab, this.transform);
        }
    }
    private void OnEnable()
    {
        List<ScriptableItemBase> _HeroItemList = InventoryManager.Instance.ItemList;

        foreach (var item in _HeroItemList)
        {

        }
    }

    public void OnAddItem(ScriptableItemBase item)
    {

    }
}

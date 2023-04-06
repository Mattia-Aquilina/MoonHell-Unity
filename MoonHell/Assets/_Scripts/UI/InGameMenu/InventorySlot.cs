using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    [SerializeField] private DraggableItem _DraggableItem;
    public DraggableItem GetDraggableItem() => _DraggableItem;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped! (On Inventory)");
        //Otteniamo l'oggetto droppato sul cella di inventario corrente
        var d = eventData.pointerDrag;
        DraggableItem droppedItem = d.GetComponent<DraggableItem>();
        InventorySlot exParent = droppedItem.originalParent;

        if (exParent is EquipSlot) Debug.Log("Equip Has Changed!");

        //Settiamo i nuovi valori per l'oggetto precendentemente contenuto nella cella attuale
        exParent.SetDraggableItem(_DraggableItem);
        _DraggableItem.transform.SetParent(exParent.transform);

        //Settiamo i nuovi valori per l'oggetto droppato
        _DraggableItem = droppedItem;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.originalParent = this;
    }
    public void SetDraggableItem(DraggableItem dr) {
        this._DraggableItem = dr;
        //dr.originalParent = this;
    }
}

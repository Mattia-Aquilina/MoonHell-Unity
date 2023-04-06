using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : InventorySlot, IDropHandler
{
    public new void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Equip Has Changed");
        var elem = eventData.pointerDrag;
        DraggableItem draggableItem = elem.GetComponent<DraggableItem>();
        DraggableItem item = draggableItem;

        if (!item._isArtifact) return;

        base.OnDrop(eventData);
        //Lanciamo l'evento per OnInventoryAdd();
    }
}

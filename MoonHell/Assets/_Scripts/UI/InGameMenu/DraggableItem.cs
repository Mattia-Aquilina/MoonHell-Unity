using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Elemento dell'inventario trascinabile 
/// </summary>
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool _isArtifact;
    public ScriptableItemBase _itemData;
    public Image image;
    public InventorySlot originalParent;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (image.sprite == null) return;

        Debug.Log("Start Drag");
        originalParent = GetComponentInParent<InventorySlot>();
        transform.SetParent(transform.parent.parent.parent.parent);///Otteniamo il transform del container dell'inventario
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (image.sprite == null) return;

        Debug.Log("Dragging");
        Debug.Log("End Dragging");
        transform.SetParent(originalParent.transform);
        image.raycastTarget = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (image.sprite == null) return;

        transform.position = new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class MouseManipulator : PointerManipulator
{
    bool isArtifact = false;
    InventoryUiManger root;
    ScriptableItemBase itemData;
    int ButtonIndex;
    public MouseManipulator(VisualElement target, ScriptableItemBase itemData, InventoryUiManger root, int ButtonIndex)
    {
        this.target = target;
        this.itemData = itemData;
        this.root = root;
        this.ButtonIndex = ButtonIndex;

        if (itemData is ScriptableArtifact) isArtifact = true;
    }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
  
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        
    }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        Debug.Log("Clicked " + itemData.Nome);
        root.DisplayItem(itemData);

    }

    public void SetItemData(ScriptableItemBase item)
    {
        itemData = item;
    }
}

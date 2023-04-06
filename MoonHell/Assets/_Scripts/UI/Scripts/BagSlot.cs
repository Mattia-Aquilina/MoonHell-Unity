using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class BagSlot : VisualElement
{
    private VisualTreeAsset ui;
    public int Id { get; private set; }

    //can be null
    public ScriptableItemBase Item { get; private set; }
    public BagSlot(int id, InventoryUiManger managerInstance, ScriptableItemBase item = null)
    {
        //Instanziamo la grafica

        ui = Resources.Load<VisualTreeAsset>("UI/SlotContainer");

        ui.CloneTree(this);

        if (item)
        {
            this.AddManipulator(new MouseManipulator(this, item, managerInstance, id));
            this.Q("itemImage").style.backgroundImage = new(item.MenuIcon);
        }
    }

}

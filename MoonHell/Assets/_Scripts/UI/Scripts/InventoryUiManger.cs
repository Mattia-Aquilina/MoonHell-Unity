using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUiManger : VisualElement
{
    //Gestisce la visualizzazione dell'intero inventario
    VisualElement ItemDataPage;
    VisualElement BagContainer;
    VisualElement EquipContainer;

    //Lista degli slot della borsa
    List<BagSlot> BagSlots = new();
    List<BagSlot> EquipSlots = new();
    //Item that is getting previed
    ScriptableArtifact currentItemInPreview;
    int ItemInPreview;
    public new class UxmlFactory : UxmlFactory<InventoryUiManger, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public InventoryUiManger()
    {
        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        
    }

    public void OnGeometryChanged(GeometryChangedEvent evt)
    {
        ItemDataPage = this.Q("itemPreview");
        BagContainer = this.Q("BagContainer");
        EquipContainer = this.Q("EquipContainer");

        CreateInventory();
        CreateBag();
    }

    private void CreateInventory()
    {
        RemoveAllEquipSlots();

        int i = 1;
        foreach (ScriptableItemBase item in InventoryManager.Instance.ArtifactList)
        {
            BagSlot element = new BagSlot(i, this, item);
            element.style.width = Length.Percent(100);
            element.style.height = Length.Percent(100);

            EquipContainer.Add(element);
            EquipSlots.Add(element);
            i++;
        }


        for (i = i; i <= InventoryManager.MaxEquipmentSlot; i++)
        {
            BagSlot element = new BagSlot(i, this);
            element.style.width = Length.Percent(100);
            element.style.height = Length.Percent(100);

            EquipContainer.Add(element);
            EquipSlots.Add(element);
        }
    }

    private void CreateBag()
    {
        RemoveAllBagSlots();
        //instanziamo 10 slot di borsa
        int i = 1;
        foreach(ScriptableItemBase item in InventoryManager.Instance.ItemList)
        {
            int row;
            if (i <= 5)
                row = 1;
            else
                row = 2;

            BagSlot element = new BagSlot(i, this, item);
            element.style.width = Length.Percent(100);
            element.style.height = Length.Percent(100);

            BagContainer.Q("row-" + row).Add(element);
            BagSlots.Add(element);
            i++;
        }


        for (i=i; i<= InventoryManager.MaxInventorySize; i++)
        {
            int row;
            if (i <= 5)
                row = 1;
            else
                row = 2;

            BagSlot element = new BagSlot(i,this);
            element.style.width = Length.Percent(100);
            element.style.height = Length.Percent(100);

            BagContainer.Q("row-" + row).Add(element);
            BagSlots.Add(element);
        }
    }

    public void DisplayItem(ScriptableItemBase item)
    {
        VisualElement sprite = ItemDataPage.Q("ItemImagePreview");
        VisualElement stats = ItemDataPage.Q("ItemStatsPreview");
        VisualElement description = ItemDataPage.Q("ItemDescriptionPreview");
        ItemDataPage.Remove(ItemDataPage.Q<Button>("equip"));
        //set the image
        sprite.style.backgroundImage = new(item.MenuIcon);

        if (item is ScriptableArtifact) {
            //set the stats
            ScriptableArtifact artifact = (ScriptableArtifact)item;
            stats.Q<Label>("PRO").text = "PRO: " + artifact.HeroStatsEmp().proiezione;
            stats.Q<Label>("MAT").text = "MAT: " + artifact.HeroStatsEmp().materializzazione;
            stats.Q<Label>("MAN").text = "MAN: " + artifact.HeroStatsEmp().manipolazione;
            stats.Q<Label>("MAF").text = "MAF: " + artifact.HeroStatsEmp().manifestazione;
            stats.Q<Label>("ATS").text = "ATS: " + artifact.HeroStatsEmp().ats;
            stats.Q<Label>("CDR").text = "CDR: " + artifact.HeroStatsEmp().cdr;
            stats.Q<Label>("HP").text = "HP: " + artifact.BaseStatsEmp().hp;
            stats.Q<Label>("MOV").text = "MOV: " + artifact.BaseStatsEmp().ms;

            Button b = new Button();
            b.name = "equip";
            ItemDataPage.Add(b);


            if (InventoryManager.Instance.ArtifactList.Contains(artifact))
            {
                
                ItemDataPage.Q<Button>("equip").text = "Unequip";
                ItemDataPage.Q<Button>("equip").UnregisterCallback<ClickEvent>(ev => HandleButtonClickEquip(item));
                ItemDataPage.Q<Button>("equip").RegisterCallback<ClickEvent>(ev => HandleButtonClickUnequip(item));
                currentItemInPreview = artifact;
            }else
            {
                ItemDataPage.Q<Button>("equip").text = "equip";
                ItemDataPage.Q<Button>("equip").UnregisterCallback<ClickEvent>(ev => HandleButtonClickUnequip(item));
                ItemDataPage.Q<Button>("equip").RegisterCallback<ClickEvent>(ev => HandleButtonClickEquip(item));
                currentItemInPreview = artifact;
            }
                
        }

        description.Q<Label>("ItemDescription").text = item.Descrizione;

        ItemDataPage.style.display = DisplayStyle.Flex;
    }

    public void HandleButtonClickUnequip(ScriptableItemBase item)
    {

        InventoryManager.Instance.UnEquipItem(currentItemInPreview);
        CreateInventory();
        CreateBag();
        DisplayItem(item);
    }

    public void HandleButtonClickEquip(ScriptableItemBase item)
    {
    
        InventoryManager.Instance.EquipItem(currentItemInPreview);
        CreateInventory();
        CreateBag();
        DisplayItem(item);
    }
    private void RemoveAllEquipSlots()
    {
        foreach (BagSlot slot in EquipSlots)
        {
            EquipContainer.Remove(slot);
        }
        EquipSlots = new();
    }
    private void RemoveAllBagSlots()
    {
        foreach(BagSlot slot in BagSlots)
        {
            if(BagContainer.Q("row-1").Contains(slot)) BagContainer.Q("row-1").Remove(slot);
            else BagContainer.Q("row-2").Remove(slot);
        }
        BagSlots = new();
    }

}

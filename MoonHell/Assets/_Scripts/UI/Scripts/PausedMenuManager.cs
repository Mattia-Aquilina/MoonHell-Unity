using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PausedMenuManager : VisualElement
{
    VisualElement topBar;
    VisualElement inventoryTab;
    VisualElement statsTab;
    VisualElement spritiSchoolTab;
    VisualElement optionsTab;

    public new class UxmlFactory : UxmlFactory<PausedMenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public PausedMenuManager()
    {
        //eventuali reference a script esterni 
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    public void OnGeometryChanged(GeometryChangedEvent evt)
    {
        topBar = this.Q("TopBar");
        inventoryTab = this.Q("Inventory");
        statsTab = this.Q("Stats");
        optionsTab = this.Q("Options");
        spritiSchoolTab = this.Q("Schools");


        topBar?.Q("options")?.RegisterCallback<ClickEvent>(ev => OpenOptionTab());
        topBar?.Q("schools")?.RegisterCallback<ClickEvent>(ev => OpenSchoolsTab());
        topBar?.Q("inventory")?.RegisterCallback<ClickEvent>(ev => OpenInventoryTab());
        topBar?.Q("stats")?.RegisterCallback<ClickEvent>(ev => OpenStatsTab());

        //inizializzazione della prima schermata
        DisplayStats();
    }
    public void DisableAllUi()
    {
        inventoryTab.style.display = DisplayStyle.None;
        statsTab.style.display = DisplayStyle.None;
        optionsTab.style.display = DisplayStyle.None;
        spritiSchoolTab.style.display = DisplayStyle.None;
    }
    public void OpenOptionTab()
    {
        DisableAllUi();
        optionsTab.style.display = DisplayStyle.Flex;
    }
    public void OpenSchoolsTab()
    {
        DisableAllUi();
        spritiSchoolTab.style.display = DisplayStyle.Flex;
    }
    public void OpenInventoryTab()
    {
        DisableAllUi();
        inventoryTab.style.display = DisplayStyle.Flex;
    }
    public void OpenStatsTab()
    {
        DisableAllUi();
        statsTab.style.display = DisplayStyle.Flex;

        //Get stats and display them
        DisplayStats();
    }

    private void DisplayStats()
    {
        this.Q<Label>("HP").text = "HP:" + GameManager.Instance.HeroInstance.Stats.hp.ToString() + "/" + GameManager.Instance.HeroInstance.Stats.MaxHP.ToString();
        this.Q<Label>("MOV").text = "MOV:" + GameManager.Instance.HeroInstance.Stats.ms.ToString();
        this.Q<Label>("ATS").text = "ATS:" + GameManager.Instance.HeroInstance.HeroStats.ats.ToString();
        this.Q<Label>("CDR").text = "CDR:" + GameManager.Instance.HeroInstance.HeroStats.cdr.ToString();
        this.Q<Label>("MAT").text = "MAT:" + GameManager.Instance.HeroInstance.HeroStats.materializzazione.ToString();
        this.Q<Label>("PRO").text = "PRO:" + GameManager.Instance.HeroInstance.HeroStats.proiezione.ToString();
        this.Q<Label>("MAF").text = "MAF:" + GameManager.Instance.HeroInstance.HeroStats.manifestazione.ToString();
        this.Q<Label>("MAN").text = "MAN:" + GameManager.Instance.HeroInstance.HeroStats.manipolazione.ToString();
    }
}
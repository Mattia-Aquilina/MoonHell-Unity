using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Classe che gestisce la UI in game, attivando/disattivando il menu di pausa all'occorrenza
/// </summary>
public class MenuManager : VisualElement
{

    //Attributi
    
    VisualElement gameUI;
    VisualElement menuContainer;
    public new class UxmlFactory : UxmlFactory<MenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public MenuManager()
    {
        //eventuali reference a script esterni 
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        if (InventoryManager.Instance != null) InventoryManager.Instance.InventoryChanged += OnInventoryChanged;
    }

    public void OnGeometryChanged(GeometryChangedEvent evt)
    {
        //evento lanciato ogni volta che viene premuto un bottone
        
        gameUI = this.Q("GameUi");
        menuContainer = this.Q("PausedMenuContainer");

        //Controlliamo quale bottone ha generato l'evento
        
        gameUI?.Q("PauseButton")?.RegisterCallback<ClickEvent>(ev => PausedGameUi());
        menuContainer?.Q("UnPauseButton")?.RegisterCallback<ClickEvent>(ev => UnPausedGameUi());

        this.Q("Dash")?.RegisterCallback<ClickEvent>(ev => DashButton());
        gameUI?.Q("Attack")?.RegisterCallback<ClickEvent>(ev => GameManager.Instance.HeroInstance._schoolSpiritList[SpiritSchool.fury].Effect());
    }

    public void DashButton()
    {
        Debug.Log("dash");
        GameManager.Instance.HeroInstance._schoolSpiritList[SpiritSchool.shadows].Effect();
    }
    public void OnInventoryChanged()
    {
        if (InventoryManager.Instance.ActiveSchools.Contains(SpiritSchool.shadows))
            this.Q<Button>("Dash").style.display = DisplayStyle.Flex;
        else
            this.Q<Button>("Dash").style.display = DisplayStyle.None;
    }
    //funzioni utili
    public void DisableAllUI()
    {
        //usiamo questo metodo per ren
        gameUI.style.display = DisplayStyle.None;
        menuContainer.style.display = DisplayStyle.None;
    }

    public void PausedGameUi()
    {
        //Chiediamo al GameManager di mettere in pausa il gioco
        DisableAllUI();
        menuContainer.style.display = DisplayStyle.Flex;
        GameManager.Instance.PauseGame();
    }

    public void UnPausedGameUi()
    {
        //Chiediamo al GameManager di riavviare il gioco
        DisableAllUI();
        gameUI.style.display = DisplayStyle.Flex;

        GameManager.Instance.UnPauseGame();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class MainMenuManager : VisualElement
{
    DataHolder mainMenuDataHolder;
    public new class UxmlFactory : UxmlFactory<MainMenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public MainMenuManager()
    {
        //eventuali reference a script esterni
        mainMenuDataHolder = DataHolder.FindObjectOfType<DataHolder>();

        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    public void OnGeometryChanged(GeometryChangedEvent evt)
    {
        this.Q("NewGame")?.RegisterCallback<ClickEvent>(ev => NewGame());
        this.Q("Continue")?.RegisterCallback<ClickEvent>(ev => Continue());
        this.Q("Settings")?.RegisterCallback<ClickEvent>(ev => Settings());
        this.Q("Quit")?.RegisterCallback<ClickEvent>(ev => Quit());
    }

    private void NewGame() {
        SceneManager.LoadScene(mainMenuDataHolder.GetGameScene());
    }
    private void Continue() { 
    
    }
    private void Settings() { 
    
    }
    private void Quit() { 
    
    }
}

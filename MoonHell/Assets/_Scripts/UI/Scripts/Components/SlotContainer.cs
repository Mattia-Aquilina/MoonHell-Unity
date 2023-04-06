using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Classe rappresentante uno slot dell'inventario
/// </summary>
public class SlotContainer : VisualElement
{
    public SlotContainer()
    {
        //nel costruttore importiamo la struttura dello slot da uxml
        //Assets / _Scripts / UI / uxml / SlotContainer.uxml
        //        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/MyWindow.uxml");
        //VisualElement ui = uiAsset.Instantiate();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    /// <summary>
    /// Lista dei bottoni nel menu a pagine
    /// </summary>
    [SerializeField]private List<GameObject> _TabsButton;
    /// <summary>
    /// Lista delle pagine del menu. IMPORTANTE, I BOTTONI ASSOCIATI AD UNA PAGINA DEVONO ESSERE NELLA POSIZIONE 
    /// RELATIVA ALLA PAGINA STESSA ALL'INTERNO DELLA LISTA
    /// </summary>
    [SerializeField]private List<GameObject> _TabsPages;
    private Dictionary<GameObject, GameObject> _Tabs;
    [SerializeField] GameObject _StartingPage;
     
    [SerializeField]private Color _UnSelectColor;
    [SerializeField]private Color _SelectColor;
    GameObject CurrentPage;
    public Dictionary<GameObject, GameObject> Tabs => _Tabs;

    public void OnEnable()
    {
        //Inizziliziamo la GUI
        CurrentPage.SetActive(true);
        
    }

    /// <summary>
    /// Inizializziamo la visualizzazione della pagine
    /// </summary>
    void Awake()
    {
        CurrentPage = _StartingPage;

        _Tabs = _TabsButton.Zip(_TabsPages, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);

        foreach(var k in _Tabs.Keys)
        {
            var _currentPage = _Tabs[k];

            if (_currentPage.Equals(CurrentPage)) k.GetComponent<Image>().color = _SelectColor;
            else
            {
                _currentPage.SetActive(false);
                k.GetComponent<Image>().color = _UnSelectColor;
            }
          }    
    }
    public void ChangeTabs(GameObject Source)
    {
        //Disattiviamo La pagina corrente
        CurrentPage.SetActive(false);
        CurrentPage = _Tabs[Source];
        CurrentPage.SetActive(true);

        foreach (var tabButton in _Tabs.Keys)
        {
            var img = Source.GetComponent<Image>();
            img.color = _UnSelectColor;
        }

        Source.GetComponent<Image>().color = _SelectColor;
    }
}

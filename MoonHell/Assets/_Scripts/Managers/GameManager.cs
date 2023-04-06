using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game manger basato su enumeration
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    /// Variabili di lavoro del gioco
    public HeroBase HeroInstance { get; private set; }
    //Instanza del controller

    [SerializeField] private HeroBase prefab;

    //public static readonly List<string> _StatsNames = new List<string> { "PRO", "MAT", "MNP", "MNF", "ATS", "CDR", "HP", "EXP" };
    /// <summary>
    /// Costante k usata nel calcolo degli exp richiesti
    /// </summary>
    public static float kConstLevels = 1;
    /// <summary>
    /// Costante h usata nel calcolo degli exp richiesti
    /// </summary>
    public static float hConstLevels = 1;


    [SerializeField]private List<GameObject> _ManagersList;
    /// <summary>
    /// Exp di base richiesti per salire di livello, utilizzato nel calcolo degli exp richiesti
    /// </summary>
    public static float baseExp = 200;
    //fine variabili 
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; } = GameState.Preparation;

    // Kick the game off with the first state
    void Start() => ChangeState(GameState.AssemblingResources);

    public bool isPaused;
    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.StartUp:
                //Istruzioni da eseguire all'avvio del gioco

            case GameState.AssemblingResources:
                //Stato separato per inserire un eventuale barra di caricamento
                ResourceSystem.Instance.AssembleResources();
                break;
            case GameState.Menu:

                break;
            case GameState.Preparation:
                CreateManagers();
                PreparePlayer();
                break;
            case GameState.PreHunting:
                break;
            case GameState.Hunting:

            case GameState.Jump:
                break;
            default:
                break;
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }

    /// <summary>
    /// Metodo che prepara il player, imposta le statistiche e inizializza l'inventario
    /// </summary>
    private void PreparePlayer() {
        InventoryManager.Instance.AddItemInInventory(ResourceSystem.Instance.GetArtifact(ItemType.Dagger));
        InventoryManager.Instance.AddItemInInventory(ResourceSystem.Instance.GetArtifact(ItemType.WoodenBow));
        InventoryManager.Instance.AddItemInInventory(ResourceSystem.Instance.GetArtifact(ItemType.WoodenShield));
        HeroInstance = Instantiate(prefab, new Vector3(0,0,0) , Quaternion.Euler(0,0,0));
        if (HeroInstance == null) Debug.Log("hero is null");
        
        CameraBehaviour.Instance.SetPlayer(HeroInstance);

        ChangeState(GameState.Hunting);
    }

    /// <summary>
    /// Metodo che instanzia tutti i manager necessari al funzionamento del gioco
    /// </summary>
    private void CreateManagers()
    {
        foreach(GameObject m in _ManagersList)
        {
            Instantiate(m, transform);
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        Joystick.Instance.gameObject.SetActive(false);
    }

    public void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        Joystick.Instance.gameObject.SetActive(true);
    }
}

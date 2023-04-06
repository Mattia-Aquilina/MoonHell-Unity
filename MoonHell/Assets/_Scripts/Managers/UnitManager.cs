using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : StaticInstance<UnitManager>
{
    // Start is called before the first frame update
    private IEnumerator CurrentCoroutine;
    void Awake()
    {
        base.Awake();
        GameManager.OnAfterStateChanged += OnStateChange;

    }

    private void OnStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                break;
            case GameState.Preparation:
                break;
            case GameState.PreHunting:
                break;
            case GameState.Jump:
                break;
            case GameState.Hunting:
                //manage hunting phase
                CurrentCoroutine = HandleHuntingPhase();
                break;
            case GameState.AssemblingResources:
                break;
            case GameState.StartUp:
                break;
            default:
                break;
        }

        StartCoroutine(CurrentCoroutine);
    }

    private IEnumerator HandleHuntingPhase()
    {
        yield return null;
    }
}

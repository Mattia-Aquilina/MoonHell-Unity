using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    [SerializeField] private string GameScenePath;
    // Start is called before the first frame update
    public string GetGameScene() => GameScenePath;
}

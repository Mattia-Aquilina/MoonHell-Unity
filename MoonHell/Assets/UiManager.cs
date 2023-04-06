using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _MenuPanel;
    [SerializeField] private Canvas _UiContainer;
    private void Awake()
    {
        Instantiate(_MenuPanel, _UiContainer.transform);
    }
}

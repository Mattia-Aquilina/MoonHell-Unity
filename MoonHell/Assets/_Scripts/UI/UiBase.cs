using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe Comune a tutte le Ui
/// </summary>
public class UiBase : MonoBehaviour
{
    [SerializeField] protected RectOffset Padding;
    [SerializeField] public RectTransform _rectTransform { get; private set; }
    [SerializeField] [Range(0, 1)] protected float widthK;
    [SerializeField] [Range(0, 1)] protected float heightK;
    protected float _screenWidth;
    protected float _screenHeight;
    protected void Awake()
    {
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;

        _rectTransform = GetComponent<RectTransform>();
    }
}

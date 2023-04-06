using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUiMain : UiBase
{
    private new void Awake()
    {
        base.Awake();

        Vector2 size = new Vector2(_screenWidth * widthK,_screenHeight * heightK);
        _rectTransform.sizeDelta = size;
    }

    private void Update()
    {
        Vector2 size = new Vector2(_screenWidth * widthK, _screenHeight * heightK);
        _rectTransform.sizeDelta = size;
    }
}

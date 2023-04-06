using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestedUi : UiBase
{
    private UiBase parent;
    public float _parentWidth;
    public   float _parentHeight;
    private new void Awake()
    {
        base.Awake();
        parent = GetComponentInParent<UiBase>();
        _parentWidth = parent._rectTransform.rect.width;
        _parentHeight = parent._rectTransform.rect.height;

        Vector2 size = new Vector2(_parentWidth * widthK, _parentHeight * heightK);
        _rectTransform.sizeDelta = size;
    }

    private void Update()
    {
        Vector2 size = new Vector2(_screenWidth * widthK, _screenHeight * heightK);
        _rectTransform.sizeDelta = size;
    }
}

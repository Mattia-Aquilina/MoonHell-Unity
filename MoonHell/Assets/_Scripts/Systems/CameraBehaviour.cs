using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : StaticInstance<CameraBehaviour>
{
    // Start is called before the first frame update
    [SerializeField] private Camera _camera;
    [SerializeField] public HeroBase _hero;
    [SerializeField] private Vector3 cameraDistance;
    [SerializeField] [Range(0, 1)] private float k;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.State != GameState.Hunting) return;

        Vector3 desiredPosition = _hero.transform.position + cameraDistance;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredPosition, k);
    }

    public void SetPlayer(HeroBase hero) => _hero = hero;
}

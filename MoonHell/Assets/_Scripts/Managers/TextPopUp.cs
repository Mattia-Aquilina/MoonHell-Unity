using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    [Header("Text Component")]
    [SerializeField] private TextMeshPro text;

    //variaibili di lavoro
    bool init = false;
    [SerializeField] float timeToLive;
    // Start is called before the first frame update
    private void Update()
    {
        if (!init) return;

        this.transform.position += new Vector3(0, 0.1f * Time.deltaTime, 0.1f * Time.deltaTime); 
    }

    public void Init(Color textColor, Transform transform, int damage)
    {
        this.transform.position = new Vector3(transform.position.x , 0.5f,transform.position.z);
        text.color = textColor;
        text.text = damage.ToString();
        init = true;
        Invoke(nameof(Die), timeToLive);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

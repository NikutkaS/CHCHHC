using UnityEngine;

public class EchoAction : MonoBehaviour
{
    private void Start()
    {
        // сделать эхо полупрозрачным автоматически (если есть SpriteRenderer)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }
    }
}

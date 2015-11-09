using UnityEngine;
using System.Collections;

public class TextureShifter : MonoBehaviour {

    public Renderer rend;
    public float xOffset;
    public float yOffset;
    float offset;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        backgroundScroller();
    }

    void backgroundScroller()
    {
        offset = (offset + Time.deltaTime * 0.1f) % 1;
        rend.material.SetTextureOffset("_MainTex", new Vector2(xOffset* offset, yOffset* offset));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public CardProperties.Suit FirstSuit;
    public CardProperties.Suit SecondSuit;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void ChangeLayer(int layer)
    {
        _spriteRenderer.sortingOrder = layer;
    }

}

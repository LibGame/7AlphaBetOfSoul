using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLine : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _spriteRenderer;
    private Transform _selectCard;
    public Transform SelectCard { get { return _selectCard; } }

    public void SetSprite(Sprite[] sprite , Sprite spriteMain , int rnd)
    {
        for (int i = 0; i < _spriteRenderer.Length; i++)
        {
            _spriteRenderer[i].sprite = sprite[i];
            if(spriteMain == sprite[i])
            {
                _selectCard = _spriteRenderer[i].gameObject.transform;
            }
        }
    }

}

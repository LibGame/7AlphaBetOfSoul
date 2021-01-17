using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitOnTable : MonoBehaviour
{

    /// <summary>
    /// Класс магнита на столе изображенного мастью 
    /// </summary>

    public Vector2 SizeMaxSuitPoint { get; private set; } // Ширина и высота точки магнита на доске
    public Vector2 SizeMinSuitPoint { get; private set; } // Ширина и высота точки магнита на доске
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Arrow[] _arrows; // стрелки которые укажут на схожие масти
    public CardProperties.Suit SuitTable;
    [SerializeField] private Color _ownCollor; // цвет в который будет окрашиваться стрелка
    public Color OwnColor { get { return _ownCollor; } }
    public int AmountCardLay { set; get; } // Количество карт на масти 
    [SerializeField] private Sprite _spriteColorfull;
    [SerializeField] private Sprite _spriteBase;
    private Vector3 _baseSize = new Vector3(1.142425f, 1.07114f,1);
    private Vector3 _colorFullSize = new Vector3(0.3f, 0.3f, 1);

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SizeMaxSuitPoint = new Vector2(_spriteRenderer.bounds.max.x, _spriteRenderer.bounds.max.y);
        SizeMinSuitPoint = new Vector2(_spriteRenderer.bounds.min.x, _spriteRenderer.bounds.min.y);
    }

    /// <summary>
    /// Указывает желтым цветом через стрелки на схожие масти
    /// К этому классу обратится карта которая ляжет на эту масть
    /// И меняю слой стрелки которая показывает на масть
    /// </summary>
    public Color ShowSameSuits()
    {
        for(int i = 0; i < _arrows.Length; i++)
        {
            _arrows[i].ChangeColor(_ownCollor);
            _arrows[i].ChangeLayer(2);
        }
        return _ownCollor;

    }

    public void ChangeSpriteColorfull()
    {
        _spriteRenderer.sprite = _spriteColorfull;
        transform.localScale = _colorFullSize;
    }

    public void ChangeSpriteBase()
    {
        _spriteRenderer.sprite = _spriteBase;
        transform.localScale = _baseSize;
    }
}

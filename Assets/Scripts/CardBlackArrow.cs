using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBlackArrow : CardProperties
{

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startPosition = transform.position;
        GameManager = FindObjectOfType<GameManager>();
    }

    protected override void OnMouseDown()
    {
        if (GameManager.IsCanTake && !_isLay)
        {
            var card = Instantiate(gameObject, _startPosition, Quaternion.identity);
            GameManager.BlackCardList[IDCard] = card; // заменяю карту в колоде на нову созданную карту
            card.GetComponent<CardProperties>().IDCard = IDCard; // передаю айди этой карты в новую созданную
            _spriteRenderer.sortingOrder = 2; // Делаю карту на слой выше карты в колоде
            card.transform.SetParent(GameManager.BlackArrowsParent.transform);
            base.OnMouseDown();
        }
    }
    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
    }

    protected override void OnMouseUp()
    {
        if (!_isLay && GameManager.IsCanTake)
        {
            CheckToCollisionSuit();
        }
    }

    public override void ChangeColorSuits()
    {
        GameManager.CangeAllArrow(1, Color.white); // меняем всем стрелкам слой отображения и цвет 
        GameManager.BackToColorfullInconSuit();
    }

    /// <summary>
    /// Проверяет на какую масть положена карта
    /// пробегаеться по массиву мастей на столе и если есть пересечение то прерывает цикл и помещяет карту на места масти
    /// если нету совпадений в конце цикла то ставит карту на бозвое место карты в колоде
    /// </summary>
    public override void CheckToCollisionSuit()
    {
        base.CheckToCollisionSuit();
    }

    /// <summary>
    /// Удоляет карту
    /// </summary>
    public override void LayCardOnStartPosition()
    {
        base.LayCardOnStartPosition();
    }

    /// <summary>
    /// Ложит карту на кординаты масти
    /// кординаты масти берет из массива масте по индексу переданному в параметре
    /// Меняю слои всех стрелок которые показывают на масть на 1
    /// </summary>
    /// <param name="suitID"></param>
    public override void LayCardOnTable()
    {
        base.LayCardOnTable();
    }


    /// <summary>
    /// Метод проверяет соостветствует ли левая масть карты  если да то вызывает метод LayCardOntable 
    /// если не сооствествует то вызывает метод LayCardOnStartPosition
    /// В аргумент принимаеть ID иконки масти на столе
    /// </summary>
    /// <param name="suitID"></param>
    public override void CheckToCanLay()
    {
        base.CheckToCanLay();
    }
}

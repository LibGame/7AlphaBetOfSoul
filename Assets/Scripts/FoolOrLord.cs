using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FoolOrLord : CardProperties
{
    private FoolOrLordChose _foolOrLordChose;
    public FoolOrLordChose FoolLordChoose { set { _foolOrLordChose = value; } }

    private void Start()
    {
        CardSuitLeft = Suit.EmptyCard;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startPosition = transform.position;
        GameManager = FindObjectOfType<GameManager>();
        _foolOrLordChose = FindObjectOfType<FoolOrLordChose>();
    }

    protected override void OnMouseDown()
    {
        if (!_isLay && GameManager.IsCanTake)
        {
            var card = Instantiate(gameObject, _startPosition, Quaternion.identity);
            card.GetComponent<CardProperties>().IDCard = IDCard; // передаю айди этой карты в новую созданную
            _foolOrLordChose.FoolOrLord = GetComponent<FoolOrLord>();
            card.GetComponent<FoolOrLord>().FoolLordChoose = _foolOrLordChose;
            _spriteRenderer.sortingOrder = 2; // Делаю карту на слой выше карты в колоде
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
   
    /// <summary>
    /// Если игрок выбрал владыку то устанавливаем заказанную масть
    /// </summary>
    public void SerOrder(Suit suit)
    {
        OrderSuit = suit;
        CardSuitRight = suit;
        FoolOrderID = (int)suit;
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
        _foolOrLordChose.SwtichLordOrFoolInterface(true);
        GameManager.FoolOnTable = gameObject;
        if (GameManager.CenterPoint.AmountCardLay % 2 == 0) // Проверяю четное ли количество карт лежит на масти
            transform.rotation = Quaternion.Euler(0, 0, 0); // ставим вертикально
        else
            transform.rotation = Quaternion.Euler(0, 0, 90); // ставим горизонтально
        GameManager.IsSave = false;
        SetLayCardSettings(GameManager.CenterPoint.AmountCardLay);
        GameManager.WhiteCardList.Remove(gameObject); // удоляем нашу карту из массива карт в колоде
        gameObject.transform.SetParent(null); // открепляем от родителя

    }

    public override void ChangeColorSuits()
    {
        if (FoolOrderID == 7)
        {
            CardArrowType = CardType.BlackArrow;
            ChangeAllArrowColor();
        }
        else
        {
            GameManager.BackToBaseInconSuit();
            GameManager.CangeAllArrow(1, Color.black); // меняем всем стрелкам слой отображения и цвет 
            FindSuitOnRightCardSuit(); // Вызываю у масти на столе которая соответствует парвой масти на карте показать стрелки
            OnFindSameArrow(Color.white); // Подкрашиваю карту на которую нужно указать
        }
    }

    public void AddToMakeMoveArray()
    {
        GameManager.FoolOnTable = null;
        GameManager.MakeMove(gameObject); // Сделать ход
    }

    public void SetArrow()
    {
        GameManager.BackToBaseInconSuit();
        GameManager.CangeAllArrow(1, Color.black); // меняем всем стрелкам слой отображения и цвет 
        FindSuitOnRightCardSuit(); // Вызываю у масти на столе которая соответствует парвой масти на карте показать стрелки
        OnFindSameArrow(Color.white); // Подкрашиваю карту на которую нужно указать
        FindSameColor();
    }


    public void ChangeRotate()
    {
        if(GameManager.CenterPoint.AmountCardLay % 2 != 0)
        {
            transform.rotation = Quaternion.Euler(180, 0, 0); // ставим вертикально
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); // ставим вертикально
        }
    }

    public void ChangeAllArrowColor()
    {
        GameManager.CangeAllArrow(1, Color.white); // меняем всем стрелкам слой отображения и цвет 
        GameManager.BackToColorfullInconSuit();
        IsHaveOrder = false;
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

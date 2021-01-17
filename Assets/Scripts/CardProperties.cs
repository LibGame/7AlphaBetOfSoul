using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardProperties : MonoBehaviour
{
    public enum Suit { Hero = 0, Victim = 1, SlyMan = 2, Simpleton = 3, Villain = 4, Idol = 5, Skinner = 6, StupidMan = 7, EmptyCard = 8 }; // Масти карты
    public Suit CardSuitLeft; // Масть Карты с лево
    public Suit CardSuitRight; // Масть Карты с право
    public SpriteRenderer BackGroundCard { set; get; }
    public enum CardType { WhiteArrow, BlackArrow, FoolOrLord }; // Тип карты белая карта , черная карта или дурень
    public CardType CardArrowType; 
    protected Vector3 _offset;
    protected bool _isLay; // лижеит ли уже карта на игровом поле или все еще в колоде
    public int IDCard; // id карты в колоде
    [SerializeField] private int _sidID; // Уникальный айди карты для сида
    public int SuitOnLay { get; set; } // Номер масти на которой лежит карта
    public GameManager GameManager { get; set; } // Ссылка на гейм менеджер , заполняеться после создания карты 
    public int SidID { get { return _sidID; } }
    protected SpriteRenderer _spriteRenderer;
    protected Vector3 _startPosition; // Начальные положение карты
    public Suit OrderSuit { get; set; }
    public bool IsHaveOrder { set; get; } // Есть ли заказанная масть
    public int FoolOrderID = 7;

    protected virtual void OnMouseDown()
    {
        if (GameManager.IsCanTake)
        {
            _offset = gameObject.transform.position -
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            transform.localScale = new Vector3(0.5f, 0.5f, 1); // изменяем размер
        }
    }

    protected virtual void OnMouseDrag()
    {
        if (!_isLay && GameManager.IsCanTake)
        {
            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
                transform.position = Camera.main.ScreenToWorldPoint(newPosition) + _offset;
        }
    }

    protected virtual void OnMouseUp()
    {
    }

    /// <summary>
    /// Ложит карту на кординаты масти
    /// кординаты масти берет из массива масте по индексу переданному в параметре
    /// Меняю слои всех стрелок которые показывают на масть на 1
    /// </summary>
    public virtual void LayCardOnTable()
    {
        if (GameManager.CenterPoint.AmountCardLay % 2 == 0) // Проверяю четное ли количество карт лежит на масти
            transform.rotation = Quaternion.Euler(0, 0, 0); // ставим вертикально
        else
            transform.rotation = Quaternion.Euler(0, 0, 90); // ставим горизонтально
        GameManager.IsSave = false;
        SetLayCardSettings(GameManager.CenterPoint.AmountCardLay);
        GameManager.MakeMove(gameObject); // Сделать ход
        GameManager.WhiteCardList.Remove(gameObject); // удоляем нашу карту из массива карт в колоде
        gameObject.transform.SetParent(null); // открепляем от родителя
        ChangeColorSuits();

    }

    public void ShowBackGround(Color color)
    {
        if (CardArrowType == CardType.WhiteArrow)
            GameManager.ShowBackGround(CardSuitRight, GameManager.WhiteCardList, color);
        else
            GameManager.ShowBackGround(CardSuitRight, GameManager.WhiteCardList, color);

    }

    public virtual void ChangeColorSuits()
    {
        GameManager.BackToBaseInconSuit();
        GameManager.CangeAllArrow(1, Color.black); // меняем всем стрелкам слой отображения и цвет 
        FindSameColor();
        FindSuitOnRightCardSuit(); // Вызываю у масти на столе которая соответствует парвой масти на карте показать стрелки
        FindSuitOnLeftCardSuit();
        OnFindSameArrow(Color.white); // Подкрашиваю карту на которую нужно указать
    }

    public void SetLayCardSettings(int lay)
    {
        transform.position = GameManager.CenterPoint.transform.position; // устанавливаем карту на позицию
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = lay; // увеличиваем слой картый
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "CardOnTable";
        transform.localScale = new Vector3(0.5f, 0.5f, 1); // изменяем размер
        _isLay = true; // ставим флаг что карта находиться на столе а не в колоде
        GameManager.CenterPoint.AmountCardLay++; // увеличиваем количество карт на масти на 1
    }

    /// <summary>
    /// Удоляет карту
    /// </summary>
    public virtual void LayCardOnStartPosition()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Проверяет на какую масть положена карта
    /// пробегаеться по массиву мастей на столе и если есть пересечение то прерывает цикл и помещяет карту на места масти
    /// если нету совпадений в конце цикла то ставит карту на бозвое место карты в колоде
    /// </summary>
    public virtual void CheckToCollisionSuit()
    {
        if (_spriteRenderer.bounds.min.x <= GameManager.CenterPoint.SizeMinSuitPoint.x && _spriteRenderer.bounds.max.x >= GameManager.CenterPoint.SizeMaxSuitPoint.x
            && _spriteRenderer.bounds.min.y <= GameManager.CenterPoint.SizeMinSuitPoint.y && _spriteRenderer.bounds.max.y >= GameManager.CenterPoint.SizeMaxSuitPoint.y)
        {
            if(GameManager.Moves.Count == 0 || CardArrowType == CardType.FoolOrLord)
                LayCardOnTable();
            else
                CheckToArrow();
        }
        else
        {
            LayCardOnStartPosition();
        }
    }

    /// <summary>
    /// Метод проверяет соостветствует ли левая масть карты  если да то вызывает метод LayCardOntable 
    /// если не сооствествует то вызывает метод LayCardOnStartPosition
    /// В аргумент принимаеть ID иконки масти на столе
    /// </summary>
    /// <param name="suitID"></param>
    public virtual void CheckToCanLay()
    {
        if (CardSuitLeft == GameManager.Moves[GameManager.Moves.Count - 1].GetComponent<CardProperties>().CardSuitRight)
        {
            LayCardOnTable();
        }
        else
        {
            LayCardOnStartPosition();
        }
    }

    /// <summary>
    /// Проверяю какая стрелка лежала на предыдущей карте
    /// </summary>
    public virtual void CheckToArrow()
    {
        if (!IsHaveOrder && !GameManager.Moves[GameManager.Moves.Count -1].GetComponent<FoolOrLord>())
        {
            if (GameManager.Moves[GameManager.Moves.Count - 1].GetComponent<CardProperties>().CardArrowType == CardType.WhiteArrow)
            {
                CheckToCanLay();
            }
            else if (GameManager.Moves[GameManager.Moves.Count - 1].GetComponent<CardProperties>().CardArrowType == CardType.BlackArrow)
            {
                LayCardOnTable();
            }
        }
        else if(GameManager.Moves[GameManager.Moves.Count - 1].GetComponent<FoolOrLord>())
        {
            if (GameManager.Moves[GameManager.Moves.Count - 1].GetComponent<FoolOrLord>().OrderSuit == CardSuitLeft || GameManager.Moves[GameManager.Moves.Count - 1].GetComponent<FoolOrLord>().FoolOrderID == 7)
            {
                LayCardOnTable();
                IsHaveOrder = false;
            }
            else 
            {
                LayCardOnStartPosition();
            }
        }
        else
        {
            LayCardOnStartPosition();
        }

    }

    /// <summary>
    /// Находит масть настоле равную правой масти на карте 
    /// И вызывает метод у масти на столе который окрашивает стрелки
    /// </summary>
    public void FindSuitOnRightCardSuit()
    {
        for (int i = 0; i < GameManager.SuitPoints.Length; i++)
        {
            if (GameManager.SuitPoints[i].SuitTable == CardSuitRight )
            {
                GameManager.SuitPoints[i].ShowSameSuits(); // Вызываем у масти на котрой лежит карта показать стрелками схожие масти
                GameManager.SuitPoints[i].ChangeSpriteColorfull(); // меняем цвет масти на цветной
                break;
            }
        }
    }


    public void FindSameColor()
    {
        for (int i = 0; i < GameManager.SuitPoints.Length; i++)
        {
            if (GameManager.SuitPoints[i].SuitTable == CardSuitRight)
            {
                ShowBackGround(GameManager.SuitPoints[i].OwnColor);
                break;
            }
        }
    }


    public void EditBackGround(Color color , bool mode)
    {
        BackGroundCard.gameObject.SetActive(mode);
        BackGroundCard.color = color;
    }


    /// <summary>
    /// Находит масть настоле равную правой масти на карте 
    /// И вызывает метод у масти на столе который окрашивает стрелки
    /// </summary>
    public void FindSuitOnLeftCardSuit()
    {
        for (int i = 0; i < GameManager.SuitPoints.Length; i++)
        {
            if (GameManager.SuitPoints[i].SuitTable == CardSuitLeft)
            {
                GameManager.SuitPoints[i].ChangeSpriteColorfull(); // меняем цвет масти на цветной
            }
        }
    }

    /// <summary>
    /// Находит общию стрелку и подкрашивает ее
    /// В аргумент принимает цвет в который будет окрашена бщяя стрелка
    /// </summary>
    public void OnFindSameArrow(Color color)
    {
        for (int i = 0; i < GameManager.ArrowsOnTable.Length; i++)
        {
            if (GameManager.ArrowsOnTable[i].FirstSuit == CardSuitLeft && GameManager.ArrowsOnTable[i].SecondSuit == CardSuitRight
                || GameManager.ArrowsOnTable[i].SecondSuit == CardSuitLeft && GameManager.ArrowsOnTable[i].FirstSuit == CardSuitRight)
            {
                GameManager.ArrowsOnTable[i].ChangeColor(color);
                GameManager.ArrowsOnTable[i].ChangeLayer(2);
                break;
            }
        }
    }

}

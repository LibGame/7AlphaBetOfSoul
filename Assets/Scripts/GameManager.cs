using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SuitOnTable[] _suitPoints;
    [SerializeField] private SuitOnTable _centerPoint;
    [SerializeField] private Color _disableTurnColor; // затемнение аватара игрока если очередь другого
    [SerializeField] private Inteface _interface;
    [SerializeField] private GameObject[] _whiteArrowCards; // колода черных карт
    [SerializeField] private GameObject[] _blackArrowCards; // колода белых карт
    [SerializeField] private SpriteRenderer[] _backGroundCard; // задний фон карт
    [SerializeField] private CardProperties[] _cards; // Список всех карт включая дурня
    [SerializeField] private FoolOrLordChose _foolOrLordChose;
    [SerializeField] private Sprite _upButtonSprite;
    [SerializeField] private Sprite _downButtonSprite;
    [SerializeField] private Image _changeButtonCardSR;
    [SerializeField] private Sprite _blackArrow; // спрайт кнопки черной масти
    [SerializeField] private Sprite _whiteArrow; // спрайт кнопки белой масти
    [SerializeField] private Image _image; //  сслыка на изображение
    [SerializeField] private Tree _tree;

    private bool _changeArrowFlag = true; // флаг меняет карты по мастям
    private bool _isCreateTree; // создано ли древо
    public int MoveAmount { get; set; }
    public int TurnFirst { get; set; }
    public SuitOnTable[] SuitPoints { get { return _suitPoints; } }
    public SuitOnTable CenterPoint { get { return _centerPoint; } }

    public GameObject WhiteArrowsParent; // родительский объект для колоды карт с белой стрелочкой 
    public GameObject BlackArrowsParent; // родительский объект для колоды карт с черной стрелочкой 
    public GameObject FoolOnTable;

    public List<GameObject> BlackCardList = new List<GameObject>(); // список черных карт
    public List<GameObject> WhiteCardList = new List<GameObject>(); // список белых карт

    public static readonly List<GameObject> Moves = new List<GameObject>(); // все ходы которые были за игру
    private int _lastCardID;
    public Arrow[] ArrowsOnTable { private set; get; } // указательные стрелки на столе
    protected bool _isCanTake = true; // Можно Ли взять карту
    private bool _isCardChange; // поменялся ли ряд карт
    public bool IsCanTake { get { return _isCanTake; } set { _isCanTake = value; } }
    public bool IsSave; // было ли сейчас сохранение


    private void Start()
    {
        ArrowsOnTable = FindObjectsOfType<Arrow>();
        SpawnCards(_whiteArrowCards, WhiteArrowsParent, WhiteCardList); // спавню карты с белой стрелкой
        SpawnCards(_blackArrowCards, BlackArrowsParent, BlackCardList); // спавню карты с черной стрелкой
        BlackArrowsParent.SetActive(false); // отелючаю все черные карты
        //_opponent.SetParametrs(OpponentName, OpponentAvatar);
        //_opponent.SetAvatarImage();
        //ShowTurn(); // Показываю сразу чья очередь
        TurnFirst = 1; // я первый
    }

    /// <summary>
    /// Спавнит карты и сразу же делает их вложенным обьектом в родительский обьект
    /// и после цикла скрывает этот обьект
    /// Как аргументы принимаються массив карт которые нужно заспавнить , и обьект которые станет родительским для всех карт
    /// И массив в который запишуться все кары
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="parentObject"></param>
    private void SpawnCards(GameObject[] cards, GameObject parentObject, List<GameObject> writeArray)
    {
        float prefousXPos = -7.85f;
        int k = 0;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i == 7)
            {
                prefousXPos = -7.85f;
                k = 0;
            }
            GameObject card = Instantiate(cards[i], new Vector3(prefousXPos, -3.3f, 1), Quaternion.identity);
            prefousXPos = card.transform.position.x + 2f;
            card.GetComponent<CardProperties>().IDCard = i;
            card.GetComponent<CardProperties>().BackGroundCard = _backGroundCard[k];
            writeArray.Add(card);
            card.transform.SetParent(parentObject.transform);
            k++;
            if (i >= 7)
            {
                card.SetActive(false);
            }

        }
    }

    /// <summary>
    /// Возращяет все к начальным значениям
    /// </summary>
    public void BackGameToStartValues()
    {
        _isCanTake = true;
        _isCreateTree = false;
        BackToBaseInconSuit();
        _centerPoint.AmountCardLay = 0;
        ClearAllMove();
        CangeAllArrow(1, Color.black); // меняем всем стрелкам слой отображения и цвет 
    }

    public List<string> GetCharIDCards(string nameSave)
    {
        char[] cardID = PlayerPrefs.GetString(nameSave).ToCharArray();
        List<string> endCardID = new List<string>();
        string str = "";

        for(int i = 0; i < cardID.Length; i++)
        {
            if (cardID[i].ToString() != "/" )
            {
                str += cardID[i].ToString();
            }
            else
            {
                endCardID.Add(str);
                str = "";
            }
        }

        return endCardID;
    }

    public void ShowBackGround(CardProperties.Suit suit ,List<GameObject> cards, Color color)
    {
        SwitchCardBackGround(false);
        foreach (var card in cards)
        {
            if(card.activeSelf && card.GetComponent<CardProperties>().CardSuitLeft == suit)
            {
                card.GetComponent<CardProperties>().EditBackGround(color, true);
            }
        }
    }

    public void SwitchCardBackGround(bool mode)
    {
        for(int i = 0; i < _backGroundCard.Length; i++)
        {
            _backGroundCard[i].gameObject.SetActive(mode);
        }
    }

    /// <summary>
    /// Показывает древо
    /// </summary>
    public void ShoowTree()
    {
        if (!IsSave)
        {
            _tree.CreateTree(ReturnArrayInRange(_lastCardID), true);
        }
        else
        {
            _tree.CreateTree(ReturnArrayInRange(_lastCardID));
        }
        _isCreateTree = true;
        _lastCardID = Moves.Count;
    }

    private List<GameObject> ReturnArrayInRange(int k)
    {
        List<GameObject> list = new List<GameObject>();


        for(int i = k; i < Moves.Count; i++)
        {
            list.Add(Moves[i]);
        }

        return list;
    }

    /// <summary>
    /// Спавним карты по сгенерируемому сиду
    /// </summary>
    public void SpawnSaveCards(string nameSave)
    {
        ClearAllMove();
        CenterPoint.AmountCardLay = 0;
        _isCanTake = true;
        _isCreateTree = false;

        if (PlayerPrefs.HasKey(nameSave))
        {
            int amountCards = 0; // Количество заспавленных карт
            string[] cardID = GetCharIDCards(nameSave).ToArray();
            GameObject card = null;
            for (int i = 0; i < cardID.Length; i++)
            {

                if (amountCards % 2 == 0)
                {
                    card = Instantiate(_cards[int.Parse(cardID[i])].gameObject, CenterPoint.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    card = Instantiate(_cards[int.Parse(cardID[i])].gameObject, CenterPoint.gameObject.transform.position, Quaternion.Euler(0, 0, 90));
                }

                card.GetComponent<CardProperties>().GameManager = GetComponent<GameManager>();
                card.GetComponent<CardProperties>().SetLayCardSettings(amountCards);
                MakeMove(card);
                if (int.Parse(cardID[i]) == 21)
                {
                    if(int.Parse(cardID[i + 1]) < 7)
                    {
                        card.GetComponent<FoolOrLord>().SerOrder((CardProperties.Suit)int.Parse(cardID[i + 1]));
                    }
                    else
                    {
                        card.GetComponent<FoolOrLord>().FoolOrderID = 7;

                    }
                    i++;
                }
   
                amountCards++;
            }

            Moves[Moves.Count - 1].GetComponent<CardProperties>().ChangeColorSuits();

        }
    }


    /// <summary>
    /// Получаем сид положения карт
    /// </summary>
    public string GetSaveSID()
    {
        string sid = "";
        foreach (var move in Moves)
        {
            if (move.GetComponent<FoolOrLord>())
            {
                sid += move.GetComponent<CardProperties>().SidID.ToString() + "/" + move.GetComponent<FoolOrLord>().FoolOrderID +  "/";
            }
            else
            {
                sid += move.GetComponent<CardProperties>().SidID.ToString() + "/";
            }
        }

        return sid;
    }

    /// <summary>
    /// Добовляю название комнаты в сейвы
    /// </summary>
    public void AddNameToSaves(string namePage)
    {
        if (PlayerPrefs.HasKey("Saves"))
        {
            string saves = PlayerPrefs.GetString("Saves");
            PlayerPrefs.SetString("Saves", saves + namePage + "@");
        }
        else
        {
            PlayerPrefs.SetString("Saves",namePage + "@");
        }
    }

    /// <summary>
    /// Сохроняю страницу
    /// </summary>
    public void SavePage(string namePage)
    {
        _interface.GetPagesName();
        if (!_interface.CheckToSameNames(namePage))
        {
            PlayerPrefs.SetString(namePage, GetSaveSID());
        }
        else
        {
            PlayerPrefs.SetString(namePage, GetSaveSID());
            AddNameToSaves(namePage);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Метод использующий для кнопки 
    /// В аргумент принимает switchMode если = true то после 7 елементов будут активными , остольные отключаються
    /// если switchMoode = false то до 7 елементы будут активными , останльные отключатся
    /// </summary>
    /// <param name="switchMode"></param>
    public void ChangeListCard(bool switchMode)
    {
        for (int i = 0; i < WhiteCardList.Count; i++)
        {
            if (i >= 7)
            {
                WhiteCardList[i].SetActive(switchMode);
            }
            else
            {
                WhiteCardList[i].SetActive(!switchMode);
            }
        }

        if(Moves.Count > 0)
            Moves[Moves.Count - 1].GetComponent<CardProperties>().FindSameColor();
    }

    public void ChangeCardsArrowList()
    {
        if (!_isCardChange)
        {
            ChangeListCard(true);
            _changeButtonCardSR.sprite = _downButtonSprite;
           _isCardChange = true;
        }
        else
        {
            ChangeListCard(false);
            _changeButtonCardSR.sprite = _upButtonSprite;
            _isCardChange = false;
        }
        _changeArrowFlag = true;
        BlackArrowsParent.SetActive(false);
        WhiteArrowsParent.SetActive(true);
    }


    /// <summary>
    /// Меняет карты с мастями с белых на черные и на оборот
    /// </summary>

    public void ChangeArrow()
    {
        if (_changeArrowFlag)
        {
            SwitchCardBackGround(false);
            SwtichFlag(true, _whiteArrow);
        }
        else
        {
            SwtichFlag(false, _blackArrow);
        }
    }

    /// <summary>
    /// Меняет скрывает белые карты и показывает черные и на оборт
    /// Изменяет спрайт кнопки
    /// </summary>
    /// <param name="switchMode"></param>
    private void SwtichFlag(bool switchMode, Sprite sprite)
    {
        WhiteArrowsParent.SetActive(!switchMode);
        BlackArrowsParent.SetActive(switchMode);
        _changeArrowFlag = !switchMode;
        _image.sprite = sprite;
        if (Moves.Count > 0 && !switchMode)
            Moves[Moves.Count - 1].GetComponent<CardProperties>().FindSameColor();
    }


    /// <summary>
    /// Меняю всем указательным стрелкам на доске слой
    /// </summary>
    /// <param name="layer"></param>
    public void CangeAllArrow(int layer , Color color)
    {
        for (int i = 0; i < ArrowsOnTable.Length; i++)
        {
            ArrowsOnTable[i].ChangeLayer(layer);
            ArrowsOnTable[i].ChangeColor(color);
        }
        
    }

    /// <summary>
    /// Отчищяет все ходы и все карты на столе
    /// </summary>
    public void ClearAllMove()
    {
        _tree.DeleteAllCardsInTree();
        _lastCardID = 0;
        _isCreateTree = false;
        CenterPoint.AmountCardLay = 0;
        SwitchCardBackGround(false);
        foreach (var move in Moves)
        {
            Destroy(move);
        }
        _foolOrLordChose.SwtichLordOrFoolInterface(false);
        BackToBaseInconSuit();
        Moves.RemoveRange(0, Moves.Count);
        CangeAllArrow(1, Color.black);  
    }

    /// <summary>
    /// Возращяет все масти на столе к без цветным спрайтам
    /// </summary>
    public void BackToBaseInconSuit()
    {
        for(int i = 0; i < _suitPoints.Length; i++)
        {
            _suitPoints[i].ChangeSpriteBase();
        }
    }

    /// <summary>
    /// Возращяет все масти на столе к цветным спрайтам
    /// </summary>
    public void BackToColorfullInconSuit()
    {
        for (int i = 0; i < _suitPoints.Length; i++)
        {
            _suitPoints[i].ChangeSpriteColorfull();
        }
    }

    /// <summary>
    /// Удоляет последний ход
    /// </summary>
    public void DeleteLastMove()
    {
        if (Moves.Count > 0)
        {
            if (Moves[Moves.Count - 1].GetComponent<FoolOrLord>())
            {
                _foolOrLordChose.SwtichLordOrFoolInterface(false);
                Destroy(FoolOnTable);
            }

            if (FoolOnTable != null)
            {
                _foolOrLordChose.SwtichLordOrFoolInterface(false);
                Destroy(FoolOnTable);
                FoolOnTable = null;
            }
            else
            {
                if (_isCreateTree)
                    _tree.DeleteLastLine();
                Destroy(Moves[Moves.Count - 1]);
                Moves.RemoveAt(Moves.Count - 1);
            }

            CenterPoint.AmountCardLay--;
            CangeAllArrow(1, Color.black);
            if (Moves.Count > 0)
            {
                Moves[Moves.Count - 1].GetComponent<CardProperties>().ChangeColorSuits();
                Moves[Moves.Count - 1].GetComponent<CardProperties>().FindSameColor();
            }
            if (Moves.Count - 1 < 0)
                BackToBaseInconSuit();
        }
        else
        {
            SwitchCardBackGround(false);
        }
    }

    /// <summary>
    /// Делаем ход , добовляем ход - карту в список ходов
    /// </summary>
    /// <param name="card"></param>

    public void MakeMove(GameObject card)
    {
        Moves.Add(card);
        MoveAmount++;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree : MonoBehaviour
{
    [SerializeField] private Sprite[] _heroCards;
    [SerializeField] private Sprite[] _victimCards;
    [SerializeField] private Sprite[] _slymanCards;
    [SerializeField] private Sprite[] _simpletonCards;
    [SerializeField] private Sprite[] _villianCards;
    [SerializeField] private Sprite[] _idolCards;
    [SerializeField] private Sprite[] _skinnerCards;
    [SerializeField] private GameObject _parent; // Родительский объект для карт
    [SerializeField] private GameObject _case;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _treeLine;
    private System.Random _rnd = new System.Random();
    private Vector3 offset;
    private BoxCollider2D _boxCollider2D;
    private float _yBordier; // предел по y по высоте
    private float _ySpawnHeight; // высота последнего заспавненой карты
    private Vector3 _startCasePosition; // начальная позиция кейса
    private List<GameObject> _cardInTree = new List<GameObject>(); // все карты в панельки дерева
    private bool _isEmpty; // если пусто

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.enabled = false;
        _startCasePosition = _case.transform.position;
    }

    public void CloseTree()
    {
        _case.transform.position = _startCasePosition;
        _parent.SetActive(false);
        _lineRenderer.enabled = false;
        _boxCollider2D.enabled = false;
    }

    public void CreateTree(List<GameObject> cards , bool isContinue = false)
    {
        _parent.SetActive(true);
        _boxCollider2D.enabled = true;
        _lineRenderer.enabled = true;

        if (cards.Count > 0)
        {
            _isEmpty = false;

            if (!isContinue)
            {
                _ySpawnHeight = 0;
            }
            Sprite[] _cards;
            GameObject card = null;

            for (int i = 0; i < cards.Count; i++)
            {
                _cards = ReturnCards(cards[i].GetComponent<CardProperties>().CardSuitLeft);
                card = Instantiate(_treeLine, new Vector3(0, _ySpawnHeight, 0), Quaternion.identity);
                card.GetComponent<TreeLine>().SetSprite(RandomArray(_cards), cards[i].GetComponent<SpriteRenderer>().sprite, _rnd.Next(0,3));
                card.transform.SetParent(_case.transform);
                _cardInTree.Add(card);
                _ySpawnHeight = card.transform.position.y + 5f;
            }
            _yBordier = _cardInTree[_cardInTree.Count - 1].transform.position.y;
            _yBordier -= 2;

            CreateLinesToCards();
        }
    }

    private Sprite[] RandomArray(Sprite[] array)
    {
        for (int i = array.Length - 1; i >= 0; i--)
        {
            int j = _rnd.Next(i);
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        return array;
    }

    private void CreateLinesToCards()
    {
        _lineRenderer.positionCount = _cardInTree.Count;

        for (int i = 0; i < _cardInTree.Count; i++)
        {
            _lineRenderer.SetPosition(i, _cardInTree[i].GetComponent<TreeLine>().SelectCard.position);
        }
    }

    public void DeleteLastLine()
    {
        if(_cardInTree.Count > 0)
        {
            _yBordier = _cardInTree[_cardInTree.Count - 1].transform.position.y;
            Destroy(_cardInTree[_cardInTree.Count - 1]);
            _cardInTree.RemoveAt(_cardInTree.Count - 1);
            _ySpawnHeight -= 5;
        }
    }

    public void DeleteAllCardsInTree()
    {
        _isEmpty = true;
        _ySpawnHeight = 0;
        for (int i = 0; i < _cardInTree.Count; i++)
        {
            Destroy(_cardInTree[i]);
        }
        _cardInTree.RemoveRange(0, _cardInTree.Count);
        _lineRenderer.positionCount = 0;
    }

    private Sprite[] ReturnCards(CardProperties.Suit suit)
    {
        switch (suit)
        {
            case CardProperties.Suit.Hero:
                return _heroCards;
            case CardProperties.Suit.Idol:
                return _idolCards;
            case CardProperties.Suit.Simpleton:
                return _simpletonCards;
            case CardProperties.Suit.Skinner:
                return _skinnerCards;
            case CardProperties.Suit.SlyMan:
                return _slymanCards;
            case CardProperties.Suit.Victim:
                return _victimCards;
            case CardProperties.Suit.Villain:
                return _villianCards;
        }

        return _heroCards;
    }

    private void OnMouseDown()
    {
        offset = _case.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
    }

    private void OnMouseDrag()
    {
        if (!_isEmpty)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            _case.transform.position = new Vector3(_case.transform.position.x, Mathf.Clamp(curPosition.y, -_yBordier, 0), _case.transform.position.z);
            CreateLinesToCards();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoolOrLordChose : MonoBehaviour
{
    public FoolOrLord FoolOrLord { private get; set; }
    [SerializeField] private GameObject _foolrOrLordChooseInterface;
    [SerializeField] private GameObject _foolInterface;
    [SerializeField] private AnimationFoolSuits _animationFoolSuits;
    public bool IsSuitsAnimation; // Если масти в анимации
 
    public void SwtichLordOrFoolInterface(bool mode)
    {
        _foolrOrLordChooseInterface.SetActive(mode);
    }

    public void IsLord()
    {
        if (!IsSuitsAnimation)
        {
            SwtichLordOrFoolInterface(false);
            _foolInterface.SetActive(true);
            _animationFoolSuits.MoveToPosition();
            FoolOrLord.ChangeRotate();
            IsSuitsAnimation = true;
        }
    }

    public void IsFool()
    {
        SwtichLordOrFoolInterface(false);
        FoolOrLord.CardArrowType = CardProperties.CardType.BlackArrow;
        FoolOrLord.ChangeAllArrowColor();
        FoolOrLord.AddToMakeMoveArray();

    }

    public void SetSuitOnTable(int id)
    {
        if (!IsSuitsAnimation)
        {
            FoolOrLord.SerOrder((CardProperties.Suit)id);
            FoolOrLord.CardSuitRight = (CardProperties.Suit)id;
            FoolOrLord.SetArrow();
            FoolOrLord.AddToMakeMoveArray();
            _animationFoolSuits.MoveToStartPosition();
            IsSuitsAnimation = true;
        }
    }
}

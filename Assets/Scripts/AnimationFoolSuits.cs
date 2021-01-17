using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFoolSuits : MonoBehaviour
{
    [SerializeField] private SuitFool[] _suitFools;

    private Coroutine _moveCoroutine;
    private Coroutine _moveStartPositionCoroutine;

    [SerializeField] private FoolOrLordChose _foolOrLordChose;


    public void MoveToPosition()
    {
        if (!_foolOrLordChose.IsSuitsAnimation)
        {
            _moveCoroutine = StartCoroutine(MoveToPositionSuits());
        }
    }

    public void MoveToStartPosition()
    {
        if (!_foolOrLordChose.IsSuitsAnimation)
        {
            _moveStartPositionCoroutine = StartCoroutine(MoveToStartPositionSuits());
        }
    }

    private IEnumerator MoveToPositionSuits()
    {
        Invoke(nameof(StopCoroutineMoveToward), 0.5f);

        while (true)
        {
            for(int i = 0; i < _suitFools.Length; i++)
            {
                _suitFools[i].MoveToPosition(_suitFools[i].FirstPosition.position,5f);
            }
           

            yield return null;
        }
    }

    private IEnumerator MoveToStartPositionSuits()
    {
        Invoke(nameof(StopCoroutineMoveBack), 0.45f);

        while (true)
        {

            for (int i = 0; i < _suitFools.Length; i++)
            {
                _suitFools[i].MoveToPosition(_suitFools[i].StartPosition, 5f);
            }

            yield return null;
        }
    }

    private void StopCoroutineMoveBack()
    {
        StopCoroutine(_moveStartPositionCoroutine);
        _foolOrLordChose.IsSuitsAnimation = false;
        gameObject.SetActive(false);
    }


    private void StopCoroutineMoveToward()
    {
        _foolOrLordChose.IsSuitsAnimation = false;
        StopCoroutine(_moveCoroutine);
    }

}

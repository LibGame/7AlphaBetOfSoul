using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitFool : MonoBehaviour
{
    [SerializeField] private Transform _firstPosition; // Стартовая позиция к которой будет двигаться масть
    private Vector3 _startPosition; // Стартовая позиция к которой будет двигаться масть

    public Transform FirstPosition { get { return _firstPosition; } }
    public Vector3 StartPosition { get { return _startPosition; } }

    public void Start()
    {
        _startPosition = transform.position;
    }
    /// <summary>
    /// Двигает масть к указаным кординатам
    /// </summary>
    /// <param name="position"></param>
    /// <param name="speed"></param>
    public void MoveToPosition(Vector3 position, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, speed);
    }

    /// <summary>
    /// Проверяет находиться ли карта на заданной позиции если да то возращяет true 
    /// </summary>
    public bool CheckToSamePosition(Vector3 position)
    {
        if (transform.position.Equals(position))
        {
            return true;
        }
        return false;
    }
}

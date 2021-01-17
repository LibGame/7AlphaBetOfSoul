using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private GameObject[] _inverseGameObjects;
    [SerializeField] private Vector3 _leftPosition;
    [SerializeField] private Vector3 _rightPosition;
    [SerializeField] private GameObject[] _cardSpawnPosition;
    [SerializeField] private Text _textOnButton;
    private bool _isRight;

    /// <summary>
    /// Скрываю окно профиля
    /// </summary>
    public void OffProfile()
    {
        gameObject.SetActive(false);
    }


    public void SwitchForLeftHanders()
    {
        if (!_isRight)
        {
            InverseObjects(-1);
            ChangeCardPosition(_rightPosition);
            _isRight = true;
            _textOnButton.text = "Левша";
        }
        else
        {
            InverseObjects(-1);
            ChangeCardPosition(_leftPosition);
            _isRight = false;
            _textOnButton.text = "Правша";
        }
    }

    private void ChangeCardPosition(Vector3 pos)
    {
        for(int i = 0; i < _cardSpawnPosition.Length; i++)
        {
            _cardSpawnPosition[i].transform.position = pos;
        }
    }

    private void InverseObjects(int index)
    {
        for(int i = 0; i < _inverseGameObjects.Length; i++)
        {
            _inverseGameObjects[i].gameObject.transform.position = new Vector3(_inverseGameObjects[i].gameObject.transform.position.x * index,_inverseGameObjects[i].gameObject.transform.position.y, _inverseGameObjects[i].gameObject.transform.position.z);
        }
    }
}

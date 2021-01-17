using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSavePage : MonoBehaviour
{
    [SerializeField] private Text _text;
    private GameManager _gameManager;
    private Inteface _interface;
    private bool _isHaveText; // есть ли текст
    [SerializeField] private GameObject _deleteButton;

    /// <summary>
    /// Устанавливаем текст на кнопке текст соотвествует названию сохранения
    /// Также передаем ссылку на GameManager
    /// </summary>
    /// <param name="text"></param>
    public void SetButtonSettings(string text, GameManager gameManager, Inteface inter, bool isHaveText)
    {
        _text.text = text;
        _gameManager = gameManager;
        _interface = inter;
        _isHaveText = isHaveText;

        if (isHaveText)
            _deleteButton.SetActive(true);
        else
            _deleteButton.SetActive(false);
    }

    public void DeleteSave()
    {
        _interface.DeleteSaveInSaves(_text.text);
        _text.text = "Пусто";
        _isHaveText = false;
        _deleteButton.SetActive(false);
    }

    /// <summary>
    /// Загружаем сохранение
    /// </summary>
    public void LoadSave()
    {
        if (_isHaveText)
        {
            _interface.OnSavePanel();
            _interface.OnMenu();
            _gameManager.SpawnSaveCards(_text.text);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Inteface : MonoBehaviour
{
    [SerializeField] private GameObject _profileInferface;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _savePanel; // панелька на которой будут появляться сохраненки
    [SerializeField] private GameObject _newPagePanel;
    [SerializeField] private GameObject _sureToSavePanel;
    [SerializeField] private GameObject _sureToLeavePanel;
    [SerializeField] private GameObject[] _buttons; // кнопки которые загружат сохранение
    [SerializeField] private GameManager _gameManager; // Ссылка на гейм менеджер
    [SerializeField] private InputField _pageNameInputField; // ссылка на инпут фиелд имени страницы
    [SerializeField] private List<string> _namePages = new List<string>(); // Названия имен страниц
    [SerializeField] private GameObject _buttonSavePref;
    [SerializeField] private Transform _pointSpawn;
    [SerializeField] private Text _textError; 
    private bool _isOpenProfile; // открыт ли сейчас профиль
    private bool _isOpenMenu; // открыто ли сейчась меню
    private bool _isOpenSavePanel; // открыто ли сейчас сохранение
    private bool _isNewPagePanel; // открыто ли сейчас создание меню
    //private List<GameObject> _buttons = new List<GameObject>();
    private int _pageNumber = 0;
    public bool IsInGame { get; private set; }

    public void GetPagesName()
    {
        _namePages.RemoveRange(0, _namePages.Count);
        char[] saves = null;

        if (PlayerPrefs.HasKey("Saves"))
        {
            saves = PlayerPrefs.GetString("Saves").ToCharArray();
            string name = null;

            for(int i = 0; i < saves.Length; i++)
            {
                if(saves[i].ToString() != "@")
                {
                    name += saves[i].ToString();
                }
                else if (saves[i].ToString() == "@")
                {
                    _namePages.Add(name);
                    name = null;
                }
            }

        }
    }

    public void ChangePageIndex(int index)
    {
        int res = (int) Math.Ceiling((decimal)_namePages.Count / 5);
        _pageNumber += index;

        if (res - 1 >= _pageNumber && _pageNumber>= 0)
        {            
            TurnSavePages(_pageNumber);
        }
        else
        {
            _pageNumber -= index;
        }
    }

    public void DeleteSaveInSaves(string name)
    {
        string saves = "";
        _namePages.RemoveRange(0, _namePages.Count);

        foreach (var page in _namePages)
        {
            if(page != name)
            {
                saves += page + "@";
            }
        }
        PlayerPrefs.SetString("Saves", saves);
    }

    /// <summary>
    /// Перелестнуть сохранения
    /// </summary>
    public void TurnSavePages(int page)
    {
        int index = 0;

        for (int i = 0; i < _buttons.Length; i++)
        {
            index = _buttons.Length * page + i;

            if (index < _namePages.Count)
            {
                _buttons[i].GetComponent<ButtonSavePage>().SetButtonSettings(_namePages[index], _gameManager, GetComponent<Inteface>(),true);
            }
            else
            {
                _buttons[i].GetComponent<ButtonSavePage>().SetButtonSettings("Пусто", _gameManager, GetComponent<Inteface>(),false);
            }

        }
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void ExitFromGame()
    {
        Application.Quit();
    }

    public void OpenLeavePanel(bool mode)
    {
        _sureToLeavePanel.SetActive(mode);
    }

    public void OnSavePanel()
    {
        if (!_isOpenSavePanel)
        {
            _pageNumber = 0;
            _savePanel.SetActive(true);
            _isOpenSavePanel = true;
            GetPagesName();
            ChangePageIndex(0);

        }
        else
        {
            _namePages.RemoveRange(0, _namePages.Count);
            _pageNumber = 0;
            _savePanel.SetActive(false);
            _isOpenSavePanel = false;
        }
    }


    private void CloseNewPagePanel()
    {
        _newPagePanel.SetActive(false);
        _isNewPagePanel = false;
    }

    public void OnNewPagePanel()
    {
        if (!_isNewPagePanel)
        {
            _newPagePanel.SetActive(true);
            _isNewPagePanel = true;
        }
        else
        {
            CloseNewPagePanel();
        }
    }


    /// <summary>
    /// Показываю меню
    /// </summary>
    public void OnMenu()
    {
        if (!_isOpenMenu)
        {
            _menuPanel.SetActive(true); // панель меню активна
            _isOpenMenu = true; // меню открыто
            IsInGame = false; // сейчас игра
            _gameManager.IsCanTake = false; // игрок не может брать карты
        }
        else
        {
            IsInGame = true; // сейчас не игра
            _gameManager.IsCanTake = true;
            _savePanel.SetActive(false);
            _isOpenSavePanel = false;
            _menuPanel.SetActive(false);
            _isOpenMenu = false;
            CloseSureToSavePanel();
            CloseNewPagePanel();
        }
        _profileInferface.SetActive(false);
        _isOpenProfile = false;
    }

    /// <summary>
    /// Показываю окно профиля
    /// </summary>
    public void OnProfile()
    {
        if (!_isOpenProfile)
        {
            _profileInferface.SetActive(true);
            _gameManager.IsCanTake = false; // игрок не может брать карты
            _isOpenProfile = true;
            SwitchMenuPanel(false);
        }
        else
        {
            _gameManager.IsCanTake = true; // игрок не может брать карты
            _profileInferface.SetActive(false);
            _isOpenProfile = false;
        }

    }

    private void SwitchMenuPanel(bool mode)
    {
        _menuPanel.SetActive(mode);
        _isOpenMenu = mode;
    }

    /// <summary>
    /// Проверяет есть ли уже такое имя в сохранение
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckToSameNames(string name)
    {
        foreach(var page in _namePages)
        {
            if (page == name)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Создаем новую страницу
    /// </summary>
    public void CreateNewPage()
    {
        _gameManager.BackGameToStartValues();
        _gameManager.IsSave = true;
        OnNewPagePanel();
        OnMenu();
    }


    public void CloseSureToSavePanel()
    {
        _sureToSavePanel.SetActive(false);
    }

    /// <summary>
    /// Сохроняю карту через метод в гейм менеджере и передаю в него название страницы
    /// </summary>
    public void SaveCard()
    {

        GetPagesName();
        if (CheckToSameNames(_pageNameInputField.text) && _pageNameInputField.text != "")
        {
            _textError.text = "Сохраненно под именем " + _pageNameInputField.text;
            _gameManager.SavePage(_pageNameInputField.text);
            _pageNameInputField.text = "";
            _gameManager.IsSave = true;
        }
        else if (!CheckToSameNames(_pageNameInputField.text))
        {
            _textError.text = "Такое имя уже существует";
        }
        else
        {
            _textError.text = "Заполните Поле";
        }

    }

    public void CheckToCreateNew()
    {
        if (_gameManager.IsSave)
        {
            CreateNewPage();
        }
        else
        {
            _sureToSavePanel.SetActive(true);
        }
    }

}

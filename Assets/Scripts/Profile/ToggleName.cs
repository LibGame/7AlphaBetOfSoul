using UnityEngine;
using UnityEngine.UI;

public class ToggleName : MonoBehaviour
{
    [SerializeField] private Text _textName;
    [SerializeField] private InputField _selfTextName;

    public void ChangeName()
    {
        _textName.text = _selfTextName.text;
    }
}

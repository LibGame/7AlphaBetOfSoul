using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opponent : MonoBehaviour
{
    public string OpponentName { get; private set; }
    public Sprite OpponentAvatar { get; private set; }

    [SerializeField] private Image _avatarImage;

    public void SetParametrs(string name, Sprite avatar)
    {
        OpponentName = name;
        OpponentAvatar = avatar;
    }

    public void SetAvatarImage()
    {
        _avatarImage.sprite = OpponentAvatar;
    }

}

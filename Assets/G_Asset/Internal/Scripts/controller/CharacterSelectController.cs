using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelectController : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public Image userSelect;
    public Button left;
    public Button right;
    public Button play;
    int current = 0;
    private void Start()
    {
        left.onClick.AddListener(() =>
        {
            Left();
        });
        right.onClick.AddListener(() =>
        {
            Right();
        });
        play.onClick.AddListener(() =>
        {
            Play();
        });
        nameTxt.text = PlayerPrefs.GetString("username");
        ChangeSprite();
    }
    public void Left()
    {
        current = CharacterConfig.instance.Left(current);
        ChangeSprite();
    }
    public void Right()
    {
        current = CharacterConfig.instance.Right(current);
        ChangeSprite();
    }
    public void Play()
    {
        PlayerPrefs.SetString("characterName", CharacterConfig.instance.GetCharacterName(current));
        SceneController.instance.LoadNewScene(SceneController.SceneName.Play, true);
    }
    public void ChangeSprite()
    {
        userSelect.sprite = CharacterConfig.instance.GetCurrentSprite(current);
    }
}

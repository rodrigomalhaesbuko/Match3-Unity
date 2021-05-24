using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public Sprite OffSprite;
    public Sprite OnSprite;

    private Button _button;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
    }

    public void ChangeSprite()
    {
        if (_button.image.sprite == OnSprite)
            _button.image.sprite = OffSprite;
        else {
            _button.image.sprite = OnSprite;
        }
    }

    // re create the game scene to play agai
    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    //go back to menu scene 
    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

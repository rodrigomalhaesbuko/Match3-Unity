using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    [Tooltip("button off sprite")]
    public Sprite OffSprite;
    
    [Tooltip("button on sprite")] 
    public Sprite OnSprite;

    [Tooltip("button text")] 
    public Text btnText;

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
            // lower the btn text
            Vector3 textPos = btnText.rectTransform.position;
            textPos.y -= _button.image.rectTransform.sizeDelta.y * 0.5f;
            btnText.rectTransform.position = textPos;
        }
        AudioManager.instance.Play("ButtonSound");
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

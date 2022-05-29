using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaster : MonoBehaviour
{
    [Header("Sounds - AudioSources")]
    [SerializeField] private AudioSource _mainMenuMusic;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _doorSound;
    [SerializeField] private AudioSource _buttonClick;
    [SerializeField] private AudioSource _buttonHover;

    public void playMainMenuMusic() { _mainMenuMusic.Play(); }
    public void playMusic() { _gameMusic.Play(); } 
    public void playDoorSound() { _doorSound.Play(); }
    public void playButtonClickSound() { _buttonClick.Play(); }
    public void playButtonHoverSound() { _buttonHover.Play(); }
}

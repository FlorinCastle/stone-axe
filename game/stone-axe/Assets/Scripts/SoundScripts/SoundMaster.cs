using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaster : MonoBehaviour
{
    [Header("Sounds - AudioSources")]
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _doorSound;
    [SerializeField] private AudioSource _buttonClick;

    public void playMusic() { _music.Play(); } 
    public void playDoorSound() { _doorSound.Play(); }
    public void playButtonClickSound() { _buttonClick.Play(); }

}

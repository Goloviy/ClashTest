using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] SOUND_TYPE sound;

    private void OnEnable()
    {
        SoundController.Instance.PlaySound(sound);    
    }
}

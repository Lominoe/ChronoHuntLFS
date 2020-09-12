using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public AudioClip Louder_Real_Start_Song;
    public AudioClip Fight_Song;
    public AudioClip footSteps;
    public AudioClip arrowShot;
    public AudioClip arrowHitEnemy;
    public AudioClip arrowhitObj;
    public static AudioSource audioSrc;

    static public void PlayMusic (GameObject gameObj, AudioClip audioClip)
    {
        gameObj.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
}

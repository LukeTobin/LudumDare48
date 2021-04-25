using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class World : MonoBehaviour
{
    public static World Instance {get;set;}

    [SerializeField] AudioClip backgroundSfx;
    [SerializeField] CinemachineVirtualCamera vcamera; 

    AudioSource audio;
    CinemachineBasicMultiChannelPerlin cinPerlin;

    float shakeTimer;

    void Awake(){
        Instance = this;
    }

    void Start(){
        audio = GetComponent<AudioSource>();
        audio.clip = backgroundSfx;
        audio.Play();
    }

    void Update(){
        if(shakeTimer > 0){
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f){
                CinemachineBasicMultiChannelPerlin cinPerlin = vcamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    public void ScreenShake(float intensity, float time){
        CinemachineBasicMultiChannelPerlin cinPerlin = vcamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    public void ResetTiles(){
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach(Tile tile in tiles){
            tile.RebuildTile();
        }
    }
}

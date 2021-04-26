using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class World : MonoBehaviour
{
    public static World Instance {get;set;}

    [Header("Rooms")]
    [SerializeField] List<Room> rooms = new List<Room>();

    [Header("Settings")]
    [SerializeField] AudioClip backgroundSfx;
    [SerializeField] CinemachineVirtualCamera vcamera; 

    AudioSource audio;
    CinemachineBasicMultiChannelPerlin cinPerlin;

    float shakeTimer;

    Room activeRoom;
    int currentRoomIndex;

    void Awake(){
        Instance = this;
    }

    void Start(){
        audio = GetComponent<AudioSource>();
        audio.clip = backgroundSfx;
        audio.Play();
        currentRoomIndex = 0;

        if(!activeRoom && rooms.Count > 0) activeRoom = Instantiate(rooms[currentRoomIndex]);
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

    public void NextRoom(){
        // play transition
        if(!activeRoom) return;

        activeRoom.gameObject.SetActive(false);
        currentRoomIndex++;
        if(rooms[currentRoomIndex]) activeRoom = Instantiate(rooms[currentRoomIndex]);
        Player.Instance.NextStage();
    }

    public int GetRoomMoves(){
        if(!activeRoom) return 0;
        
        return activeRoom.roomMoves;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class World : MonoBehaviour
{
    public static World Instance {get;set;}

    [Header("Rooms")]
    [SerializeField] List<Room> rooms = new List<Room>();

    [Header("Settings")]
    [SerializeField] AudioClip backgroundSfx;
    [SerializeField] CinemachineVirtualCamera vcamera;
    [SerializeField] TMP_Text levelText;
    public Animator transitions;
    [Range(0, 1)]
    [SerializeField] float volume;

    AudioSource audio;
    CinemachineBasicMultiChannelPerlin cinPerlin;

    float shakeTimer;

    Room activeRoom;
    int currentRoomIndex;

    bool gameMuted = false;
    AudioSource[] sources;

    void Awake(){
        Instance = this;
    }

    void Start(){
        AudioListener.volume = volume;
        audio = GetComponent<AudioSource>();
        audio.clip = backgroundSfx;
        audio.Play();
        currentRoomIndex = 0;

        if(!activeRoom && rooms.Count > 0) activeRoom = Instantiate(rooms[currentRoomIndex]);
        Player.Instance.NextStage();
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
        StartCoroutine(LoadRoom());
    }

    public int GetRoomMoves(){
        if(!activeRoom) return 0;
        
        return activeRoom.roomMoves;
    }

    IEnumerator LoadRoom(){
        transitions.SetTrigger("end");
        yield return new WaitForSeconds(0.35f);

        activeRoom.gameObject.SetActive(false);
        currentRoomIndex++;
        if(rooms[currentRoomIndex]) activeRoom = Instantiate(rooms[currentRoomIndex]);
        levelText.text = currentRoomIndex.ToString();
        Player.Instance.NextStage();
    }

    public void MuteGame(){
        if(gameMuted){
            AudioListener.volume = volume;
            gameMuted = false;
        }else{
            AudioListener.volume = 0;
            gameMuted = true;
        }
    }
}

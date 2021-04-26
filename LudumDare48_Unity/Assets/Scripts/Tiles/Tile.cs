using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [Header("Tile Properties")]
    [SerializeField] protected Sprite tileSprite;
    [SerializeField] protected Color spriteColor = new Color(0, 0, 0, 1);
    [Space]
    public bool canBeEntered = true;
    [Range(0,10)]
    [SerializeField] protected int hitsRequired = 1;
    [Range(0, 10)]
    public int traversalCost = 1;
    [Space]
    [Range(0, 1f)]
    [SerializeField] float wiggleIntensity = 0.1f;
    [Range(0, 1f)]
    [SerializeField] float wiggleTime = 0.1f;
    [SerializeField] float wiggleSpeed = 1f;

    [Header("Sound Effects")]
    [SerializeField] protected AudioClip hitSfx;
    [SerializeField] protected AudioClip destroySfx;
    [Range(0, 0.5f)]
    [SerializeField] protected float pitchNoise = 0.2f;

    [Header("Debugging")]
    public bool tileIsBroken = false;
    [SerializeField] Sprite uniqueDestroyedSprite = null;
    [Tooltip("Color the tile will be when destroyed")]
    [SerializeField] Color destroyColor = new Color(0, 0, 0, 1);
    [Space]
    [Tooltip("Whether the sprite should be hidden when the game starts")]
    public bool spriteHides = true;
    [SerializeField] Sprite uniqueHiddenSprite = null;
    [Tooltip("Color the tile will be when hidden")]
    [SerializeField] Color hiddenColor = new Color(0, 0, 0, 1);

    protected SpriteRenderer spriteRenderer;
    protected AudioSource audio;

    float shakeTimer = 0;
    Vector2 orginalPos;

    // Original states
    int _hitsRequired;
    bool _tileIsBroken;

    bool isVisible;

    void Awake() {
        _hitsRequired = hitsRequired;
        _tileIsBroken = tileIsBroken;
    }
    
    protected virtual void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        if(tileSprite) spriteRenderer.sprite = tileSprite;
        spriteRenderer.color = spriteColor;

        if(spriteHides){
            if(uniqueHiddenSprite) spriteRenderer.sprite = uniqueHiddenSprite;
            spriteRenderer.color = hiddenColor; 
        }
    }

    protected virtual void Update(){
        if(shakeTimer > 0){
            shakeTimer -= Time.deltaTime;
            float xVector = orginalPos.x + Mathf.Sin(Time.time * wiggleSpeed) * wiggleIntensity;
            float yVector = orginalPos.y + Mathf.Sin(Time.time * wiggleSpeed) * wiggleIntensity;
            transform.position = new Vector3(xVector, yVector, 0);
            if(shakeTimer <= 0f){
                transform.position = orginalPos;
            }
        }
    }

    // Returns rather the tile can be entered
    public virtual bool EnterTile(){
        if(tileIsBroken) return true;

        if(hitSfx){
            float pitch = Random.Range(-pitchNoise, pitchNoise);
            audio.clip = hitSfx;
            audio.pitch = 1 + pitch;
            audio.Play();
        }
        

        hitsRequired--;
        if(hitsRequired <= 0){
            OnTileEnter();
            OnTileDestroyed();
            return true;
        }

        TileShake();

        return false;
    }

    public virtual void OnTileEnter(){
        if(tileIsBroken) return;

        //traversalCost = 0;
        if(uniqueDestroyedSprite) spriteRenderer.sprite = uniqueDestroyedSprite;
        spriteRenderer.color = destroyColor;
        tileIsBroken = true;
    }

    public virtual void OnTileDestroyed(){
        if(destroySfx){
            float pitch = Random.Range(-pitchNoise, pitchNoise);
            audio.clip = destroySfx;
            audio.pitch = 1 + pitch;
            audio.Play();
        }

        World.Instance.ScreenShake(1f, 0.2f);
        
        // destroy effects
    }

    public virtual void DisplaySprite(){
        if(isVisible || tileIsBroken || !gameObject.activeInHierarchy) return;

        if(tileSprite) spriteRenderer.sprite = tileSprite;
        spriteRenderer.color = spriteColor;
        isVisible = true;
    }

    public virtual void HideSprite(){
        if(!isVisible || tileIsBroken) return;

        if(uniqueHiddenSprite) spriteRenderer.sprite = uniqueHiddenSprite;
        spriteRenderer.color = hiddenColor;
        isVisible = false;
    }

    public virtual void RebuildTile(){
        hitsRequired = _hitsRequired;
        tileIsBroken = _tileIsBroken;
        isVisible = false;

        if(uniqueHiddenSprite) spriteRenderer.sprite = uniqueHiddenSprite;
        spriteRenderer.color = hiddenColor;
    }

    public virtual void Damage(int damage){
        if(!canBeEntered) return;

        TileShake();

        hitsRequired -= damage;
        if(hitsRequired <= 0){
            OnTileEnter();
            OnTileDestroyed();
        }
    }

    public virtual void TileShake(){
        shakeTimer = wiggleTime;
        orginalPos = transform.position;
    }
}

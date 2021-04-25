using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [Header("Tile Properties")]
    [SerializeField] Sprite tileSprite;
    [SerializeField] Color spriteColor = new Color(0, 0, 0, 1);
    [Space]

    public bool canBeEntered = true;
    [Range(0,10)]
    [SerializeField] int hitsRequired = 1;
    [Range(0, 10)]
    public int traversalCost = 1;

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

    SpriteRenderer spriteRenderer;

    // Original states
    int _hitsRequired;
    bool _tileIsBroken;

    bool isVisible;

    void Awake() {
        _hitsRequired = hitsRequired;
        _tileIsBroken = tileIsBroken;
    }
    
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(tileSprite) spriteRenderer.sprite = tileSprite;
        spriteRenderer.color = spriteColor;

        if(spriteHides){
            if(uniqueHiddenSprite) spriteRenderer.sprite = uniqueHiddenSprite;
            spriteRenderer.color = hiddenColor; 
        }
    }

    // Returns rather the tile can be entered
    public virtual bool EnterTile(){
        if(tileIsBroken) return true;

        hitsRequired--;
        if(hitsRequired <= 0){
            OnTileEnter();
            OnTileDestroyed();
            return true;
        }

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
        // destroy effects
    }

    public virtual void DisplaySprite(){
        if(isVisible || tileIsBroken) return;

        if(uniqueHiddenSprite) spriteRenderer.sprite = tileSprite;
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
}

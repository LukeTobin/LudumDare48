using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Bomb : Tile
{
    [Header("Bomb Settings")]
    [Range(0, 10)]
    [SerializeField] public int explodeDamage = 5;
    [SerializeField] public GameObject indicatorSprite = null;
    [SerializeField] LayerMask tileLayer;
    [SerializeField] LayerMask playerLayer;

    bool triggerExplosionCountdown = false;

    List<Tile> neighbours = new List<Tile>();

    GameObject up, down, left, right;

    public override bool EnterTile()
    {
        if(tileIsBroken) return true;

        if(hitSfx){
            float pitch = Random.Range(-pitchNoise, pitchNoise);
            audio.clip = hitSfx;
            audio.pitch = 1 + pitch;
            audio.Play();
        }

        TriggerExplosionStart();
        hitsRequired--;
        return false;
    }

    void TriggerExplosionStart(){
        if(triggerExplosionCountdown) return;

        triggerExplosionCountdown = true;
        if(indicatorSprite){
            up = Instantiate(indicatorSprite, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            down = Instantiate(indicatorSprite, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);
            left = Instantiate(indicatorSprite, new Vector2(transform.position.x - 1, transform.position.y), Quaternion.identity);
            right = Instantiate(indicatorSprite, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
        }
    }

    public bool Countdown(){
        if(!triggerExplosionCountdown) return false;

        hitsRequired--;
        if(hitsRequired <= 0){
            neighbours = GetNeighbours();
            foreach(Tile tile in neighbours){
                tile.Damage(explodeDamage);
            }

            OnTileEnter();
            OnTileDestroyed();
            return true;
        } 

        return false;
    }

    public override void OnTileDestroyed()
    {
        base.OnTileDestroyed();

        Destroy(up);
        Destroy(down);
        Destroy(left);
        Destroy(right);
    }

    public override void HideSprite()
    {
        
    }

    public override void RebuildTile()
    {
        base.RebuildTile();
        triggerExplosionCountdown = false;
        if(up) Destroy(up);
        if(down) Destroy(down);
        if(right) Destroy(right);
        if(left) Destroy(left);
    }

    List<Tile> GetNeighbours(){
        List<Tile> neighbourList = new List<Tile>();

        // make a collider finding all tiles
        Collider2D hitRight = Physics2D.OverlapPoint(new Vector2(transform.position.x + 1, 
                                                            transform.position.y),
                                                            tileLayer);

        Collider2D hitLeft = Physics2D.OverlapPoint(new Vector2(transform.position.x - 1, 
                                                            transform.position.y),
                                                            tileLayer);
        
        Collider2D hitDown = Physics2D.OverlapPoint(new Vector2(transform.position.x, 
                                                            transform.position.y - 1),
                                                            tileLayer);
        
        Collider2D hitUp = Physics2D.OverlapPoint(new Vector2(transform.position.x, 
                                                            transform.position.y + 1),
                                                            tileLayer);

        // Check if a tile exists
        if(hitRight && hitRight.GetComponent<Tile>())
            neighbourList.Add(hitRight.GetComponent<Tile>());
        
        if(hitLeft && hitLeft.GetComponent<Tile>())
            neighbourList.Add(hitLeft.GetComponent<Tile>());

        
        if(hitDown && hitDown.GetComponent<Tile>())
            neighbourList.Add(hitDown.GetComponent<Tile>());

        
        if(hitUp && hitUp.GetComponent<Tile>())
            neighbourList.Add(hitUp.GetComponent<Tile>());

        return neighbourList;

    }

    public bool PlayerIsNeighbours(){
        // make a collider finding all tiles
        Collider2D hitRight = Physics2D.OverlapPoint(new Vector2(transform.position.x + 1, 
                                                            transform.position.y),
                                                            playerLayer);

        Collider2D hitLeft = Physics2D.OverlapPoint(new Vector2(transform.position.x - 1, 
                                                            transform.position.y),
                                                            playerLayer);
        
        Collider2D hitDown = Physics2D.OverlapPoint(new Vector2(transform.position.x, 
                                                            transform.position.y - 1),
                                                            playerLayer);
        
        Collider2D hitUp = Physics2D.OverlapPoint(new Vector2(transform.position.x, 
                                                            transform.position.y + 1),
                                                            playerLayer);

        // Check if a tile exists
        if(hitRight && hitRight.GetComponent<Player>())
            return true;
        
        if(hitLeft && hitLeft.GetComponent<Player>())
            return true;
        
        if(hitDown && hitDown.GetComponent<Player>())
            return true;
       
        if(hitUp && hitUp.GetComponent<Player>())
            return true;

        return false;

    }
}

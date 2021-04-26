using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance {get; set;}

    [Header("Player Properties")]
    [SerializeField] float movementSize;
    [SerializeField] float movementSpeed;
    [SerializeField] LayerMask tileLayer;
    [Space]
    [SerializeField] int movementPoints = 30;

    [Header("Sound Effects")]
    [SerializeField] AudioClip walkSfx;

    [Header("References")]
    [SerializeField] TMP_Text staminaText;
    [SerializeField] Transform spawn;

    Vector2 input;
    Vector2 targetPosition;
    Vector2 direction;

    Vector2 linearPosition;

    List<Tile> neighbours = new List<Tile>();
    List<Tile_Bomb> bombs = new List<Tile_Bomb>();
    
    Animator animator;
    SpriteRenderer spriteRenderer;
    AudioSource audio;

    int _movementPoints;

    void Awake(){
        Instance = this;
    }

    void Start(){
        staminaText.text = movementPoints.ToString();
        _movementPoints = movementPoints;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        linearPosition = transform.position;
    }

    void Update(){
         if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
             Collider2D currentTile = Physics2D.OverlapPoint(transform.position, tileLayer);
             if(currentTile && currentTile.GetComponent<Tile_Ladder>())
                CheckMove(new Vector2(0,movementSize));
        }
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            CheckMove(new Vector2(0,-movementSize));
            //spriteRenderer
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            CheckMove(new Vector2(-movementSize,0));
            spriteRenderer.flipX = false;
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            CheckMove(new Vector2(movementSize,0));
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate() {
        if(input == new Vector2(0, 0)) return;

        if((Vector2)transform.position != targetPosition)
            transform.position = (Vector2)transform.position + direction * movementSpeed * Time.deltaTime;
        else{
            linearPosition = targetPosition;
            transform.position = targetPosition;
            animator.SetBool("moving", false);
            animator.SetBool("falling", false);
            animator.SetBool("climbing", false);
            audio.Stop();
            input = new Vector2(0, 0); // reset input vector
        }
    }

    // Check if the player can move into tile location
    void CheckMove(Vector2 wantedInput){
        Collider2D hit = Physics2D.OverlapPoint(new Vector2(linearPosition.x + wantedInput.x, 
                                                            linearPosition.y + wantedInput.y),
                                                            tileLayer);
        if(!hit) return;

        Tile _tile = hit.GetComponent<Tile>();
        if(_tile && _tile.canBeEntered){
            DecreaseMovement(1);
            
            if(_tile is Tile_Bomb)
                if(!bombs.Contains(_tile.GetComponent<Tile_Bomb>()) && !_tile.tileIsBroken) 
                    bombs.Add(_tile.GetComponent<Tile_Bomb>());
            
            if(_tile.EnterTile()){
                audio.clip = walkSfx;
                audio.Play();

                input = wantedInput;
                targetPosition = (Vector2)linearPosition + input;
                direction = (targetPosition - (Vector2)linearPosition).normalized;
                if(input == new Vector2(0,-1))
                    animator.SetBool("falling", true);
                else if(input == new Vector2(0, 1))
                    animator.SetBool("climbing", true);
                else
                    animator.SetBool("moving", true);

                UpdateNeighbours(targetPosition);
            }
        }
    }

    public void DecreaseMovement(int amount){
        movementPoints -= amount;
        staminaText.text = movementPoints.ToString();
        if(movementPoints <= 0) ResetPlayer();

        for(int i = 0;i< bombs.Count;i++){
            if(!bombs[i]) return;

            if(bombs[i].Countdown()){
                if(bombs[i].PlayerIsNeighbours()) {
                    ResetPlayer();
                }else{
                    bombs.Remove(bombs[i]);
                }
            }
        }
    }

    public void ResetPlayer(){
        movementPoints = _movementPoints;
        staminaText.text = movementPoints.ToString();

        transform.position = spawn.position;
        linearPosition = spawn.position;
        targetPosition = spawn.position;
        input = new Vector2(0, 0);

        animator.SetBool("moving", false);
        animator.SetBool("falling", false);

        World.Instance.ResetTiles();
        bombs.Clear();
    }

    void UpdateNeighbours(Vector2 targetPos){
        // Hide each neighbour and reset the list
        foreach(Tile tile in neighbours){
            tile.HideSprite();
        }

        neighbours.Clear();

        // make a collider finding all tiles
        Collider2D hitRight = Physics2D.OverlapPoint(new Vector2(targetPos.x + 1, 
                                                            targetPos.y),
                                                            tileLayer);

        Collider2D hitLeft = Physics2D.OverlapPoint(new Vector2(targetPos.x - 1, 
                                                            targetPos.y),
                                                            tileLayer);
        
        Collider2D hitDown = Physics2D.OverlapPoint(new Vector2(targetPos.x, 
                                                            targetPos.y - 1),
                                                            tileLayer);
        
        Collider2D hitUp = Physics2D.OverlapPoint(new Vector2(targetPos.x, 
                                                            targetPos.y + 1),
                                                            tileLayer);

        // Check if a tile exists
        if(hitRight && hitRight.GetComponent<Tile>()){
            Tile right = hitRight.GetComponent<Tile>();
            if(!right.tileIsBroken){
                right.DisplaySprite();
                neighbours.Add(right);
            }
        }
        
        if(hitLeft && hitLeft.GetComponent<Tile>()){
            Tile left = hitLeft.GetComponent<Tile>();
            if(!left.tileIsBroken){
                left.DisplaySprite();
                neighbours.Add(left);
            }
            
        }
        
        if(hitDown && hitDown.GetComponent<Tile>()){
            Tile down = hitDown.GetComponent<Tile>();
            if(!down.tileIsBroken){
                down.DisplaySprite();
                neighbours.Add(down);
            }    
        }
        
        if(hitUp && hitUp.GetComponent<Tile>()){
            Tile up = hitUp.GetComponent<Tile>();
            if(!up.tileIsBroken){
                up.DisplaySprite();
                neighbours.Add(up);
            }
        }
    }
}
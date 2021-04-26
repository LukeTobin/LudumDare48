using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Door : Tile
{   
    [Header("Door Settings")]
    [SerializeField] bool isLocked = true;

    public override bool EnterTile()
    {
        if(isLocked) return false;
        return base.EnterTile();
    }

    public void UnlockDoor(){
        isLocked = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Finish : Tile
{
    public override bool EnterTile()
    {
        OnTileEnter();
        return true;
    }

    public override void OnTileEnter()
    {
        Debug.Log("FINISH TILE");
    }

    public override void RebuildTile()
    {
        // NULL
    }
}

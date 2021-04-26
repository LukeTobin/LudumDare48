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
        World.Instance.NextRoom();
    }

    public override void RebuildTile()
    {
        // NULL
    }
}

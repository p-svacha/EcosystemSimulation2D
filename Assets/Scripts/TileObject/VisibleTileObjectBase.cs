using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A VisibleTileObject is a TileObject represented as an own visible GameObject in the world with a SpriteRenderer.
/// <br/> It's one of the two types of TileObjects beside TilemapTileObject.
/// </summary>
public abstract class VisibleTileObjectBase : TileObjectBase
{
    // General
    public SpriteRenderer Renderer { get; private set; }

    public override void Init()
    {
        base.Init();

        Renderer = GetComponent<SpriteRenderer>();
    }
}

public enum TileObjectId
{
    TallGrass,
    Creer,
    Wofox
} 

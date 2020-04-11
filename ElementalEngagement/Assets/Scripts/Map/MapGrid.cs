using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{

    public int width = 150;
    public int height = 150;
    public float tilesize = 2;

    Tile[,] grid;


    // Start is called before the first frame update
    public void Init()
    {
        grid = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Tile(x, y);
            }
        }
        this.gameObject.transform.localScale = new Vector3(tilesize * width / 10, 1, tilesize * height / 10);
        this.gameObject.transform.Translate(new Vector3(width, 0, height));
    }


    // Get tile from world coordinates (float)
    public Tile getTileAt(float x, float y)
    {
        return getTileAt((int)Mathf.Floor(x / tilesize), (int)Mathf.Floor(y / tilesize));
    }

    // Get tile from grid coordinates (int)
    public Tile getTileAt(int x, int y)
    {
        if (x >= width || x < 0 || y >= height || y < 0)
        {
            return null;
        }
        return grid[x, y];
    }

    // Get a rectangular selection of many tiles
    public TileSelection selectTiles(int x, int y, int w = 1, int h = 1)
    {
        TileSelection selection = new TileSelection(x, y, w, h);
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Tile t = getTileAt(x + i, y + j);
                if (t != null)
                {
                    selection.tiles.Add(t);
                }
                else
                {
                    selection.withinBounds = false;
                }
            }
        }
        return selection;

    }

    public Structure getStructureAt(int x, int y)
    {
        return getTileAt(x, y).getStructure();
    }

    public Vector3 getTileWorldPosition(Tile tile)
    {
        return new Vector3(tile.x * tilesize, 0, tile.y * tilesize);
    }

    // Place a structure on a tile.
    // Update tiles with data of structure that sits on them
    // Note that the structure is not created here - we return a position so we can create the structure in the calling function
    public Vector3 placeStructureAt(Structure newStructure, int x, int y)
    {
        int w = (int)newStructure.dimensions.x;
        int h = (int)newStructure.dimensions.y;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                getTileAt(x + i, y + j).setStructure(newStructure);
            }
        }
        return getTileWorldPosition(getTileAt(x, y)) + new Vector3(w * tilesize / 2, 0, h * tilesize / 2);
    }

    public void clearTile(int x, int y){
        getTileAt(x, y).clear();
    }





    // Manage wall rotation. Walls should be rotated to form a line with adjacent walls
    // When we build a new wall, we must update the orientation of nearby walls
    public void updateWallsAroundTile(Wall wall, int x, int y)
    {
        if (wall)
        {
            wall.setRotation(getWallRotation(x, y));
        }

        Structure s = getStructureAt(x, y + 1);
        if (s)
        {
            if (s.finished && s.type == "Wall")
            {
                s.GetComponent<Wall>().setRotation(getWallRotation(x, y + 1));
            }
        }
        s = getStructureAt(x, y - 1);
        if (s)
        {
            if (s.finished && s.type == "Wall")
            {
                s.GetComponent<Wall>().setRotation(getWallRotation(x, y - 1));
            }
        }
        s = getStructureAt(x + 1, y);
        if (s)
        {
            if (s.finished && s.type == "Wall")
            {
                s.GetComponent<Wall>().setRotation(getWallRotation(x + 1, y));
            }
        }
        s = getStructureAt(x - 1, y);
        if (s)
        {
            if (s.finished && s.type == "Wall")
            {
                s.GetComponent<Wall>().setRotation(getWallRotation(x - 1, y));
            }
        }
    }

    public Wall.Mode getWallRotation(int x, int y)
    {
        int N = 0, S = 0, W = 0, E = 0;
        Structure s = getStructureAt(x, y + 1);
        if (s)
        {
            if (s.finished && s.type == "Wall" && !s.MarkedForDeletion) N = 1;
        }
        s = getStructureAt(x, y - 1);

        if (s)
        {
            if (s.finished && s.type == "Wall" && !s.MarkedForDeletion) S = 1;
        }
        s = getStructureAt(x + 1, y);

        if (s)
        {
            if (s.finished && s.type == "Wall" && !s.MarkedForDeletion) E = 1;
        }
        s = getStructureAt(x - 1, y);

        if (s)
        {
            if (s.finished && s.type == "Wall" && !s.MarkedForDeletion) W = 1;
        }

        if (N + E + S + W != 2)
        {
            return Wall.Mode.Corner;
        }
        else if (N + S == 2)
        {
            return Wall.Mode.Vertical;
        }
        else if (E + W == 2)
        {
            return Wall.Mode.Horizonal;
        }
        else
        {
            return Wall.Mode.Corner;
        }

    }

}

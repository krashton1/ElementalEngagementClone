using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// The Map Controller class handles user interaction with the map
// It's primary function is to allow the user to select positions on the map

public class MapController : MonoBehaviour
{

    enum StateEnum {defaultState, selectingTile}

    StateEnum state = StateEnum.defaultState;

    public GameObject tileSelectorPrefab;
    List<GameObject> tileSelectors = new List<GameObject>();

    public Material valid;
    public Material invalid;


    TileSelection selection;
    int w, h; // dimensions of selection
    bool isSelectionValid = false;

    UnityEvent tileSelectionEvent;
    MapGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        if (tileSelectionEvent == null){
            tileSelectionEvent = new UnityEvent();
        }
        grid = this.GetComponent<MapGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == StateEnum.selectingTile){
            RaycastHit hit = new RaycastHit();
            var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 300.0f, LayerMask.GetMask("Ground")))
            {
                Vector3 worldPosition = hit.point;
                hoverTiles(worldPosition.x, worldPosition.z);
            }
            if (Input.GetMouseButtonDown(0) && isSelectionValid){
                tileSelectionEvent.Invoke();
                if (!Input.GetKey("left ctrl")&& !Input.GetKey("left shift"))
                endTileSelection();
            }
            if (Input.GetMouseButtonDown(1)){
                endTileSelection();
            }
        }
    }

    public void beginTileSelection(int width, int height){
            w = width;
            h = height;
            state = StateEnum.selectingTile;
            for (int i = 0; i < width; i++){
                for (int j = 0; j < height; j++){
                    GameObject tileSelector = GameObject.Instantiate(tileSelectorPrefab, Vector3.zero, Quaternion.identity);
                    tileSelector.name = "TileSelector" + i + "-" + j;
                    tileSelectors.Add(tileSelector);
                }
            }
        
    }

    void endTileSelection(){
        state = StateEnum.defaultState;
        for (int i = tileSelectors.Count - 1; i >= 0; i--){
            Destroy(tileSelectors[i]);
            tileSelectors.RemoveAt(i);
        }
        tileSelectionEvent.RemoveAllListeners();
    }

    // Highlight tiles under mouse
    public void hoverTiles(float x, float y){
        Tile selectedTile = grid.getTileAt(x, y);
        selection = grid.selectTiles(selectedTile.x, selectedTile.y, w, h);
        isSelectionValid = true;

        for (int i = 0; i<tileSelectors.Count; i++){
            if (i < selection.tiles.Count){
                tileSelectors[i].SetActive(true);
                tileSelectors[i].transform.position = grid.getTileWorldPosition(selection.tiles[i]) + new Vector3(grid.tilesize, 0,grid.tilesize) / 2;
                if (!selection.tiles[i].isEmpty() || selection.withinBounds == false){
                    tileSelectors[i].GetComponent<Renderer>().material = invalid;
                    isSelectionValid = false;
                }
                else{
                    tileSelectors[i].GetComponent<Renderer>().material = valid;
                }
            }
            else {
                tileSelectors[i].SetActive(false);
            }
        }

        
    }


    // For passing selection info
    public void listenForSelection(UnityAction call){
        if (tileSelectionEvent == null){
            tileSelectionEvent = new UnityEvent();
        }
        tileSelectionEvent.AddListener(call);
    }

    public Vector2 getSelectedPosition(){
        return new Vector2(selection.x, selection.y);
    }

    public Vector3 placeStructureAt(Structure newStructure, int x, int y){
        return grid.placeStructureAt(newStructure, x, y);

    }

}

public struct TileSelection {
    public bool withinBounds;
    public List<Tile> tiles;
    public int x, y; //bottom corner
    public int w, h;

    public TileSelection(int x_, int y_, int w_, int h_){
        x = x_;
        y = y_;
        w = w_;
        h = h_;
        withinBounds = true;
        tiles = new List<Tile>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public enum Mode {Horizonal, Vertical, Corner};
    MapGrid grid;
    int x = 0;
    int y = 0;

    private void Start() {
        grid = GameObject.Find("GroundPlane").GetComponent<MapGrid>();
    }

    public void setRotationAroundTile(int x_, int y_){
        x = x_;
        y = y_;
        grid.updateWallsAroundTile(this, x, y);
    }

    public void setRotation(Wall.Mode m){
        if (m == Mode.Corner){
            foreach (Transform child in gameObject.transform) {
                if (child.CompareTag("Default Model")) { child.gameObject.SetActive(false);} 
                if (child.CompareTag("Alt Model")) { child.gameObject.SetActive(true);} 
         }
        }
        else{
            foreach (Transform child in gameObject.transform) {
                if (child.CompareTag("Default Model")) { child.gameObject.SetActive(true);} 
                if (child.CompareTag("Alt Model")) { child.gameObject.SetActive(false);} 
            }
            if (m == Mode.Horizonal){
                this.transform.eulerAngles = new Vector3(0, 0, 0);
            }
                else {
                    this.transform.eulerAngles = new Vector3(0, 90, 0);
                }
            }
    }

    private void OnDestroy() {
        grid.updateWallsAroundTile(null, x, y);
    }
}

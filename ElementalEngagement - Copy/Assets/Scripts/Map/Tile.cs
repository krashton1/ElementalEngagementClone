using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    Structure structure;
    bool empty = true;
    public int x, y;
    
    public Tile(int x_, int y_){
        x = x_;
        y = y_;
    }

    public bool isEmpty(){
        return empty;
    }

    public Structure getStructure(){
        if (!empty) return structure;
        else return null;
    }

    public void setStructure(Structure s){
        structure = s;
        empty = false;
    }
}

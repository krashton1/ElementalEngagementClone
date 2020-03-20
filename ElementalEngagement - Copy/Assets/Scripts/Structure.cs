using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity
{
    public Vector2 dimensions;
    public string type;
    Vector2 gridPosition;

    public void setGridPosition(Vector2 pos){
        gridPosition = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObj : MonoBehaviour
{
    private int cx;
    private int cy;
    private int tx;
    private int ty;

    public void SetCoords(int cx, int cy, int tx, int ty)
    {
        this.cx = cx;
        this.cy = cy;
        this.tx = tx;
        this.ty = ty;
    }

    public (int cx, int cy, int tx, int ty) GetCoordinates()
    {
        return (cx, cy, tx, ty);
    }

    public void Destroy()
    {
        Destroy(gameObject);
        //Debug.Log("destroy");
    }
}

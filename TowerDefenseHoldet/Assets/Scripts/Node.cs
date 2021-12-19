using System;
using UnityEngine;

/*
En class der skal bruges af A* algoritmen
Hver MapTile skal have en node forbundet til sig
*/

public class Node
{

    //nodens position på banen
    public GraphPoint GrafPosition { get; private set; }

    public TileInfo TileReference { get; private set; }

    //bruges til at refererer til den MapTile som noden er forbundet til
    public Vector2 MapPosition { get; set; }

    //bruges af A* til at finde gå tilbage når en sti er blevet fundet
    public Node Parent { get; private set; }

    public int ValueG { get; set; }

    public int ValueH { get; set; }

    public int ValueF { get; set; }

    //contructor for at sætte den MapTile som noden er forbundet til
    public Node(TileInfo tileReference)
    {
        this.TileReference = tileReference;
        this.GrafPosition = tileReference.TileDict;
        this.MapPosition = tileReference.cameraPos;
    }

    //vores AStar algoritme skal kunne beregne værdier på hvert enkelt
    public void CostCalculation(Node parent, Node goal, int gCost)
    {
        this.Parent = parent;
        this.ValueG = parent.ValueG + gCost;
        this.ValueH = (Math.Abs(GrafPosition.X - goal.GrafPosition.X) + Math.Abs(goal.GrafPosition.Y - GrafPosition.Y)) * 10;
        this.ValueF = ValueG + ValueH;
    }
}
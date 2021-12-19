using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
Indeholder A*-algoritmen
*/

public static class AStar
{

    //metode som laver nodes til alle spillets maptiles
    private static void GenerateNodes()
    {
        //instantiator dictionary
        tileDict = new Dictionary<GraphPoint, Node>();
     
        //for hver tile i spillet skal den tildeles en node  
     
        foreach (TileInfo item in MapGenerator.Instance.TileDict.Values)
        {
            tileDict.Add(item.TileDict, new Node(item));
        }
        
    }

    //vores algoritme skal returnere en stack med vejen 
    public static Stack<Node> GetRoute(GraphPoint startNode, GraphPoint endNode)
    {

        //hvis der ikke er nogen nodes til tiles skal de laves
        if (tileDict == null)
        {
            GenerateNodes();
        }

        //laver en open list
        HashSet<Node> openList = new HashSet<Node>();

        //laver en closed list
        HashSet<Node> closedList = new HashSet<Node>();

        //laver en stack med den endelige vej for critters
        Stack<Node> finalCritterRoute = new Stack<Node>();

        //til at starte med skal nodeCurrent v�re = med startNoden
        Node nodeCurrent = tileDict[startNode];

        //tilf�jer nodeCurrent til open list efter den er unders�gt
        openList.Add(nodeCurrent);

        //det her loop skal blive ved med at s�ge efter en vej indtil der ikke l�ngere er nodes i den �bne liste
        while (openList.Count > 0)
        {
            //to forloops for at finde alle omkringliggende nodes omkring den nuv�rende node
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //viser i console om vi f�r alle omkringliggende valgmuligheder
                    //Debug.Log(x + ", " + y);

                    //gemmer positionen for current nodeNeighborPositionen
                    GraphPoint nodeNeighbourPosition = new GraphPoint(nodeCurrent.GrafPosition.X - x, nodeCurrent.GrafPosition.Y - y);
                    
                    //viser i console om vi f�r positionerne
                    //Debug.Log(nodeNeighbourPosition.X + " " + nodeNeighbourPosition.Y);
                    
                    //nodeNeighborPositionen skal v�re inde p� banen og m� ikke v�re start eller nuv�rende  
                    if (MapGenerator.Instance.InsideMap(nodeNeighbourPosition) && MapGenerator.Instance.TileDict[nodeNeighbourPosition].IsWalkable && nodeNeighbourPosition != nodeCurrent.GrafPosition)
                    {  
                        //for at v�re sikre p� at vi f�r en v�rdi senere s�tter vi gCost = 0
                        int gValueCost = 0;

                        //hvis noden er vertikal eller horisonal placeret
                        if (Math.Abs(x - y) == 1)
                        {
                            //s� skal noden v�re = 10
                            gValueCost = 10;
                        }
                        else
                        {   
                            //hvis noden er placeret diagonalt
                            if (!AreNodesDiagonallyConnected(nodeCurrent, tileDict[nodeNeighbourPosition]))
                            {
                                continue;
                            }
                            //s�ttes de til 14
                            gValueCost = 14;
                        }

                        //laver reference til noden
                        Node nodeNeighbour = tileDict[nodeNeighbourPosition];

                        //hvis nodeNeighbor eksisterer i den �bne liste
                        if (openList.Contains(nodeNeighbour))
                        {
                            //s� skal vi tjekke om den her node er en bedre parent
                            if (nodeCurrent.ValueG + gValueCost < nodeNeighbour.ValueG)
                            {
                                nodeNeighbour.CostCalculation(nodeCurrent, tileDict[endNode], gValueCost);
                            }
                        }
                        //hvis nodeNeighbour ikke eksisterer i den lukkede liste
                        else if (!closedList.Contains(nodeNeighbour))
                        {
                            //tilf�jer nodeNeighbor til den �bne liste
                            openList.Add(nodeNeighbour);
                            //udregning af nodens v�rdi
                            nodeNeighbour.CostCalculation(nodeCurrent, tileDict[endNode], gValueCost);
                        }
                    }
                }
            }

            //flytter nuv�rende node fra open til closed list
            openList.Remove(nodeCurrent);
            closedList.Add(nodeCurrent);

            if (openList.Count > 0)
            {
                //sorterer listen med v�rdien f v�rdi og v�lger den f�rste p� liste
                nodeCurrent = openList.OrderBy(n => n.ValueF).First();
            }
            
            //n�r den nuv�rende node er endNoden s� har vi fundet vejen
            if (nodeCurrent == tileDict[endNode])
            {
                //pusher vores nodes indtil vejen er dannet
                while (nodeCurrent.GrafPosition != startNode)
                {
                    //pusher nuv�rende node til vejen
                    finalCritterRoute.Push(nodeCurrent);
                    //finder nodens Parent ved at l�be tilbage igennem stacken (finalCritterRoute)
                    nodeCurrent = nodeCurrent.Parent;
                }

                break;
            }
        }
        bool gg= false;
        //Det her er kun til debugging - vigtigt at "AStarVisualizer" skrives pr�cis som det er skrevet i Hierarchy 
        if(gg == false) { 
        GameObject.Find("AStarVisualizer").GetComponent<AStarVisualizer>().ColorTheTiles(openList, closedList, finalCritterRoute);
           
        }
        
        return finalCritterRoute;
    }
    
    //boolean der returnerer om to nodes er ved sidan af hinanden diagonalt uden at noget blokere stien
    private static bool AreNodesDiagonallyConnected (Node nodeCurrent, Node nodeNeighbor)
    {
        //f�rst skal vi bruge retningen
        GraphPoint grafDirection = nodeNeighbor.GrafPosition - nodeCurrent.GrafPosition;

        //s� skal vi bruge nodernes positionerne
        GraphPoint firstPos = new GraphPoint(nodeCurrent.GrafPosition.X + grafDirection.X, nodeCurrent.GrafPosition.Y);

        GraphPoint secondPos = new GraphPoint(nodeCurrent.GrafPosition.X, nodeCurrent.GrafPosition.Y + grafDirection.Y);

        //hvis node er inde i grid eksisterer det og hvis vores tile inde i levelmanager ikke er walkable
        if (MapGenerator.Instance.InsideMap(firstPos) && !MapGenerator.Instance.TileDict[firstPos].IsWalkable)
        {
            return false;
        }
        if (MapGenerator.Instance.InsideMap(secondPos) && !MapGenerator.Instance.TileDict[secondPos].IsWalkable)
        {
            return false;
        }

        return true;
    }

    //dictionary med information om alle spillets maptiles
    private static Dictionary<GraphPoint, Node> tileDict;

}
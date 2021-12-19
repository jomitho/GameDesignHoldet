using System;
using System.Collections.Generic;
using UnityEngine;

/*
Den her class skal genere vores bane i spillet
*/

public class MapGenerator : Singleton<MapGenerator>
{

    //laver property for at f� vores MapTiles st�rrelse
    public float getSizeOfTile
    {
        get { return mapTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    //inds�tter prefabs af GrassTile og PathTile i inspektoren
    [SerializeField]
    private GameObject[] mapTiles;

    //inds�tter objekt af samme navn i inspektor-feltet som alle MapTiles s� kan havne i n�r de er generet
    [SerializeField]
    private Transform mapTile;

    //metode for at instansiere vores bane i spillet
    //henter fra flere metoder l�ngere nede i scriptet!
    private void InstantiateMap()
    {
        //instansierer vores dictionary
        TileDict = new Dictionary<GraphPoint, TileInfo>();

        //et array af strings er lig med metoden for at hente vores data fra tekstfil
        string[] mapTileInfo = MapGenerationText();

        //s�tter banens st�rrelse baseret p� dens data
        sizeOfMap = new GraphPoint(mapTileInfo[0].ToCharArray().Length, mapTileInfo.Length);

        //s�tter banens MapTiles
        int mapPosX = mapTileInfo[0].ToCharArray().Length;
        int mapPosY = mapTileInfo.Length;

        //indstiller hvor p� vores Main Camera vores bane skal placeres
        Vector3 cameraPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        //for loops for for at placere felterne p� banen
        for (int y = 0; y < mapPosY; y++)
        {
            char[] newMapTiles = mapTileInfo[y].ToCharArray();
            for (int x = 0; x < mapPosX; x++)
            {
                //placer vores MapTiles
                CreateMapTile(newMapTiles[x].ToString(), x, y, cameraPos, true);
            }
        }
        //placer ogs� vores start- og slutpunkt
        StartAndEnd();
    }

    //i starten af spillet skal metoden for at instansiere vores bane eksekveres
    void Start()
    {
        InstantiateMap();
    }

    //deklarerer at start- og slutpunkt skal have koordinater p� banen
    private GraphPoint startTile;
    private GraphPoint endTile;

    //inds�tter vores prefab for startpunktet her
    [SerializeField]
    private GameObject startObject;

    //inds�tter vores prefab for slutpunktet her
    [SerializeField]
    private GameObject endObject;

    //laver property til startpunktet s� det kan tilg�s til spawning
    public StartTile StartTileObject { get; set; }

    //banens st�rrelse
    private GraphPoint sizeOfMap;

    //vi skal bruge den endelige rute
    private Stack<Node> finalRoute;

    public Stack<Node> Route
    {
        get
        {
            if (finalRoute == null)
            {
                CreateRoute();
            }

            //for at undg� at Enemy genbruger stacks
            return new Stack<Node>(new Stack<Node>(finalRoute));
        }
    }

    //laver dictionary af alle spillets MapTiles
    public Dictionary<GraphPoint,TileInfo> TileDict { get; set; }

    //metoden for at lave vores felter eksekveres oppe i vores InstantiateMap-metode
    private void CreateMapTile(string tilePrefab, int x, int y, Vector3 cameraPos, bool IsWalkable)
    {
        //skal det index fra hvor MapTiles skal laves
        int indexOfTile = int.Parse(tilePrefab);

        //laver en ny MapTile
        TileInfo newTile = Instantiate(mapTiles[indexOfTile]).GetComponent<TileInfo>();
        
        //s�tter properties til vores MapTiles
        if (indexOfTile == 0)
        {
            newTile.SetMapTileProperties(new GraphPoint(x, y), new Vector3(cameraPos.x + (getSizeOfTile * x), cameraPos.y - (getSizeOfTile * y), 0), mapTile, false, true);
        }
        else
        {
            newTile.SetMapTileProperties(new GraphPoint(x, y), new Vector3(cameraPos.x + (getSizeOfTile * x), cameraPos.y - (getSizeOfTile * y), 0), mapTile, true, false);
        }
    }

    /*
    
    Herfra loader vi vores tekstfiler fra mappen Resources for at lave vores bane:

    Level1 = DebugLevel hvor alle felter er path undtagen et par enkelte felter med gr�s
    Level2 = En labyrant-lignende bane for at fors�ge at g�re banen s� kompliceret som muligt
    Level3 = En vej hvor der ikke er PathTiles hen til slutpunktet
    Level4 = En bane tilt�nkt til hvordan den kunne se ud i et tower defense spil

    */

    private string[] MapGenerationText()
    {
        //inds�t navnet p� den level der skal testes
        TextAsset readFile = Resources.Load("Level4") as TextAsset;

        string mapText = readFile.text.Replace(Environment.NewLine, string.Empty);

        return mapText.Split('/');
    }

    //metode for at instantiere vores start- og slutpunkt
    private void StartAndEnd()
    {

        System.Random rd = new System.Random();
        int rand_num = rd.Next(0, 2);
        int rand_numX = rd.Next(0, 9);
        int rand_num1 = rd.Next(0, 9);
      
        
        startTile = new GraphPoint(1, 0);
        
        while(TileDict[startTile].GetComponent<TileInfo>().IsWalkable == false)
        {
            rand_num = rd.Next(0, 2);
            rand_numX = rd.Next(0, 9);
            startTile = new GraphPoint(rand_numX, rand_num);
        }
     
        endTile = new GraphPoint(rand_num1, 9);
        while (TileDict[endTile].GetComponent<TileInfo>().IsWalkable == false)
        {
            rand_num1 = rd.Next(0, 9);
            endTile = new GraphPoint(rand_num1, 9);
        }


        GameObject objectVar = (GameObject) Instantiate(startObject, TileDict[startTile].GetComponent<TileInfo>().cameraPos, Quaternion.identity);
        StartTileObject = objectVar.GetComponent<StartTile>();
        StartTileObject.name = "StartTile";

        Instantiate(endObject, TileDict[endTile].GetComponent<TileInfo>().cameraPos, Quaternion.identity);
    }

    //metode der henter den vej vores AStar algoritme har fundet
    public void CreateRoute()
    {
        finalRoute = AStar.GetRoute(startTile, endTile);
    }

    //boolean for rent faktisk at tjekke om felterne er inde p� banen
    public bool InsideMap(GraphPoint tilePos)
    {
        return tilePos.X >= 0 && tilePos.Y >= 0 && tilePos.X < sizeOfMap.X && tilePos.Y < sizeOfMap.Y;
    }
}
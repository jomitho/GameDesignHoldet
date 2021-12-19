using System.Collections.Generic;
using UnityEngine;

/*
Uafh�ngig class til debugging og er ikke en del af spillet 
*/

public class AStarVisualizer : MonoBehaviour
{
    //felter for start- og slutpunkt, men skal IKKE udfyldes
    //bliver udfyldt senere nede i RightClik-metoden,
    //hvor f�rste whiteTile bliver farvet bl� og n�ste r�d (visualisering af start- og slutpunkt) 
    [SerializeField]
    private TileInfo start, end;

    //inds�t pil her for at visualisere hvor dens parent er
    [SerializeField]
    private GameObject pointerToParent;

    //inds�t hvid tile her for senere at farve den for visualisering af A*
    [SerializeField]
    private GameObject whiteTile;

    //bruger update fordi den hele tiden vil checke (once per frame)
    void Update()
    {
        RightClick();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.GetRoute(start.TileDict, end.TileDict);
        }
    }

    public void ColorTheTiles(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path)
    {
        foreach (Node item in openList)
        {
            if (item.TileReference != start && item.TileReference != end)
            {
                generateWhiteTile(item.TileReference.cameraPos, Color.magenta, item);
            }

            PointerToParent(item, item.TileReference.cameraPos);
        }
        foreach (Node item in closedList)
        {
            if (item.TileReference != start && item.TileReference != end && !path.Contains(item))
            {
                generateWhiteTile(item.TileReference.cameraPos, Color.magenta, item);
            }

            PointerToParent(item, item.TileReference.cameraPos);
        }

        foreach (Node item in path)
        {
            if (item.TileReference != start && item.TileReference != end)
            {
                generateWhiteTile(item.TileReference.cameraPos, Color.yellow, item);
            }
        }
    }

    //laver metode der styrer hvilken retning vores pil skal pege p�
    //for at vise hvem den p�g�ldende nodes parent er
    private void PointerToParent(Node nodeInfo, Vector2 posInfo)
    {
        if (nodeInfo.Parent != null)
        {
            GameObject arrowPointer = (GameObject)Instantiate(pointerToParent, posInfo, Quaternion.identity);
            
            arrowPointer.GetComponent<SpriteRenderer>().sortingOrder = 3;

            //op
            if ((nodeInfo.GrafPosition.X == nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y > nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            //h�jre
            else if ((nodeInfo.GrafPosition.X < nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y == nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            //nede i til h�jre
            else if ((nodeInfo.GrafPosition.X < nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y < nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 315);
            }
            //oppe til h�jre
            else if ((nodeInfo.GrafPosition.X < nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y > nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            
            //oppe til venstre
            else if ((nodeInfo.GrafPosition.X > nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y > nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            //nede til venstre
            else if ((nodeInfo.GrafPosition.X > nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y < nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            //venstre
            else if ((nodeInfo.GrafPosition.X > nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y == nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 180);
            }
          
            //nede
            else if ((nodeInfo.GrafPosition.X == nodeInfo.Parent.GrafPosition.X) && (nodeInfo.GrafPosition.Y < nodeInfo.Parent.GrafPosition.Y))
            {
                arrowPointer.transform.eulerAngles = new Vector3(0, 0, 270);
            } 
        }
    }
       
    //generer en whiteTile, som skal farves og p� den m�de visualisere AStar algoritmens vej
    private void generateWhiteTile(Vector3 worldPos, Color32 newColor, Node nodeInfo = null)
    {
        
        GameObject whiteTileVariable = (GameObject)Instantiate(whiteTile,worldPos,Quaternion.identity);
      
        if (nodeInfo != null)
        {
            Visualizer visualizerVariable = whiteTileVariable.GetComponent<Visualizer>();

            visualizerVariable.G.text += nodeInfo.ValueG;
            visualizerVariable.H.text += nodeInfo.ValueH;
            visualizerVariable.F.text += nodeInfo.ValueF;
        }

        whiteTileVariable.GetComponent<SpriteRenderer>().color = newColor;
    }
      
    

    //f�rste h�jreklik giver et startpunkt og andet et slutpunkt
    //klik derefter space og s� skal vejen for AStar visualiseres
    //klik IKKE space f�r de to h�jreklik
    public void RightClick()
    {
        //1 = h�jre klik p� musen
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D detectedHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (detectedHit.collider != null)
            {
                TileInfo tileInfoVariable = detectedHit.collider.GetComponent<TileInfo>();

                if (tileInfoVariable != null)
                {
                    if (start == null)
                    {
                        start = tileInfoVariable;
                        float XX = detectedHit.point.x;
                        float YY = detectedHit.point.y;
                        int XXX = (int)Mathf.Round(XX);
                        int YYY = (int)Mathf.Round(YY);
                        generateWhiteTile(start.cameraPos, new Color32(0, 121, 254, 255));
                    }
                    else if (end == null)
                    {
                        end = tileInfoVariable;
                        generateWhiteTile(end.cameraPos, new Color32(249, 0, 0, 255));

                    }
                }
            }
        }
    }
}
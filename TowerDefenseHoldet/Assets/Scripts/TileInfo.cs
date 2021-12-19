using UnityEngine;
using UnityEngine.EventSystems;

/*
Det her script lægges ovenpå på vores prefabs for felterne på banen 
*/

public class TileInfo : MonoBehaviour
{
    void Start()
    {
        //der skal være en reference til vores spriterenderer
        spriteVisualizer = GetComponent<SpriteRenderer>();
    }

    //felternes position på banen
    public GraphPoint TileDict { get; set; }

    //boolean tjekker om der er noget på feltet
    public bool IsEmpty { get; set; }

    //refererer til felternes sprite
    
    private SpriteRenderer spriteVisualizer;

    //laver properties så det kan tilgås om noget er walkable og om noget bliver debugget
    [SerializeField]
    public bool IsWalkable { get; set; }
    public bool IsDebugging { get; set; }

    //felternes position i spillet skal sættes
    public Vector2 cameraPos
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    //metode for at sætte alt nødvendig information om vores MapTiles
    public void SetMapTileProperties(GraphPoint mapPosition, Vector3 gamePos, Transform parent, bool walk, bool empty)
    {
        
        IsWalkable = walk;
        IsEmpty = empty;
        this.TileDict = mapPosition;
        transform.position = gamePos;
        transform.SetParent(parent);
        
        MapGenerator.Instance.TileDict.Add(mapPosition, this);

    }

    private void BuildTower()
    {
        if (GetComponentInParent<TileInfo>().IsEmpty == true)
        {
            if (GetComponentInParent<TileInfo>().IsWalkable == false)
            {
                GameObject buildTower = Instantiate(GameMaster.Instance.ButtonPressed.Tower, transform.position, Quaternion.identity);

                //lad os sørge for at sorterer layer af towers:
                buildTower.GetComponent<SpriteRenderer>().sortingOrder = TileDict.Y;

                //det her sørger for at det Tower der blir placeret bliver child af den tile det står på
                buildTower.transform.SetParent(transform);

                GameMaster.Instance.Purchase();
                GetComponentInParent<TileInfo>().IsEmpty = false;
            }
        }
    }
    //OnMouseOver bliver kaldt hver frame
    private void OnMouseOver()
    {

        if (!EventSystem.current.IsPointerOverGameObject() && GameMaster.Instance.ButtonPressed != null) { 
        
            if (Input.GetMouseButtonDown(0))
            {
                BuildTower();
            }
        }
    }
}
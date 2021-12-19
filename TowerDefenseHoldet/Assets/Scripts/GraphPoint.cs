/*
Vi bruger en struct for hele tiden at vide på hvilken felter vi befinder os på
Struct er som value type (ikke en reference type) god fordi den ikke kan ændre sig
og fordi den er involveret i mange scripts
Det får vores A* også brug for
*/

public struct GraphPoint
{
    //laver properties for vores x og y koordinater på banen
    public int X { get; set; }
    public int Y { get; set; }

    //constructor for vores x og y koordinater på banen
    public GraphPoint(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    //laver de her operators for at kunne sammenligne koordinater

    public static bool operator == (GraphPoint first, GraphPoint second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    public static bool operator != (GraphPoint first, GraphPoint second)
    {
        return first.X != second.X || first.Y != second.Y;
    }

    public static GraphPoint operator - (GraphPoint x, GraphPoint y)
    {
        return new GraphPoint(x.X - y.X, x.Y - y.Y);
    }
}
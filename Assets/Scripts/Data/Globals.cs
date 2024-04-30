using System.Collections.Generic;
public class Globals
{
    public static int TERRAIN_LAYER_MASK = 1 << 6;

    public static BuildingData[] BUILDING_DATA = new BuildingData[]
    {
        //Demo Building
        new BuildingData(
            //Name
            "Building",
            //HP
            100,
            //Cost
            new Dictionary<string, int>()
            {
                {"gold", 100 }
            }
            ),

        //Demo Tower
        new BuildingData(
            //Name
            "Tower",
            //HP
            150,
            //Cost
            new Dictionary<string, int>()
            {
                {"gold", 50 }
            }
            )
    };

    public static Dictionary<string, GameResource> GAME_RESOURCES =
        new Dictionary<string, GameResource>()
    {
        {"gold", new GameResource("Gold", 500) },
    };
}

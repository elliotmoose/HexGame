using UnityEngine;

public class ShopItem 
{
    public Identifiers id;
    public string title;
    public string description;
    public float price;
    public GameObject prefab;

    public ShopItem(Identifiers id, string title, string description, float price, GameObject prefab)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.price = price;
        this.prefab = prefab;
    }

    public ShopItem() 
    {

    }

    public static ShopItem StonePlatform() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.STONE_PLATFORM;        
        item.title = "Stone";
        item.description = "A block of stone, let’s you build buildings";
        item.price = 500;
        item.prefab = PrefabManager.GetInstance().stoneDisplay;                        
        return item;
    }
    
    public static ShopItem SoilPlatform() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.SOIL_PLATFORM;
        item.title = "Soil";
        item.description = "A block of soil, let's you grow things";
        item.price = 300;
        item.prefab = PrefabManager.GetInstance().soilDisplay;                
        return item;
    }

    public static ShopItem MiningPlatform() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.MINING_PLATFORM;
        item.title = "Mineral Miner";
        item.description = "Mines minerals and generates income.";
        item.price = 500;
        item.prefab = PrefabManager.GetInstance().miningDisplay;                
        return item;
    }
 
    public static ShopItem DigSitePlatform() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.DIG_SITE_PLATFORM;
        item.title = "Digging Site";
        item.description = "Demarcated area for digging";
        item.price = 500;
        item.prefab = PrefabManager.GetInstance().digSiteDisplay;                
        return item;
    }

    #region buildings

    public static ShopItem Tree() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.TREE_BUILDING;
        item.title = "Tree";
        item.description = "Nature's miracle. Converts CO2 into Oxygen, but burns easily";
        item.price = 900;
        item.prefab = PrefabManager.GetInstance().treeDisplay;                
        return item;
    }

    public static ShopItem Condenser() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.CONDENSER_BUILDING;
        item.title = "Condenser";
        item.description = "Produces water";
        item.price = 900;
        item.prefab = PrefabManager.GetInstance().condenserDisplay;                
        return item;
    }

    public static ShopItem LightSource() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.LIGHTSOURCE_BUILDING;
        item.title = "Light Source";
        item.description = "Help's trees grow";
        item.price = 900;
        item.prefab = PrefabManager.GetInstance().lightsourceDisplay;                
        return item;
    }

    public static ShopItem Generator() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.GENERATOR_BUILDING;
        item.title = "Generator";
        item.description = "Generates energy for nearby platforms";
        item.price = 900;
        item.prefab = PrefabManager.GetInstance().generatorDisplay;                
        return item;
    }

    public static ShopItem Turbine() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.TURBINE_BUILDING;
        item.title = "Wind Turbine";
        item.description = "Generates energy for nearby platforms";
        item.price = 500;
        item.prefab = PrefabManager.GetInstance().turbineDisplay;                
        return item;
    }

    public static ShopItem MineralMiner() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.MINERAL_MINER_BUILDING;
        item.title = "Mineral Miner";
        item.description = "Digs the ground for minerals";
        item.price = 500;
        item.prefab = PrefabManager.GetInstance().mineralMiner;                
        return item;
    }

    public static ShopItem OilPump() 
    {
        ShopItem item = new ShopItem();
        item.id = Identifiers.OIL_PUMP_BUILDING;
        item.title = "Oil Pump";
        item.description = "Digs the ground for oil";
        item.price = 500;
        item.prefab = PrefabManager.GetInstance().oilPump;                
        return item;
    }

    #endregion
}
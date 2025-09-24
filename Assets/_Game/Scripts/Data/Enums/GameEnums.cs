namespace RTS.Data
{
    public enum Faction
    {
        USA,
        Russia
    }

    public enum UnitType
    {
        Infantry,
        LightVehicle,
        HeavyVehicle,
        Artillery,
        Aircraft,
        Helicopter,
        Naval
    }

    public enum ArmorType
    {
        None,
        Light,
        Medium,
        Heavy,
        Reactive,
        Composite
    }

    public enum DamageType
    {
        Kinetic,        // Bullets, tank shells
        HighExplosive,  // HE rounds, grenades
        HEAT,           // Shaped charges
        Fragmentation,  // Artillery, bombs
        Incendiary      // Fire damage
    }

    public enum BuildingType
    {
        CommandCenter,
        Production,
        Defense,
        Support,
        Resource,
        Superweapon
    }

    public enum TechLevel
    {
        Tier1,
        Tier2,
        Tier3
    }

    // For future command system
    public enum CommandType
    {
        Move,
        Attack,
        AttackMove,
        Stop,
        HoldPosition,
        Patrol,
        Follow,
        Build,
        Repair,
        Gather
    }

    // For fog of war
    public enum VisibilityState
    {
        Hidden,         // Never seen
        Fogged,         // Previously seen, not currently visible
        Visible         // Currently in line of sight
    }
}
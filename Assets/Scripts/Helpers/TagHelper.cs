public static class RoomName
{
    public const string Start = "Start";
    public const string Empty = "Empty";
    public const string Enemy = "Enemy";
    public const string Treasure = "Treasure";
    public const string Shop = "Shop";
    public const string Ritual = "Ritual";
    public const string Boss = "Boss";

    public static string GetRoomName(string roomType, int level, int variant)
    {
        return $"Level{level}_{roomType}_{variant}";
    }
}

public static class TagName
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Wall = "Wall";
}
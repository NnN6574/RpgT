using UnityEngine;

public class ValueConverter
{
    public static float ChangeValueByRange(float value, Vector2 from, Vector2 to)
    {
        return to.x + (value - from.x) * (to.y - to.x) / (from.y - from.x);
    }
}

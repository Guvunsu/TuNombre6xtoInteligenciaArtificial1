using System.Collections.Generic;

public class WorldState : Dictionary<string, object>
{
    public int GetInt(string key, int defaultValue = 0)
    {
        if (!ContainsKey(key) || this[key] == null) return defaultValue;

        object v = this[key];
        if (v is int i) return i;
        if (v is float f) return (int)f;
        if (v is bool b) return b ? 1 : 0;

        if (int.TryParse(v.ToString(), out int parsed))
            return parsed;

        return defaultValue;
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        if (!ContainsKey(key) || this[key] == null) return defaultValue;

        object v = this[key];
        if (v is bool b) return b;
        if (v is int i) return i != 0;

        if (bool.TryParse(v.ToString(), out bool parsed))
            return parsed;

        return defaultValue;
    }
}
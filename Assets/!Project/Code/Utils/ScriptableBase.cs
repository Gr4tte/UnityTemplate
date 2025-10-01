using UnityEngine;

public class ScriptableBase : ScriptableObject
{
    public int ID => GetInstanceID();

    [field: SerializeField] public string Name { get; private set; }

    public static bool operator ==(ScriptableBase a, ScriptableBase b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        return a.ID == b.ID;
    }

    public static bool operator !=(ScriptableBase a, ScriptableBase b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is ScriptableBase item)
        {
            return this == item;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }
}

using UnityEngine;

public class DisplayNameAttribute : PropertyAttribute
{
    public string m_displayName;

    public DisplayNameAttribute(string displayName)
    {
        this.m_displayName = displayName;
    }
}
public class ReadOnlyAttribute : PropertyAttribute { }

public class ReorderableListAttribute : PropertyAttribute { }


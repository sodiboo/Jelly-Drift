using System;

[AttributeUsage(AttributeTargets.Class)]
public class ChildEffectAttribute : Attribute
{
    private readonly string id;
    private readonly string name;
    public ChildEffectAttribute(string id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public string Id => id;
    public string Name => name;
}

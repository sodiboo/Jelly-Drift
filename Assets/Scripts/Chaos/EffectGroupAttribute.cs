using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class EffectGroupAttribute : Attribute
{
    private string id;
    private string name;
    public EffectGroupAttribute(string id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public string Id => id;
    public string Name => name;
    public bool SeparateCheats = false;
}

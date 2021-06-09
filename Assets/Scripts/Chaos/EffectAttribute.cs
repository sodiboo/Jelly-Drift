using System.Collections;
using System.Collections.Generic;
using System;

[AttributeUsage(AttributeTargets.Class)]
public class EffectAttribute : Attribute
{
    private string name;
    private string id;
    private EffectInfo.Alignment alignment;
    public EffectAttribute(string id, string name, EffectInfo.Alignment alignment)
    {
        this.name = name;
        this.id = id;
        this.alignment = alignment;
    }
    public string Name => name;
    public string Id => id;
    public EffectInfo.Alignment Alignment => alignment;

}

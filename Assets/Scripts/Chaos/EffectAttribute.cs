using System.Collections;
using System.Collections.Generic;
using System;

[AttributeUsage(AttributeTargets.Class)]
public class EffectAttribute : Attribute
{
    private string name;
    private string id;
    public EffectAttribute(string id, string name)
    {
        this.name = name;
        this.id = id;
    }
    public string Name => name;
    public string Id => id;

}

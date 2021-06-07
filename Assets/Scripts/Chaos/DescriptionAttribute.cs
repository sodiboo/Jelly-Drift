using System;

[AttributeUsage(AttributeTargets.Class)]
public class DescriptionAttribute : Attribute
{
    private string value;
    public DescriptionAttribute(string description)
    {
        value = description;
    }
    public string Description => value;
}
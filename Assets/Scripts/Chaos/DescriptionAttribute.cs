using System;

[AttributeUsage(AttributeTargets.Class)]
public class DescriptionAttribute : Attribute
{
    private readonly string value;
    public DescriptionAttribute(string description) => value = description;
    public string Description => value;
}
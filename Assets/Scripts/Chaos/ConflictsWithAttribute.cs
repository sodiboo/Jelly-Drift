using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ConflictsWithAttribute : Attribute
{
    private readonly Type[] conflicts;
    public ConflictsWithAttribute(params Type[] conflicts) => this.conflicts = conflicts;

    public Type[] Conflicts => conflicts;
}

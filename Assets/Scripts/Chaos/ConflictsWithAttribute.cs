using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ConflictsWithAttribute : Attribute
{
    Type[] conflicts;
    public ConflictsWithAttribute(params Type[] conflicts)
    {
        this.conflicts = conflicts;
    }

    public Type[] Conflicts { get => conflicts; }
}

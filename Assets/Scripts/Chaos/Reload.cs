using System;

public sealed class Reload
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OnEnable : Attribute
    {
        private Type[] effects;
        public OnEnable(params Type[] effects)
        {
            this.effects = effects;
        }
        public Type[] Effects => effects;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class OnDisable : Attribute
    {
        private Type[] effects;
        public OnDisable(params Type[] effects)
        {
            this.effects = effects;
        }
        public Type[] Effects => effects;
    }
}

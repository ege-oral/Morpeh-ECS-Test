using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Components.Events
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DamageEvent : IComponent 
    {
        public Entity sourceEntity;
        public Entity targetEntity;
        public int damageAmount;
        public bool instantKill;
    }
}
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Components.Health
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HealthComponent : IComponent
    {
        public int currentHealth;
        public int maxHealth;
    }
}
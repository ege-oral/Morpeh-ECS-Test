using Components;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemyTagProvider : MonoProvider<EnemyTag> 
    {
    
    }
}
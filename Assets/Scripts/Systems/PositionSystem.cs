using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PositionSystem : ISystem 
    {
        public World World { get; set;}

        public void OnAwake() 
        {

        }

        public void OnUpdate(float deltaTime) 
        {

        }

        public void Dispose()
        {

        }
    }
}
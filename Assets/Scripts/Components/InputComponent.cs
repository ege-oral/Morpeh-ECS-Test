using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InputComponent : IComponent 
    { 
        public float horizontalInput;
        public float verticalInput;
    }
}
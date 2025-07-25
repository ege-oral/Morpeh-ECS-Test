using Scellecs.Morpeh;
using Systems;
using UnityEngine;

public class Startup : MonoBehaviour
{
    private World _world;
    private void Start()
    {
        _world = World.Default;
        var systemGroup = _world.CreateSystemsGroup();
        systemGroup.AddSystem(new HealthSystem());
        systemGroup.AddSystem(new InputSystem());
        systemGroup.Initialize();
        
        _world.AddSystemsGroup(order: 0, systemGroup);
    }
}

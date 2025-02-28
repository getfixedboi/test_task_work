using UnityEngine;
using Zenject;

public class MovementInstaller : MonoInstaller
{
    [SerializeField] private FloatingJoystick _floatJoystick;
    public override void InstallBindings()
    {
        Container.Bind<FloatingJoystick>().FromInstance(_floatJoystick).AsSingle();
    }
}
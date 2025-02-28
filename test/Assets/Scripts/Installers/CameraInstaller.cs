using UnityEngine;
using Zenject;

public interface ICameraController
{
    void RotateCamera(float x, float y);
}

public class CameraInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<ICameraController>().To<PlayerCameraMovement>().FromNewComponentOnNewGameObject().AsSingle();
    }
}

using Injecta;
using UnityEngine;

public class SceneInstallerTest : MonoInstaller
{
    public override void InstallBindings()
    {
        SceneManager sceneManager = FindFirstObjectByType<SceneManager>();
        Debug.Log(Container);
        Container.BindInstance(sceneManager);
    }
}

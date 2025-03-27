using Injecta;
using UnityEngine;

public class InjectPrefabTest : MonoBehaviour
{
    [Inject]
    private SceneManager sceneManager;

    void Start()
    {
        sceneManager.Add();
    }
}

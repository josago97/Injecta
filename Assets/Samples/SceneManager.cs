using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private int _counter;

    public void Add()
    {
        _counter++;
    }
}

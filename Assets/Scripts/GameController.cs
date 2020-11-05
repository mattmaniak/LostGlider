using UnityEngine;

sealed class GameController : MonoBehaviour
{
    void Start()
    {
        Level.Generator.Instance.Start();
    }

    void Update()
    {
        Level.Generator.Instance.Update();
    }
}

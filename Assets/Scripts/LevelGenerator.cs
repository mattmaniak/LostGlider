#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    List<Sprite> groundsPool = new List<Sprite>();

    void Start()
    {
        const int filesNumber = 2;
        Sprite loadedSprite;
        string spriteFilename;

        for (int i = 0; i < filesNumber; i++)
        {
            spriteFilename = "Sprites/ground" + i + ".psd";
            loadedSprite = Resources.Load<Sprite>(spriteFilename);

            if (loadedSprite == null)
            {
#if DEBUG
                Debug.Log(GetType().Name +
                          " initialization aborted. Unable to load: "
                          + spriteFilename);
#endif
                return;
            }
            groundsPool.Add(loadedSprite);
        }
    }

    void Update()
    {
        
    }
}

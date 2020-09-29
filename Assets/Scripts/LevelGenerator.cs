#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject groundPrefab;

    List<GameObject> groundsPool = new List<GameObject>();

    void Start()
    {
        const int spritesNumber = 2;
        Sprite loadedSprite;
        string spriteBasename;

        for (int i = 0; i < spritesNumber; i++)
        {
            spriteBasename = "Sprites/Level/Grounds/ground" + i;
            loadedSprite = Resources.Load<Sprite>(spriteBasename);

            if (loadedSprite == null)
            {
#if DEBUG
                Debug.Log(GetType().Name +
                          " initialization aborted. Unable to load: "
                          + spriteBasename);
#endif
                return;
            }
            groundsPool.Add(Instantiate(groundPrefab));
            groundsPool[i].GetComponent<SpriteRenderer>().sprite = loadedSprite;

            groundsPool[i].GetComponent<BoxCollider2D>().offset
                = groundsPool[i].GetComponent<SpriteRenderer>().
                  sprite.bounds.center;
            
            groundsPool[i].GetComponent<BoxCollider2D>().size
                = groundsPool[i].GetComponent<SpriteRenderer>().
                  sprite.bounds.size;
        }
        // TODO: TEMPONARY POSITION.
        groundsPool[0].transform.Translate(Vector3.left * 2.0f) ;
        groundsPool[1].transform.Translate(Vector3.right * 2.0f) ;
    }
}

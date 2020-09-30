#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject groundPrefab;

    [SerializeField]
    Transform playerTransform;

    List<GameObject> groundsPool = new List<GameObject>();
    float groundWidth;
    float nextGroundTransitionX;
    float rightScreenEdgeX;

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
                = groundsPool[i].GetComponent<SpriteRenderer>().sprite.bounds.
                  center;
            
            groundsPool[i].GetComponent<BoxCollider2D>().size = groundsPool[i].
                GetComponent<SpriteRenderer>().sprite.bounds.size;

            groundsPool[i].transform.position = new Vector2(0.0f, -1.0f);
        }
        groundWidth = groundsPool[0].GetComponent<SpriteRenderer>().bounds.size.
                      x;

        rightScreenEdgeX = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, 0.0f, 0.0f)).x;

        groundsPool[0].GetComponent<SpriteRenderer>().transform.Translate(
            (groundWidth * 0.5f) - rightScreenEdgeX, 0.0f, 0.0f);
        groundsPool[1].GetComponent<SpriteRenderer>().transform.Translate(
            (groundWidth * 1.5f) - rightScreenEdgeX, 0.0f, 0.0f);

        nextGroundTransitionX = groundWidth * 1.5f;
    }

    void Update()
    {
        rightScreenEdgeX = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, 0.0f, 0.0f)).x;

        if (rightScreenEdgeX >= nextGroundTransitionX)
        {
            foreach (var ground in groundsPool)
            {
                ground.transform.Translate(groundWidth, 0.0f, 0.0f);
            }
            nextGroundTransitionX += groundWidth;
        }
    }
}

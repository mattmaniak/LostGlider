#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject groundChunkPrefab;

    [SerializeField]
    Transform playerTransform;

    List<GameObject> groundChunksPool = new List<GameObject>();
    float groundChunkWidth;
    float nextGroundChunkTransitionX;
    float rightScreenEdgeX;
    int currentGroundChunkIndex = 0;
    int nextGroundChunkIndex = 0;

    void Start()
    {
        const int spritesNumber = 2;

        Sprite loadedSprite;
        string spriteBasename;

        for (int i = 0; i < spritesNumber; i++)
        {
            spriteBasename = "Sprites/Level/Grounds/ground_chunk_" + i;
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
            groundChunksPool.Add(Instantiate(groundChunkPrefab));
            
            groundChunksPool[i].GetComponent<SpriteRenderer>().sprite
                = loadedSprite;

            groundChunksPool[i].GetComponent<BoxCollider2D>().offset
                = groundChunksPool[i].GetComponent<SpriteRenderer>().sprite.
                  bounds.center;
            
            groundChunksPool[i].GetComponent<BoxCollider2D>().size
                = groundChunksPool[i].GetComponent<SpriteRenderer>().sprite.
                  bounds.size;

            groundChunksPool[i].transform.position
                = new Vector2(0.0f, (groundChunksPool[i].transform.
                              GetComponent<SpriteRenderer>().sprite.bounds.size.
                              y / 2.0f) - 1.0f);
        }
        groundChunkWidth = groundChunksPool[0].GetComponent<SpriteRenderer>().
                           bounds.size.x;

        rightScreenEdgeX = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, 0.0f, 0.0f)).x;

        currentGroundChunkIndex = 0;
        nextGroundChunkIndex = 1;

        groundChunksPool[currentGroundChunkIndex].
            GetComponent<SpriteRenderer>().transform.Translate(
                (groundChunkWidth * 0.5f) - rightScreenEdgeX, 0.0f, 0.0f);

        groundChunksPool[nextGroundChunkIndex].GetComponent<SpriteRenderer>().
            transform.Translate((groundChunkWidth * 1.5f) - rightScreenEdgeX,
                                0.0f, 0.0f);

        nextGroundChunkTransitionX = rightScreenEdgeX = Camera.main.
            ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x
            + groundChunkWidth;
    }

    void Update()
    {
        int previousGroundChunkIndex;

        rightScreenEdgeX = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, 0.0f, 0.0f)).x;

        if (rightScreenEdgeX >= nextGroundChunkTransitionX)
        {
            // Swap...
            previousGroundChunkIndex = currentGroundChunkIndex;
            currentGroundChunkIndex = nextGroundChunkIndex;
            nextGroundChunkIndex = previousGroundChunkIndex;

            for (int i = 0; i < groundChunksPool.Count; i++)
            {
                if (i == nextGroundChunkIndex)
                {
                    groundChunksPool[i].transform.Translate(
                        groundChunkWidth* groundChunksPool.Count, 0.0f, 0.0f);                        
                }
            }
            nextGroundChunkTransitionX += groundChunkWidth;
        }
    }
}

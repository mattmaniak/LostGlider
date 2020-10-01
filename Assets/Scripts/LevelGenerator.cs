#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    const int spritesNumber = 3;
    readonly Vector2 invisiblePosition = new Vector2(-100.0f, -1.0f);

    [SerializeField]
    GameObject groundChunkPrefab;

    List<GameObject> groundChunksPool = new List<GameObject>();
    float groundChunkWidth;
    float nextGroundChunkTransitionX;
    float rightScreenEdgeX;
    int currentGroundChunkIndex = 0;
    int nextGroundChunkIndex = 0;

    void Start()
    {
        Sprite loadedSprite;
        string spriteBasename;
        float groundChunkOffset;
        
        currentGroundChunkIndex = Random.Range(0, spritesNumber);
        do
        {
            nextGroundChunkIndex = Random.Range(0, spritesNumber);
        }
        while (nextGroundChunkIndex == currentGroundChunkIndex);

        for (int i = 0; i < spritesNumber; i++)
        {
            spriteBasename = "Sprites/Level/ground_chunk_" + i;
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
            
            if ((i != currentGroundChunkIndex) && (i != nextGroundChunkIndex))
            {
                groundChunksPool[i].transform.position = invisiblePosition;
            }
        }
        groundChunkWidth = groundChunksPool[0].GetComponent<SpriteRenderer>().
                           bounds.size.x;

        groundChunkOffset = (groundChunkWidth / 2.0f)
            - (Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, 0.0f, 0.0f)).x);

        nextGroundChunkTransitionX = (groundChunkWidth * 1.5f)
                                     - groundChunkOffset;

        groundChunksPool[currentGroundChunkIndex].transform.position
            = new Vector2(groundChunkOffset,
                          (groundChunksPool[currentGroundChunkIndex].
                          transform.GetComponent<SpriteRenderer>().sprite.
                          bounds.size.y / 2.0f) - 1.0f);

        groundChunksPool[nextGroundChunkIndex].transform.position
            = new Vector2(groundChunkOffset + groundChunkWidth,
                          (groundChunksPool[nextGroundChunkIndex].transform.
                           GetComponent<SpriteRenderer>().sprite.bounds.size.y
                           / 2.0f) - 1.0f);
    }

    void Update()
    {
        rightScreenEdgeX = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, 0.0f, 0.0f)).x;

        if (rightScreenEdgeX >= nextGroundChunkTransitionX)
        {
            currentGroundChunkIndex = nextGroundChunkIndex;
            do
            {
                nextGroundChunkIndex = Random.Range(0, spritesNumber);
            }
            while (nextGroundChunkIndex == currentGroundChunkIndex);

            for (int i = 0; i < spritesNumber; i++)
            {
                if (i == nextGroundChunkIndex)
                {
                    groundChunksPool[i].transform.position = new Vector2(
                        nextGroundChunkTransitionX + (groundChunkWidth / 1.5f),
                        (groundChunksPool[i].transform.
                        GetComponent<SpriteRenderer>().sprite.bounds.size.y
                        / 2.0f) - 1.0f);
                }
                else if (i != currentGroundChunkIndex)
                {
                    groundChunksPool[i].transform.position = invisiblePosition;
                }
            }
            nextGroundChunkTransitionX += groundChunkWidth;
        }
    }
}

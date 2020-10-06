#undef DEBUG

using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    const int spritesNumberMin = 3;
    readonly int spritesNumber = 4;
    readonly Vector2 invisiblePosition = new Vector2(-100.0f, 0.0f);

    [SerializeField]
    GameObject groundChunkPrefab;
    GameObject groundChunksHolder;

    bool initialGroundChunk;
    float cameraHalfWidthInWorld;
    float groundChunkWidth;
    float nextGroundChunkTransitionX;
    int currentGroundChunkIndex;
    int nextGroundChunkIndex;
    int previousGroundChunkIndex;
    List<GameObject> groundChunksPool = new List<GameObject>();

    float CameraLeftEdgeInWorldX
    {
        get => Camera.main.transform.position.x - cameraHalfWidthInWorld;
    }

    float GroundChunkHalfWidth
    {
        get => groundChunkWidth / 2.0f;
    }

    void Start()
    {
        initialGroundChunk = true;

        currentGroundChunkIndex = Random.Range(0, spritesNumber);
        previousGroundChunkIndex = -1;

        cameraHalfWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, 0.0f, 0.0f)).x;

        nextGroundChunkTransitionX = CameraLeftEdgeInWorldX;

        if (spritesNumber < spritesNumberMin)
        {
#if DEBUG
            Debug.Log(GetType().Name
                +" initialization aborted. At least 3 ground sprites needed.");
#endif
            UnityQuit.Quit(1);
        }
        try
        {
            LoadGroundChunkSprites();
        }
        catch (System.Exception ex)
        {
#if DEBUG
            Debug.Log(ex);
#endif
            UnityQuit.Quit(1);
        }
        groundChunksHolder = new GameObject("GroundChunks");
    }

    void Update()
    {   
        GenerateInfiniteGround();
    }

    float CenterObjectVertically(in GameObject gameObject) =>
        gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;

    void GenerateInfiniteGround()
    {
        if (CameraLeftEdgeInWorldX >= nextGroundChunkTransitionX)
        {
            previousGroundChunkIndex = currentGroundChunkIndex;
            if (!initialGroundChunk)
            {
                currentGroundChunkIndex = nextGroundChunkIndex;
            }
            else
            {
                initialGroundChunk = false;
            }

            do
            {
                nextGroundChunkIndex = Random.Range(0, spritesNumber);
            }
            while ((nextGroundChunkIndex == previousGroundChunkIndex)
                   || (nextGroundChunkIndex == currentGroundChunkIndex));

            for (int i = 0; i < spritesNumber; i++)
            {
                groundChunksPool[i].transform.parent = groundChunksHolder.
                                                       transform;
                if (i == nextGroundChunkIndex)
                {
                    groundChunksPool[i].transform.position = new Vector2(
                        nextGroundChunkTransitionX + groundChunkWidth
                        + GroundChunkHalfWidth,
                        CenterObjectVertically(groundChunksPool[i]));
                }
                else if (i != currentGroundChunkIndex)
                {
                    groundChunksPool[i].transform.position = invisiblePosition;
                }
            }
            nextGroundChunkTransitionX += groundChunkWidth;
        }
    }

    void LoadGroundChunkSprites()
    {
        BoxCollider2D groundChunkCollider;
        GameObject groundChunk;
        Sprite loadedSprite;
        SpriteRenderer groundChunkRenderer;
        string spriteBasename;

        for (int i = 0; i < spritesNumber; i++)
        {
            spriteBasename = "Sprites/Level/ground_chunk_" + i;
            loadedSprite = Resources.Load<Sprite>(spriteBasename);

            if (loadedSprite == null)
            {
                string errMsg = GetType().Name
                                + " initialization aborted. Unable to load: "
                                + spriteBasename;
#if DEBUG
                Debug.Log(errMsg);
#endif
                throw new FileNotFoundException(errMsg);
            }
            groundChunksPool.Add(Instantiate(groundChunkPrefab));
            
            groundChunk = groundChunksPool[i];
            groundChunkCollider = groundChunk.GetComponent<BoxCollider2D>();
            groundChunkRenderer = groundChunk.GetComponent<SpriteRenderer>();

            groundChunkRenderer.sprite = loadedSprite;
            groundChunkCollider.size = groundChunkRenderer.sprite.bounds.size;
            groundChunkCollider.offset
                = groundChunkRenderer.sprite.bounds.center;
            
            if (i == currentGroundChunkIndex)
            {
                groundChunkWidth = groundChunkRenderer.sprite.bounds.size.x;

                groundChunk.transform.position = new Vector2(
                    GroundChunkHalfWidth - cameraHalfWidthInWorld,
                    CenterObjectVertically(
                        groundChunksPool[currentGroundChunkIndex]));
            }
            else
            {
                groundChunk.transform.position = invisiblePosition;
            }
        }
    }
}

#undef DEBUG

using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    const int spritesNumberMin = 3;
    readonly int spritesNumber = 4;
    readonly string[] airStreamSuffixes = { "cold", "hot" };
    readonly Vector2 graveyardPosition = new Vector2(-100.0f, 0.0f);

    [SerializeField]
    GameObject airStreamPrefab;

    [SerializeField]
    GameObject groundChunkPrefab;

    GameObject airStreamsParent;
    GameObject groundChunksParent;

    bool initialGroundChunk;
    float cameraHalfWidthInWorld;
    float groundChunkWidth;
    float nextGroundChunkTransitionX;
    int currentGroundChunkIndex;
    int nextGroundChunkIndex;
    int? previousGroundChunkIndex = null;
    List<GameObject> airStreamsPool = new List<GameObject>();
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
        airStreamsParent = new GameObject("AirStreamsPool");
        groundChunksParent = new GameObject("GroundChunksPool");

        cameraHalfWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, 0.0f, 0.0f)).x;
        try
        {
            InitializeAirStreamPool();
            InitializeGroundChunksPool();
        }
        catch (System.Exception ex)
        {
#if DEBUG
            Debug.Log(ex);
#endif
            UnityQuit.Quit(1);
        }
    }

    void Update()
    {   
        GenerateInfiniteGround();
    }

    float CenterObjectVertically(in GameObject gameObject) =>
        gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;

    GameObject CreateObjectFromPrefab(GameObject sourcePrefab, string basename)
    {
        BoxCollider2D objBoxCollider;
        Sprite objSprite;
        GameObject obj;
        SpriteRenderer objSpriteRenderer;

        obj = Instantiate(sourcePrefab);

        if ((objSprite = Resources.Load<Sprite>(basename)) == null)
        {
            string errMsg = GetType().Name
                            + " initialization aborted. Unable to load: "
                            + basename;
#if DEBUG
            Debug.Log(errMsg);
#endif
            throw new FileNotFoundException(errMsg);
        }
        obj.transform.position = graveyardPosition;
    
        objBoxCollider = obj.GetComponent<BoxCollider2D>();
        objSpriteRenderer = obj.GetComponent<SpriteRenderer>();

        objSpriteRenderer.sprite = objSprite;
        objBoxCollider.size = objSpriteRenderer.sprite.bounds.size;
        objBoxCollider.offset = objSpriteRenderer.sprite.bounds.center;

        return obj;
    }

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
                if (i == nextGroundChunkIndex)
                {
                    groundChunksPool[i].transform.position = new Vector2(
                        nextGroundChunkTransitionX + groundChunkWidth
                        + GroundChunkHalfWidth,
                        CenterObjectVertically(groundChunksPool[i]));
                }
                else if (i != currentGroundChunkIndex)
                {
                    groundChunksPool[i].transform.position = graveyardPosition;
                }
            }
            nextGroundChunkTransitionX += groundChunkWidth;
        }
    }

    void InitializeAirStreamPool()
    {
        int initialStreamIndex = Random.Range(0, airStreamSuffixes.Length);

        foreach (string suffix in airStreamSuffixes)
        {
            try
            {
                airStreamsPool.Add(CreateObjectFromPrefab(
                    airStreamPrefab, "Sprites/Level/air_stream_" + suffix));
            }
            catch (FileNotFoundException ex)
            {
#if DEBUG
                Debug.Log(ex);
#endif
                UnityQuit.Quit(1);
            }            
            airStreamsPool[airStreamsPool.Count - 1].transform.parent
                = airStreamsParent.transform;
        }
        // TODO: IT'S A TEMPONARY SOLUTION.
        airStreamsPool[initialStreamIndex].transform.position
            = new Vector2(6.0f, 4.0f);
    }

    void InitializeGroundChunksPool()
    {
        initialGroundChunk = true;
        currentGroundChunkIndex = Random.Range(0, spritesNumber);
        nextGroundChunkTransitionX = CameraLeftEdgeInWorldX;

        if (spritesNumber < spritesNumberMin)
        {
#if DEBUG
            Debug.Log(GetType().Name
                + " initialization aborted. At least 3 ground sprites needed.");
#endif
            UnityQuit.Quit(1);
        }
        for (int i = 0; i < spritesNumber; i++)
        {
            try
            {
                groundChunksPool.Add(CreateObjectFromPrefab(
                    groundChunkPrefab, "Sprites/Level/ground_chunk_" + i));
            }
            catch (FileNotFoundException ex)
            {
#if DEBUG
                Debug.Log(ex);
#endif
                UnityQuit.Quit(1);
            }
            groundChunksPool[i].transform.parent = groundChunksParent.transform;

            if (i == currentGroundChunkIndex)
            {
                var groundChunk = groundChunksPool[i];

                groundChunkWidth = groundChunk.GetComponent<SpriteRenderer>().
                                   sprite.bounds.size.x;

                groundChunk.transform.position = new Vector2(
                    GroundChunkHalfWidth - cameraHalfWidthInWorld,
                    CenterObjectVertically(groundChunk));
            }
        }
    }
}

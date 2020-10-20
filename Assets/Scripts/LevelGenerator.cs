using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    const int spritesNumberMin = 3;
    readonly int spritesNumber = 4;
    readonly string[] soaringLiftSuffixes
        = { "cold", "hot", "super_cold", "super_hot" };
    readonly Vector2 graveyardPosition = new Vector2(-100.0f, 0.0f);

    [SerializeField]
    GameObject soaringLiftPrefab;

    [SerializeField]
    GameObject groundChunkPrefab;

    GameObject soaringLiftsParent;
    GameObject groundChunksParent;

    bool initialAirStream; 
    bool initialGroundChunk;
    float cameraHalfWidthInWorld;
    float groundChunkWidth;
    float nextGroundChunkTransitionX;
    int nextAirStreamIndex;
    int? previousAirStreamIndex = null;
    int currentGroundChunkIndex;
    int nextGroundChunkIndex;
    int? previousGroundChunkIndex = null;
    List<GameObject> soaringLiftsPool = new List<GameObject>();
    List<GameObject> groundChunksPool = new List<GameObject>();

    float CameraLeftEdgeInWorldX
    {
        get => Camera.main.transform.position.x - cameraHalfWidthInWorld
               + Camera.main.transform.localPosition.x;
    }

    float GroundChunkHalfWidth
    {
        get => groundChunkWidth / 2.0f;
    }

    void Start()
    {
        soaringLiftsParent = new GameObject("SoaringLiftsPool");
        groundChunksParent = new GameObject("GroundChunksPool");

        cameraHalfWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, 0.0f, 0.0f)).x;
        try
        {
            InitializeSoaringLiftsPool();
            InitializeGroundChunksPool();
        }
        catch (System.Exception ex)
        {
            if (DebugUtils.GlobalEnabler.activated)
            {
                Debug.Log(ex);
            }
            UnityQuit.Quit(1);
        }
    }

    void Update()
    {   
        GenerateInfiniteGround();
        GenerateSoaringLiftsInfinitely();
    }

    float CenterObjectVertically(in GameObject gameObject) =>
        gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;

    GameObject CreateObjectFromPrefab(GameObject sourcePrefab, string basename)
    {
        BoxCollider2D goBoxCollider;
        Sprite goSprite;
        GameObject go;
        SpriteRenderer goSpriteRenderer;

        go = Instantiate(sourcePrefab);

        if ((goSprite = Resources.Load<Sprite>(basename)) == null)
        {
            string errMsg = GetType().Name
                            + " initialization aborted. Unable to load: "
                            + basename;

            if (DebugUtils.GlobalEnabler.activated)
            {
                Debug.Log(errMsg);
            }
            throw new FileNotFoundException(errMsg);
        }
        go.transform.position = graveyardPosition;
    
        goBoxCollider = go.GetComponent<BoxCollider2D>();
        goSpriteRenderer = go.GetComponent<SpriteRenderer>();

        goSpriteRenderer.sprite = goSprite;
        goBoxCollider.size = goSpriteRenderer.sprite.bounds.size;
        goBoxCollider.offset = goSpriteRenderer.sprite.bounds.center;

        return go;
    }

    void GenerateSoaringLiftsInfinitely()
    {
        const float maxOffCameraOffsetY = 1.0f;
        const float minOffScreenOffsetX = 1.0f;
        const float maxOffScreenOffsetX = 10.0f;

        GameObject soaringLift = soaringLiftsPool[nextAirStreamIndex];
        Vector2 newPosition;

        if (initialAirStream || (CameraLeftEdgeInWorldX
            >= (soaringLift.transform.position.x
                + (soaringLift. GetComponent<SpriteRenderer>().bounds.size.x
                   / 2.0f))))
        {
            previousAirStreamIndex = nextAirStreamIndex;
            do
            {
                nextAirStreamIndex = Random.Range(0,
                                                  soaringLiftSuffixes.Length);
            }
            while (nextAirStreamIndex == previousAirStreamIndex);

            newPosition.x = Random.Range(CameraLeftEdgeInWorldX
                + (cameraHalfWidthInWorld * 2.0f) + minOffScreenOffsetX,
                CameraLeftEdgeInWorldX + (cameraHalfWidthInWorld * 2.0f)
                + maxOffScreenOffsetX - Camera.main.transform.localPosition.x);

            newPosition.y = Random.Range(Camera.main.transform.position.y
                                         - maxOffCameraOffsetY,
                                         Camera.main.transform.position.y
                                         + maxOffCameraOffsetY);

            soaringLiftsPool[nextAirStreamIndex].transform.position
                = newPosition;
            initialAirStream = false;
        }
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
                        + GroundChunkHalfWidth
                        - Camera.main.transform.localPosition.x,
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

    void InitializeSoaringLiftsPool()
    {
        int initialStreamIndex = Random.Range(0, soaringLiftSuffixes.Length);
        initialAirStream = true;

        foreach (string suffix in soaringLiftSuffixes)
        {
            try
            {
                soaringLiftsPool.Add(CreateObjectFromPrefab(
                    soaringLiftPrefab, "Sprites/Level/soaring_lift_" + suffix));
            }
            catch (FileNotFoundException ex)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(ex);
                }
                UnityQuit.Quit(1);
            }            
            soaringLiftsPool[soaringLiftsPool.Count - 1].transform.parent
                = soaringLiftsParent.transform;

            // TODO: SAVE THOSE DATA IN JSON/XML?
            if (suffix == soaringLiftSuffixes[0])
            {
                soaringLiftsPool[soaringLiftsPool.Count - 1].
                    GetComponent<AirStream>().LiftRatio = -1.0f;
            }
            else if (suffix == soaringLiftSuffixes[1])
            {
                soaringLiftsPool[soaringLiftsPool.Count - 1].
                    GetComponent<AirStream>().LiftRatio = 1.0f;
            }
            else if (suffix == soaringLiftSuffixes[2])
            {
                soaringLiftsPool[soaringLiftsPool.Count - 1].
                    GetComponent<AirStream>().LiftRatio = -2.0f;
            }
            else if (suffix == soaringLiftSuffixes[3])
            {
                soaringLiftsPool[soaringLiftsPool.Count - 1].
                    GetComponent<AirStream>().LiftRatio = 2.0f;
            }
        }
    }

    void InitializeGroundChunksPool()
    {
        initialGroundChunk = true;
        currentGroundChunkIndex = Random.Range(0, spritesNumber);
        nextGroundChunkTransitionX = CameraLeftEdgeInWorldX;

        if (spritesNumber < spritesNumberMin)
        {
            if (DebugUtils.GlobalEnabler.activated)
            {
                Debug.Log(GetType().Name + " initialization aborted. "
                          + "At least 3 ground sprites needed.");
            }
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
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(ex);
                }
                UnityQuit.Quit(1);
            }
            groundChunksPool[i].transform.parent = groundChunksParent.transform;

            if (i == currentGroundChunkIndex)
            {
                var groundChunk = groundChunksPool[i];

                groundChunkWidth = groundChunk.GetComponent<SpriteRenderer>().
                                   sprite.bounds.size.x;

                groundChunk.transform.position = new Vector2(
                    GroundChunkHalfWidth - cameraHalfWidthInWorld
                    + Camera.main.transform.localPosition.x,
                    CenterObjectVertically(groundChunk));
            }
        }
    }
}

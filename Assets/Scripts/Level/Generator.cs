using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    class Generator : MonoBehaviour
    {
        const int groundChunksMin = 3;
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
        int? previousAirStreamIndex;
        int currentGroundChunkIndex;
        int nextGroundChunkIndex;
        int? previousGroundChunkIndex;
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

        float CenterObjectVertically(in GameObject go) =>
            go.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;

        GameObject CreateObjectFromPrefab(in GameObject sourcePrefab,
                                          string basename)
        {
            BoxCollider2D goBoxCollider;
            GameObject go;
            Sprite goSprite;
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
                                                      soaringLiftsPool.Count);
                }
                while (nextAirStreamIndex == previousAirStreamIndex);

                newPosition.x = Random.Range(CameraLeftEdgeInWorldX
                    + (cameraHalfWidthInWorld * 2.0f) + minOffScreenOffsetX,
                    CameraLeftEdgeInWorldX + (cameraHalfWidthInWorld * 2.0f)
                    + maxOffScreenOffsetX
                    - Camera.main.transform.localPosition.x);

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
                    nextGroundChunkIndex
                        = Random.Range(0, groundChunksPool.Count);
                }
                while ((nextGroundChunkIndex == previousGroundChunkIndex)
                       || (nextGroundChunkIndex == currentGroundChunkIndex));

                for (int i = 0; i < groundChunksPool.Count; i++)
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
                        groundChunksPool[i].transform.position
                            = graveyardPosition;
                    }
                }
                nextGroundChunkTransitionX += groundChunkWidth;
            }
        }

        void InitializeSoaringLiftsPool()
        {
            const string spritesPath 
                = @"Sprites/Level/Atmosphere/SoaringLifts/";

            string[] spritesNames = Directory.GetFiles(
                Path.Combine(@"Assets/Resources/", spritesPath), "*.psd");
            int initialStreamIndex = Random.Range(0, spritesNames.Length);

            initialAirStream = true;

            foreach (string name in spritesNames)
            {
                var spriteName = Path.GetFileNameWithoutExtension(name);

                try
                {
                    soaringLiftsPool.Add(CreateObjectFromPrefab(
                        soaringLiftPrefab,
                        Path.Combine(spritesPath, spriteName)));
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
                if (spriteName == "hot_air")
                {
                    soaringLiftsPool[soaringLiftsPool.Count - 1].
                        GetComponent<AirStream>().LiftRatio = 3.0f;
                }
                else if (spriteName == "wave_lift")
                {
                    soaringLiftsPool[soaringLiftsPool.Count - 1].
                        GetComponent<AirStream>().LiftRatio = 1.0f;
                }
                else if (spriteName == "cold_air")
                {
                    soaringLiftsPool[soaringLiftsPool.Count - 1].
                        GetComponent<AirStream>().LiftRatio = -2.0f;
                }
            }
        }

        void InitializeGroundChunksPool()
        {
            const string spriteNamePrefix = @"ground_chunk_";
            const string spritesPath = @"Sprites/Level/GroundChunks/";

            string[] spritesNames = Directory.GetFiles(
                Path.Combine(@"Assets/Resources/", spritesPath), "*.psd");

            currentGroundChunkIndex = Random.Range(0, spritesNames.Length);
            nextGroundChunkTransitionX = CameraLeftEdgeInWorldX;
            initialGroundChunk = true;

            if (spritesNames.Length < groundChunksMin)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(GetType().Name + " initialization aborted. "
                              + $"At least {groundChunksMin} ground needed.");
                }
                UnityQuit.Quit(1);
            }
            for (int i = 0; i < spritesNames.Length; i++)
            {
                try
                {
                    groundChunksPool.Add(CreateObjectFromPrefab(
                        groundChunkPrefab,
                        Path.Combine(spritesPath, spriteNamePrefix) + i));
                }
                catch (FileNotFoundException ex)
                {
                    if (DebugUtils.GlobalEnabler.activated)
                    {
                        Debug.Log(ex);
                    }
                    UnityQuit.Quit(1);
                }
                groundChunksPool[i].transform.parent
                    = groundChunksParent.transform;

                if (i == currentGroundChunkIndex)
                {
                    var groundChunk = groundChunksPool[i];

                    groundChunkWidth = groundChunk.
                        GetComponent<SpriteRenderer>().sprite.bounds.size.x;

                    groundChunk.transform.position = new Vector2(
                        GroundChunkHalfWidth - cameraHalfWidthInWorld
                        + Camera.main.transform.localPosition.x,
                        CenterObjectVertically(groundChunk));
                }
            }
        }
    }
}

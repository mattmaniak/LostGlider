using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Loader))]
    public sealed class Generator : MonoBehaviour
    {
        int? previousAirStreamIndex;
        int? previousGroundChunkIndex;

        Loader loader;

        internal bool InitialAtmosphericPhenomenon { get; set; }
        internal bool InitialGroundChunk { get; set; }
        internal float CameraLeftEdgeInWorldX
        {
            get => Camera.main.transform.position.x
                - loader.CameraHalfWidthInWorld
                + Camera.main.transform.localPosition.x;
        }

        internal float NextGroundChunkTransitionX { get; set; }
        internal int CurrentGroundChunkIndex { get; set; }

        int NextAirStreamIndex { get; set; }
        int NextGroundChunkIndex { get; set; }

        void Awake()
        {
            loader = GetComponent<Loader>();

            CurrentGroundChunkIndex = Random.Range(0,
                loader.GroundChunksPool.Count);
            NextGroundChunkTransitionX = CameraLeftEdgeInWorldX;
            InitialAtmosphericPhenomenon = InitialGroundChunk = true;

            DisplayInitialGroundChunk();
        }

        void Update()
        {   
            GenerateInfiniteGround();
            GenerateAtmosphericPhenomenaInfinitely();
        }

        void DisplayInitialGroundChunk()
        {
            for (int i = 0; i < loader.GroundChunksPool.Count; i++)
            {
                if (i == CurrentGroundChunkIndex)
                {
                    var groundChunk = loader.GroundChunksPool[i];

                    loader.GroundChunkWidth = groundChunk.
                        GetComponent<SpriteRenderer>().sprite.bounds.size.x;

                    groundChunk.transform.position = new Vector2(
                        loader.GroundChunkHalfWidth
                        - loader.CameraHalfWidthInWorld
                        + Camera.main.transform.localPosition.x
                        + FindObjectOfType<Player>().transform.position.x,
                        loader.CenterObjectVertically(groundChunk));
                }
            }
        }

        void GenerateAtmosphericPhenomenaInfinitely()
        {
            const float maxOffCameraOffsetY = 1.0f;
            const float minOffScreenOffsetX = 1.0f;
            const float maxOffScreenOffsetX = 2.0f;

            GameObject atmosphericPhenomenon = loader.AtmosphericPhenomenaPool
                [NextAirStreamIndex];
            Vector2 newPosition;

            if (InitialAtmosphericPhenomenon || (CameraLeftEdgeInWorldX
                >= (atmosphericPhenomenon.transform.position.x
                    + (atmosphericPhenomenon.GetComponent<SpriteRenderer>().
                       bounds.size.x / 2.0f))))
            {
                previousAirStreamIndex = NextAirStreamIndex;
                do
                {
                    NextAirStreamIndex = Random.Range(0,
                        loader.AtmosphericPhenomenaPool.Count);
                }
                while (NextAirStreamIndex == previousAirStreamIndex);

                newPosition.x = Random.Range(CameraLeftEdgeInWorldX
                    + loader.CameraWidthInWorld + minOffScreenOffsetX,
                    CameraLeftEdgeInWorldX + loader.CameraWidthInWorld
                    + maxOffScreenOffsetX
                    - Camera.main.transform.localPosition.x);

                newPosition.y = Random.Range(
                    Camera.main.transform.position.y - maxOffCameraOffsetY,
                    Camera.main.transform.position.y + maxOffCameraOffsetY);

                loader.AtmosphericPhenomenaPool[NextAirStreamIndex].
                    transform.position = newPosition;

                InitialAtmosphericPhenomenon = false;
            }
        }

        void GenerateInfiniteGround()
        {
            if (CameraLeftEdgeInWorldX >= NextGroundChunkTransitionX)
            {
                previousGroundChunkIndex = CurrentGroundChunkIndex;
                if (!InitialGroundChunk)
                {
                    CurrentGroundChunkIndex = NextGroundChunkIndex;
                }
                else
                {
                    InitialGroundChunk = false;
                }

                do
                {
                    NextGroundChunkIndex = Random.Range(0,
                        loader.GroundChunksPool.Count);
                }
                while ((NextGroundChunkIndex == previousGroundChunkIndex)
                       || (NextGroundChunkIndex == CurrentGroundChunkIndex));

                for (int i = 0; i < loader.GroundChunksPool.Count; i++)
                {
                    if (i == NextGroundChunkIndex)
                    {
                        loader.GroundChunksPool[i].transform.position =
                            new Vector2(NextGroundChunkTransitionX
                            + loader.GroundChunkWidth
                            + loader.GroundChunkHalfWidth
                            - Camera.main.transform.localPosition.x,
                            loader.CenterObjectVertically(
                            loader.GroundChunksPool[i]));
                    }
                    else if (i != CurrentGroundChunkIndex)
                    {
                        loader.GroundChunksPool[i].transform.position =
                            loader.graveyardPosition;
                    }
                }
                NextGroundChunkTransitionX += loader.GroundChunkWidth;
            }
        }
    }
}

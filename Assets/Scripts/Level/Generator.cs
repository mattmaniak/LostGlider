using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Level
{
    public sealed class Generator : ScriptableObject
    {
#region Singleton
        static readonly Generator instance = new Generator();
        public static Generator Instance { get => instance; }

        static Generator() { }
        Generator() { }
#endregion

#region Constants
        internal readonly Vector2 graveyardPosition = new Vector2(-100.0f, 0.0f);
#endregion

        int? previousAirStreamIndex;
        int? previousGroundChunkIndex;

        internal List<GameObject> AtmosphericPhenomenaPool
        {
            get;
            set;
        } = new List<GameObject>();

        internal List<GameObject> GroundChunksPool { get; set; } =
            new List<GameObject>();

        internal bool InitialAtmosphericPhenomenon { get; set; }
        internal bool InitialGroundChunk { get; set; }
        internal float CameraLeftEdgeInWorldX
        {
            get => Camera.main.transform.position.x - Loader.Instance.CameraHalfWidthInWorld
                + Camera.main.transform.localPosition.x;
        }

        internal float NextGroundChunkTransitionX { get; set; }
        internal int CurrentGroundChunkIndex { get; set; }

        int NextAirStreamIndex { get; set; }
        int NextGroundChunkIndex { get; set; }

        public void Start()
        {
            Loader.Instance.CameraHalfWidthInWorld = Camera.main.
                ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x;
            try
            {
                Loader.Instance.InitializeAtmosphericPhenomenaPool();
                Loader.Instance.InitializeGroundChunksPool();
                Loader.Instance.ConfigureAtmosphericPhenomena();
            }
            catch (System.Exception ex)
            {
                if (DebugUtils.GlobalEnabler.activated)
                {
                    Debug.Log(ex);
                }
                Utils.UnityQuit.Quit(1);
            }
        }

        public void Update()
        {   
            GenerateInfiniteGround();
            GenerateSoaringLiftsInfinitely();
        }

        internal float CenterObjectVertically(in GameObject go) =>
            go.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;

        void GenerateSoaringLiftsInfinitely()
        {
            const float maxOffCameraOffsetY = 1.0f;
            const float minOffScreenOffsetX = 1.0f;
            const float maxOffScreenOffsetX = 2.0f;

            GameObject atmosphericPhenomenon =
                AtmosphericPhenomenaPool[NextAirStreamIndex];
            Vector2 newPosition;

            if (InitialAtmosphericPhenomenon || (CameraLeftEdgeInWorldX
                >= (atmosphericPhenomenon.transform.position.x
                    + (atmosphericPhenomenon.GetComponent<SpriteRenderer>().
                       bounds.size.x / 2.0f))))
            {
                previousAirStreamIndex = NextAirStreamIndex;
                do
                {
                    NextAirStreamIndex = Random.Range(
                        0, AtmosphericPhenomenaPool.Count);
                }
                while (NextAirStreamIndex == previousAirStreamIndex);

                newPosition.x = Random.Range(
                    CameraLeftEdgeInWorldX + Loader.Instance.CameraWidthInWorld
                    + minOffScreenOffsetX,
                    CameraLeftEdgeInWorldX + Loader.Instance.CameraWidthInWorld
                    + maxOffScreenOffsetX
                    - Camera.main.transform.localPosition.x);

                newPosition.y = Random.Range(
                    Camera.main.transform.position.y - maxOffCameraOffsetY,
                    Camera.main.transform.position.y + maxOffCameraOffsetY);

                AtmosphericPhenomenaPool[NextAirStreamIndex].
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
                    NextGroundChunkIndex = Random.Range(
                        0, GroundChunksPool.Count);
                }
                while ((NextGroundChunkIndex == previousGroundChunkIndex)
                       || (NextGroundChunkIndex == CurrentGroundChunkIndex));

                for (int i = 0; i < GroundChunksPool.Count; i++)
                {
                    if (i == NextGroundChunkIndex)
                    {
                        GroundChunksPool[i].transform.position = new Vector2(
                            NextGroundChunkTransitionX
                            + Loader.Instance.GroundChunkWidth
                            + Loader.Instance.GroundChunkHalfWidth
                            - Camera.main.transform.localPosition.x,
                            CenterObjectVertically(GroundChunksPool[i]));
                    }
                    else if (i != CurrentGroundChunkIndex)
                    {
                        GroundChunksPool[i].transform.position =
                            graveyardPosition;
                    }
                }
                NextGroundChunkTransitionX += Loader.Instance.GroundChunkWidth;
            }
        }
    }
}

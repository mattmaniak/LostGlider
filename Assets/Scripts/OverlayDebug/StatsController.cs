#define DEBUG

using UnityEngine;

namespace OverlayDebug
{
    public class StatsController : MonoBehaviour
    {
        void Start()
        {
#if DEBUG
            OverlayDebug.StatsModel.ReadGitRepoData();
#endif
        }
    }
}

#define DEBUG

using UnityEngine;

namespace DebugUtils
{
    public class StatsController : MonoBehaviour
    {
        void Start()
        {
#if DEBUG
            DebugUtils.StatsModel.ReadGitRepositoryData();
#endif
        }
    }
}

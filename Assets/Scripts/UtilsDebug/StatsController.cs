#define DEBUG

using UnityEngine;

namespace UtilsDebug
{
    public class StatsController : MonoBehaviour
    {
        void Start()
        {
#if DEBUG
            UtilsDebug.StatsModel.ReadGitRepositoryData();
#endif
        }
    }
}

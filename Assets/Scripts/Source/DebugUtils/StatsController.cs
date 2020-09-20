using UnityEngine;

namespace DebugUtils
{
    public class StatsController : MonoBehaviour
    {
        void Start()
        {
            DebugUtils.StatsModel.ReadGitRepositoryData();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace DebugUtils
{
    public class StatsView : MonoBehaviour
    {
        // public Text buildGUID;
        [SerializeField]
        Text gitBranchUI;

        [SerializeField]
        Text gitRevisionUI;

        void Start()
        {
            gitBranchUI.text = "Git branch: "
                                + DebugUtils.StatsModel.GitBranch;

            gitRevisionUI.text = "Last Git revision: "
                                 + DebugUtils.StatsModel.GitRevision;
        }
    }
}

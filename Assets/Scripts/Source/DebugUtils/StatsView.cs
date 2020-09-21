using UnityEngine;
using UnityEngine.UI;

namespace DebugUtils
{
    public class StatsView : MonoBehaviour
    {
        // public Text buildGUID;
        [SerializeField]
        Text gitBranchText;

        [SerializeField]
        Text gitRevisionText;

        static bool statsUpdated;
        static string gitBranch;
        static string gitRevision;

        void Start()
        {
            statsUpdated = false;
        }

        void Update()
        {
            if (statsUpdated)
            {
                DisplayStats();
                statsUpdated = false;
            }
        }

        // Get Data from StatsModel.
        public static void UpdateStats()
        {
            gitBranch = DebugUtils.StatsModel.GitBranch;
            gitRevision = DebugUtils.StatsModel.GitRevision;

            statsUpdated = true;
        }

        void DisplayStats()
        {
            gitBranchText.text = "Git branch: " + gitBranch;
            gitRevisionText.text = "Last Git revision: " + gitRevision;
        }
    }
}

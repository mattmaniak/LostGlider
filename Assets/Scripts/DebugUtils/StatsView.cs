using UnityEngine;
using UnityEngine.UI;

namespace DebugUtils
{
    public class StatsView : MonoBehaviour
    {
        [SerializeField]
        Text appSummaryText;

        [SerializeField]
        Text gitBranchText;

        [SerializeField]
        Text gitRevisionText;

        const string debugLabel = "[DEBUG] ";

        static bool statsUpdated;
        static string appSummary;
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
            appSummary = DebugUtils.StatsModel.AppSummary;
            gitBranch = DebugUtils.StatsModel.GitBranch;
            gitRevision = DebugUtils.StatsModel.GitRevision;

            statsUpdated = true;
        }

        void DisplayStats()
        {
            appSummaryText.text  = debugLabel + appSummary;
            gitBranchText.text   = debugLabel + "Git branch: " + gitBranch;
            gitRevisionText.text = debugLabel + "Last Git revision: "
                                   + gitRevision;
        }
    }
}

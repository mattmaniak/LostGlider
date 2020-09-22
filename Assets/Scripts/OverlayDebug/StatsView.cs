using UnityEngine;
using UnityEngine.UI;

namespace OverlayDebug
{
    public class StatsView : MonoBehaviour
    {
        const string debugLabel = "[DEBUG] ";

        [SerializeField]
        Text unitySummaryText;

        [SerializeField]
        Text gitRepoDataText;

        static bool statsUpdated;
        static string gitRepoData;
        static string unityProjectInfo;

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
            gitRepoData      = OverlayDebug.StatsModel.GitRepoData;
            unityProjectInfo = OverlayDebug.StatsModel.UnityProjectInfo;

            statsUpdated = true;
        }

        void DisplayStats()
        {
            gitRepoDataText.text  = debugLabel + gitRepoData;
            unitySummaryText.text = debugLabel + unityProjectInfo;
        }
    }
}

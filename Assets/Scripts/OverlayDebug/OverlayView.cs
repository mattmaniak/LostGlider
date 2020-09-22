using UnityEngine;
using UnityEngine.UI;

namespace OverlayDebug
{
    public class OverlayView : MonoBehaviour
    {
        const string debugLabel = "[DEBUG] ";

        [SerializeField]
        Text unitySummaryText;

        [SerializeField]
        Text gitRepoDataText;

        static bool OverlayUpdated;
        static string gitRepoData;
        static string unityProjectInfo;

        void Start()
        {
            OverlayUpdated = false;
        }

        void Update()
        {
            if (OverlayUpdated)
            {
                DisplayOverlay();
                OverlayUpdated = false;
            }
        }

        // Get Data from OverlayModel.
        public static void UpdateOverlay()
        {
            gitRepoData      = OverlayDebug.OverlayModel.GitRepoData;
            unityProjectInfo = OverlayDebug.OverlayModel.UnityProjectInfo;

            OverlayUpdated = true;
        }

        void DisplayOverlay()
        {
            gitRepoDataText.text  = debugLabel + gitRepoData;
            unitySummaryText.text = debugLabel + unityProjectInfo;
        }
    }
}

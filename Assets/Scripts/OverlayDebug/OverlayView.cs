using UnityEngine;
using UnityEngine.UI;

namespace OverlayDebug
{
    public class OverlayView : MonoBehaviour
    {
        const string debugLabel = "[DEBUG] ";

        [SerializeField]
        Text unityProjectInfoText;

        [SerializeField]
        Text gitRepoSummaryText;

        static bool OverlayUpdated;
        static string gitRepoSummary;
        static string unityProjectInfo;

        void Start()
        {
            OverlayUpdated = false;
        }

        void Update()
        {
            if (OverlayUpdated)
            {
                RenderOverlay();
                OverlayUpdated = false;
            }
        }

        // Get Data from OverlayModel.
        public static void DisplayOverlay()
        {
            gitRepoSummary      = OverlayDebug.OverlayModel.GitRepoSummary;
            unityProjectInfo = OverlayDebug.OverlayModel.UnityProjectInfo;

            OverlayUpdated = true;
        }

        void RenderOverlay()
        {
            gitRepoSummaryText.text  = debugLabel + gitRepoSummary;
            unityProjectInfoText.text = debugLabel + unityProjectInfo;
        }
    }
}

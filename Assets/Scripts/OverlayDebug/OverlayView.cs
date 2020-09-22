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

        static string gitRepoSummary;
        static string unityProjectInfo;

        void Update()
        {
            if (OverlayDebug.OverlayController.ShouldUpdateView)
            {
                Display();
                OverlayDebug.OverlayController.DisableViewUpdateAction();
            }
        }
        internal static void UpdateView()
        {
            gitRepoSummary   = OverlayDebug.OverlayModel.GitRepoSummary;
            unityProjectInfo = OverlayDebug.OverlayModel.UnityProjectInfo;
        }

        void Display()
        {
            gitRepoSummaryText.text   = debugLabel + gitRepoSummary;
            unityProjectInfoText.text = debugLabel + unityProjectInfo;
        }
    }
}

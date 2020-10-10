using UnityEngine;
using UnityEngine.UI;

namespace DebugUtils
{
    namespace Overlay
    {
        public class View : MonoBehaviour
        {
            const string debugLabel = "[DEBUG] ";

            [SerializeField]
            Text gitRepoSummaryText;

            [SerializeField]
            Text unityProjectInfoText;

            static string gitRepoSummary;
            static string unityProjectInfo;

            void Update()
            {
                if (DebugUtils.Overlay.Controller.ShouldUpdateView)
                {
                    Display();
                    DebugUtils.Overlay.Controller.DisableViewUpdateAction();
                }
            }
            internal static void UpdateView()
            {
                gitRepoSummary = DebugUtils.Overlay.Model.GitRepoSummary;
                unityProjectInfo = DebugUtils.Overlay.Model.UnityProjectInfo;
            }

            void Display()
            {
                gitRepoSummaryText.text = debugLabel + gitRepoSummary;
                unityProjectInfoText.text = debugLabel + unityProjectInfo;
            }
        }
    }
}


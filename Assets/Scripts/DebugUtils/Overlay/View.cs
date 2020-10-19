using UnityEngine;
using UnityEngine.UI;

namespace DebugUtils
{
    namespace Overlay
    {
        public class View : MonoBehaviour
        {
            [SerializeField]
            Text gitRepoSummaryText;

            [SerializeField]
            Text unityProjectInfoText;

            [SerializeField]
            Text wipLabelText;

            static string gitRepoSummary;
            static string unityProjectInfo;
            static string wipLabel;

            void Update()
            {
                if (DebugUtils.Overlay.Controller.ShouldUpdateView)
                {
                    Display();
                    DebugUtils.Overlay.Controller.DisableViewUpdateAction();
                }
            }
            internal static void UpdateView(Modes currentMode)
            {
                switch (currentMode)
                {
                    case Modes.Disabled:
                        break;

                    case Modes.PartiallyEnabled:
                        wipLabel = DebugUtils.Overlay.Model.WipLabel;
                        break;

                    case Modes.FullyEnabled:
                        gitRepoSummary
                            = DebugUtils.Overlay.Model.GitRepoSummary;
                        unityProjectInfo
                            = DebugUtils.Overlay.Model.UnityProjectInfo;
                        
                        wipLabel = DebugUtils.Overlay.Model.WipLabel;
                        break;
                }
            }

            void Display()
            {
                gitRepoSummaryText.text = gitRepoSummary;
                unityProjectInfoText.text = unityProjectInfo;
                wipLabelText.text = wipLabel;
            }
        }
    }
}


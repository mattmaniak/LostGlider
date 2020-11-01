using UnityEngine;
using UnityEngine.UI;

namespace DebugUtils
{
    namespace Overlay
    {
        class View : MonoBehaviour
        {
            [SerializeField]
            Text gitRepoSummaryText;

            [SerializeField]
            Text unityProjectInfoText;

            [SerializeField]
            Text wipLabelText;

            static string GitRepoSummary { get; set; }
            static string UnityProjectInfo { get; set; }
            static string WipLabel { get; set; }

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
                        WipLabel = DebugUtils.Overlay.Model.WipLabel;
                        break;

                    case Modes.FullyEnabled:
                        GitRepoSummary
                            = DebugUtils.Overlay.Model.GitRepoSummary;
                        UnityProjectInfo
                            = DebugUtils.Overlay.Model.UnityProjectInfo;
                        
                        WipLabel = DebugUtils.Overlay.Model.WipLabel;
                        break;
                }
            }

            void Display()
            {
                gitRepoSummaryText.text = GitRepoSummary;
                unityProjectInfoText.text = UnityProjectInfo;
                wipLabelText.text = WipLabel;
            }
        }
    }
}


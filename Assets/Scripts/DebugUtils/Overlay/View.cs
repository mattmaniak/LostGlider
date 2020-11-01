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

            string GitRepoSummary { get; set; }
            string UnityProjectInfo { get; set; }
            string WipLabel { get; set; }

            void Update()
            {
                if (DebugUtils.Overlay.Controller.ShouldUpdateView)
                {
                    Display();
                    DebugUtils.Overlay.Controller.DisableViewUpdateAction();
                }
            }

            internal void UpdateView(Modes currentMode)
            {
                switch (currentMode)
                {
                    case Modes.Disabled:
                        break;

                    case Modes.PartiallyEnabled:
                        WipLabel = DebugUtils.Overlay.Model.Instance.WipLabel;
                        break;

                    case Modes.FullyEnabled:
                        GitRepoSummary = DebugUtils.Overlay.Model.Instance.
                            GitRepoSummary;
                        
                        UnityProjectInfo = DebugUtils.Overlay.Model.Instance.
                            UnityProjectInfo;
                        
                        WipLabel = DebugUtils.Overlay.Model.Instance.WipLabel;
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


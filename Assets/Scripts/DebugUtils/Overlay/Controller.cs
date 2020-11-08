using UnityEngine;

namespace DebugUtils
{
    namespace Overlay
    {    
        enum Modes
        {
            Disabled, // Production.
            PartiallyEnabled, // Public presentation.
            FullyEnabled // Internal development.
        }

        class Controller : MonoBehaviour
        {
            static Modes currentMode = Modes.FullyEnabled;
            internal static bool ShouldUpdateView { get; private set; }
            
            void Awake()
            {
                if (currentMode == Modes.Disabled)
                {
                    return;
                }
                DebugUtils.Overlay.Model.Instance.UpdateModel();
            }

            internal static void NotifiyModelUpdated()
            {
                ShouldUpdateView = true;
                FindObjectOfType<DebugUtils.Overlay.View>().UpdateView(
                    currentMode);
            }

            internal static void DisableViewUpdateAction()
            {
                ShouldUpdateView = false;
            }
        }
    }
}

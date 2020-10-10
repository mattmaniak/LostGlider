using UnityEngine;

namespace DebugUtils
{
    namespace Overlay
    {    
        public class Controller : MonoBehaviour
        {
            [SerializeField]
            bool enabled = true;

            static bool shouldUpdateView = false;

            internal static bool ShouldUpdateView
            {
                get => shouldUpdateView;
            }

            void Start()
            {
                if (enabled)
                {
                    DebugUtils.Overlay.Model.UpdateModel();
                }
            }

            internal static void NotifiyModelUpdated()
            {
                shouldUpdateView = true;
                DebugUtils.Overlay.View.UpdateView();
            }

            internal static void DisableViewUpdateAction()
            {
                shouldUpdateView = false;
            }
        }
    }
}

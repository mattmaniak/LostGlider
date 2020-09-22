#define DEBUG

using UnityEngine;

namespace OverlayDebug
{
    public class OverlayController : MonoBehaviour
    {
        static bool shouldUpdateView = false;

        internal static bool ShouldUpdateView
        {
            get { return shouldUpdateView; }
        }

        void Start()
        {
#if DEBUG
            OverlayDebug.OverlayModel.UpdateModel();
#endif
        }

        internal static void NotifiyModelUpdated()
        {
            shouldUpdateView = true;
            OverlayDebug.OverlayView.UpdateView();
        }

        internal static void DisableViewUpdateAction()
        {
            shouldUpdateView = false;
        }
    }
}

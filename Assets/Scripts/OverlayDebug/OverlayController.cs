#define DEBUG

using UnityEngine;

namespace OverlayDebug
{
    public class OverlayController : MonoBehaviour
    {
        static bool shouldUpdateView = false;

        public static bool ShouldUpdateView
        {
            get { return shouldUpdateView; }
        }

        void Start()
        {
#if DEBUG
            OverlayDebug.OverlayModel.UpdateModel();
#endif
        }

        public static void NotifiyModelUpdated()
        {
            shouldUpdateView = true;
            OverlayDebug.OverlayView.UpdateView();
        }

        public static void DisableViewUpdateAction()
        {
            shouldUpdateView = false;
        }
    }
}

using UnityEngine;

namespace OverlayDebug
{    
    public class OverlayController : MonoBehaviour
    {
        [SerializeField]
        bool overlayEnabled = true;

        static bool shouldUpdateView = false;

        internal static bool ShouldUpdateView
        {
            get { return shouldUpdateView; }
        }

        void Start()
        {
            if (overlayEnabled)
            {
                OverlayDebug.OverlayModel.UpdateModel();
            }
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

using UnityEngine;

namespace OverlayDebug
{    
    public class OverlayController : MonoBehaviour
    {
        [SerializeField]
        bool displayOverlay = true;

        static bool shouldUpdateView = false;

        internal static bool ShouldUpdateView
        {
            get => shouldUpdateView;
        }

        void Start()
        {
            if (displayOverlay)
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

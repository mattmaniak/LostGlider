#define DEBUG

using UnityEngine;

namespace OverlayDebug
{
    public class OverlayController : MonoBehaviour
    {
        void Start()
        {
#if DEBUG
            OverlayDebug.OverlayModel.UpdateGitRepoSummary();
            OverlayDebug.OverlayModel.UpdateUnityProjectInfo();
#endif
        }
    }
}

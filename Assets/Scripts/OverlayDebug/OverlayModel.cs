#define DEBUG

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace OverlayDebug
{
    internal class OverlayModel
    {
        const string onErrorPlaceholder = "[not found]";

#region Directories
        static string gitBranchBasename;
        static string gitRepoPath = @Application.dataPath + @"/../.git/";
#endregion

#region Git data holders
        static string gitBranch;
        static string gitRevision;
        static string gitRepoSummary;
#endregion

        static string unityProjectInfo;

        internal static string GitRepoSummary
        {
            get
            {
                return "Modifying last Git revision: "
                       + ShortenGitRevision() + " on branch: " + gitBranch;
            }
        }

        internal static string UnityProjectInfo
        {
            get { return unityProjectInfo; }
        }

        internal static void UpdateModel()
        {
            UpdateGitRepoSummary();
            UpdateUnityProjectInfo();
            OverlayDebug.OverlayController.NotifiyModelUpdated();
        }

        static void UpdateGitRepoSummary()
        {
            if (!Directory.Exists(gitRepoPath))
            {
                return;
            }
            gitBranchBasename = ReadString(
                gitRepoPath + @"HEAD").Split(' ')[1].Trim();

            // Remove the relative directory that points to a branch file.
            gitBranch = gitBranchBasename.TrimStart(
                @"refs/heads/".ToCharArray()).Trim();

            gitRevision = ReadString(gitRepoPath + gitBranchBasename);
        }

        static void UpdateUnityProjectInfo()
        {
            unityProjectInfo = Application.productName + " "
                               + Application.version + " (dev) by "
                               + Application.companyName + " using Unity "
                               + Application.unityVersion;
        }

        static string ReadString(string path)
        {
            using (var stream = new StreamReader(path))
            {
                try
                {
                    return stream.ReadLine();
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.Log(ex);
#endif
                    return onErrorPlaceholder;
                }
            }
        }

        static string ShortenGitRevision()
        {
            const int length = 7;
            return gitRevision.Substring(0, length);
        }
    }
}

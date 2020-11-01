using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace DebugUtils
{
    namespace Overlay
    {
        class Model
        {
            const string debugLabel = "[DEBUG] ";
            const string onErrorPlaceholder = "[not found]";

#region Directories
            static string GitBranchBasename { get; set; }
            static string GitRepoPath
            {
                get => Application.dataPath + @"/../.git/";
            }
#endregion

#region Git data holders
            public static string GitBranch { get; set; }
            public static string GitShortRev { get; set; }
#endregion

            internal static string GitRepoSummary
            {
                get => debugLabel + "Modifying last Git revision: "
                       + GitShortRev + " on branch: " + GitBranch;
            }

            internal static string UnityProjectInfo { get; private set; }

            internal static string WipLabel
            {
                get => "WORK IN PROGRESS - "
                       + "DOES NOT REPRESENT FINAL LOOK OF THE GAME";
            }

            internal static void UpdateModel()
            {
                UpdateGitRepoSummary();
                UpdateUnityProjectInfo();
                DebugUtils.Overlay.Controller.NotifiyModelUpdated();
            }

            static void UpdateGitRepoSummary()
            {
                const int shortGitRevLength = 7;

                if (!Directory.Exists(GitRepoPath))
                {
                    GitBranch = GitShortRev = onErrorPlaceholder;
                    return;
                }
                try
                {
                    GitBranchBasename = ReadString(GitRepoPath + @"HEAD")?.
                        Split(' ')?[1]?.Trim();
                }
                catch (IndexOutOfRangeException ex)
                {
                    if (DebugUtils.GlobalEnabler.activated)
                    {
                        Debug.Log(ex);
                    }
                }

                // Remove the relative directory that points to a branch file.
                GitBranch = GitBranchBasename?.
                    TrimStart(@"refs/heads/".ToCharArray())?.Trim()
                    ?? onErrorPlaceholder;

                GitShortRev = ReadString(GitRepoPath + GitBranchBasename)?.
                    Substring(0, shortGitRevLength) ?? onErrorPlaceholder;
            }

            static void UpdateUnityProjectInfo()
            {
                UnityProjectInfo = debugLabel
                                   + Application.productName + " "
                                   + Application.version + " (dev) by "
                                   + Application.companyName + " using Unity "
                                   + Application.unityVersion;
            }

            static string ReadString(string path)
            {
                try
                {
                    using (var stream = new StreamReader(path))
                    {
                        return stream.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    if (DebugUtils.GlobalEnabler.activated)
                    {
                        Debug.Log(ex);
                    }
                    return null;
                }
            }
        }
    }
}

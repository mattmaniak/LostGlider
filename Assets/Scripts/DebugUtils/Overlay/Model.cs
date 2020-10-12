﻿using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace DebugUtils
{
    namespace Overlay
    {
        internal class Model
        {
            const string onErrorPlaceholder = "[not found]";

#region Directories
            static string gitBranchBasename;
            static string gitRepoPath = @Application.dataPath + @"/../.git/";
#endregion

#region Git data holders
            static string gitBranch;
            static string gitShortRev;
            static string gitRepoSummary;
#endregion

            static string unityProjectInfo;

            internal static string GitRepoSummary
            {
                get => "Modifying last Git revision: " + gitShortRev
                        + " on branch: " + gitBranch;
            }

            internal static string UnityProjectInfo
            {
                get => unityProjectInfo;
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

                if (!Directory.Exists(gitRepoPath))
                {
                    gitBranch = gitShortRev = onErrorPlaceholder;
                    return;
                }
                try
                {
                    gitBranchBasename = ReadString(gitRepoPath + @"HEAD")?.
                        Split(' ')?[1]?.Trim();
                }
                catch (IndexOutOfRangeException ex)
                {
                    if (DebugUtils.GlobalEnabler.enabled)
                    {
                        Debug.Log(ex);
                    }
                }

                // Remove the relative directory that points to a branch file.
                gitBranch = gitBranchBasename?.
                    TrimStart(@"refs/heads/".ToCharArray())?.Trim()
                    ?? onErrorPlaceholder;

                gitShortRev = ReadString(gitRepoPath + gitBranchBasename)?.
                    Substring(0, shortGitRevLength)
                    ?? onErrorPlaceholder;;
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
                try
                {
                    using (var stream = new StreamReader(path))
                    {
                        return stream.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    if (DebugUtils.GlobalEnabler.enabled)
                    {
                        Debug.Log(ex);
                    }
                    return null;
                }
            }
        }
    }
}
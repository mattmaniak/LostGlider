using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace DebugUtils
{
    namespace Overlay
    {
        sealed class Model
        {
            static readonly Model instance = new Model();

            const string debugLabel = "[DEBUG] ";
            const string onErrorPlaceholder = "[not found]";

#region Singleton handling
            static Model() { }
            Model() { }

            public static Model Instance { get => instance; }
#endregion

#region Directories
            string GitBranchBasename { get; set; }
            string GitRepoPath { get => Application.dataPath + @"/../.git/"; }
#endregion

#region Git data holders
            public string GitBranch { get; set; }
            public string GitShortRev { get; set; }
#endregion

            internal string GitRepoSummary
            {
                get => debugLabel + "Modifying last Git revision: "
                       + GitShortRev + " on branch: " + GitBranch;
            }

            internal string UnityProjectInfo { get; private set; }
            internal string WipLabel
            {
                get => "WORK IN PROGRESS - "
                       + "DOES NOT REPRESENT FINAL LOOK OF THE GAME";
            }

            internal void UpdateModel()
            {
                UpdateGitRepoSummary();
                UpdateUnityProjectInfo();
                DebugUtils.Overlay.Controller.NotifiyModelUpdated();
            }

            void UpdateGitRepoSummary()
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
                    GlobalEnabler.LogException(ex);
                }

                // Remove the relative directory that points to a branch file.
                GitBranch = GitBranchBasename?.TrimStart(
                    @"refs/heads/".ToCharArray())?.Trim()
                    ?? onErrorPlaceholder;

                GitShortRev = ReadString(GitRepoPath + GitBranchBasename)?.
                    Substring(0, shortGitRevLength) ?? onErrorPlaceholder;
            }

            void UpdateUnityProjectInfo()
            {
                UnityProjectInfo = debugLabel + Application.productName + " "
                    + Application.version + " (dev) by "
                    + Application.companyName + " using Unity "
                    + Application.unityVersion;
            }

            string ReadString(string path)
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
                    GlobalEnabler.LogException(ex);
                    return null;
                }
            }
        }
    }
}

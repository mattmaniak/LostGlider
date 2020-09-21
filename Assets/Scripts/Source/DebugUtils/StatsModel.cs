#define DEBUG

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace DebugUtils
{
    public class StatsModel
    {
        const string onErrorPlaceholder = "[not found]";

        static string appSummary = Application.productName + " "
                                   + Application.version + " by "
                                   + Application.companyName + " using Unity "
                                   + Application.unityVersion;
        static string gitBranch;
        static string gitRevision;
        static string gitRevisionPath;
        static string gitRepositoryPath = @Application.dataPath + @"/../.git/";

        public static string AppSummary
        {
            get { return appSummary; }
        }

        public static string GitBranch
        {
            get { return gitBranch ?? onErrorPlaceholder; }
            internal set { gitBranch = (value != null)
                           ? value : onErrorPlaceholder; }
        }

        public static string GitRevision
        {
            get { return gitRevision ?? onErrorPlaceholder; }
            internal set { gitRevision = (value != null)
                           ? value : onErrorPlaceholder; }
        }

        public static void ReadGitRepositoryData()
        {
            if (!Directory.Exists(gitRepositoryPath))
            {
                return;
            }
            ReadGitBranch();
            ReadGitRevision();
            DebugUtils.StatsView.UpdateStats();
        }

        static void ReadGitBranch()
        {
            string gitBranchPath = gitRepositoryPath + @"HEAD";

            if (File.Exists(gitBranchPath))
            {
                gitRevisionPath = Encoding.ASCII.GetString(
                    File.ReadAllBytes(gitBranchPath)).Split(' ')[1];

                gitBranch = gitRevisionPath.TrimStart(
                    @"refs/heads/".ToCharArray());
            }
            else
            {
                gitBranch = onErrorPlaceholder;
            }
        }

        static void ReadGitRevision()
        {
            gitRevisionPath = gitRepositoryPath + gitRevisionPath.Trim();

            using (var stream = new StreamReader(gitRevisionPath))
            {
                try
                {
                    gitRevision = stream.ReadLine();
                }
                catch (Exception ex)
                {
                    gitRevision = onErrorPlaceholder;
#if DEBUG
                    Debug.Log(ex);
#endif
                }
            }
        }
    }
}

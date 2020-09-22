#define DEBUG

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace UtilsDebug
{
    public class StatsModel
    {
        const string onErrorPlaceholder = "[not found]";

        static string unityProjectInfo = Application.productName + " "
                                         + Application.version + " by "
                                         + Application.companyName
                                         + " using Unity "
                                         + Application.unityVersion;
        static string gitBranch;
        static string gitRevision;
        static string gitRevisionPath;
        static string gitRepositoryPath = @Application.dataPath + @"/../.git/";

        static string gitRepoData;

        public static string GitBranch
        {
            get { return gitBranch ?? onErrorPlaceholder; }
            internal set
            {
                gitBranch = (value != null) ? value : onErrorPlaceholder;
            }
        }

        public static string GitRevision
        {
            get { return gitRevision ?? onErrorPlaceholder; }
            internal set
            {
                gitRevision = (value != null) ? value : onErrorPlaceholder;
            }
        }

        public static string UnityProjectInfo
        {
            get { return unityProjectInfo; }
        }

        public static string GitRepoData
        {
            get
            {
                return "Modifying last Git revision: "
                + gitRevision.Substring(0, 7) + " on branch: " + gitBranch;
            }
        }

        public static void ReadGitRepositoryData()
        {
            if (!Directory.Exists(gitRepositoryPath))
            {
                return;
            }
            ReadGitBranch();
            ReadGitRevision();

            UtilsDebug.StatsView.UpdateStats();
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

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
        static string gitRepoData;
        static string gitRepoPath = @Application.dataPath + @"/../.git/";

        public static string GitRepoData
        {
            get
            {
                return "Modifying last Git revision: "
                       + ShortenGitRevision() + " on branch: " + gitBranch;
            }
        }

        public static string UnityProjectInfo
        {
            get { return unityProjectInfo; }
        }

        public static void ReadGitRepoData()
        {
            if (!Directory.Exists(gitRepoPath))
            {
                return;
            }
            ReadGitBranch();
            ReadGitRevision();

            UtilsDebug.StatsView.UpdateStats();
        }

        static void ReadGitBranch()
        {
            string gitBranchPath = gitRepoPath + @"HEAD";

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
            gitRevisionPath = gitRepoPath + gitRevisionPath.Trim();

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

        static string ShortenGitRevision()
        {
            const int length = 7;
            return gitRevision.Substring(0, length);
        }
    }
}

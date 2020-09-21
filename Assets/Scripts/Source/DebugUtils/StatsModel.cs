using System.IO;
using System.Text;
using UnityEngine;

namespace DebugUtils
{
    public class StatsModel
    {
        const string onErrorPlaceholder = "[not found]";
        static string gitBranch;
        static string gitRevision;
        static string gitRevisionPath;

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
            ReadGitBranch();
            ReadGitRevision();
            DebugUtils.StatsView.UpdateStats();
        }

        static void ReadGitBranch()
        {
            string branchPath = Application.dataPath + "/../.git/HEAD";

            if (File.Exists(branchPath))
            {
                gitRevisionPath = Encoding.ASCII.GetString(
                    File.ReadAllBytes(branchPath)).Split(' ')[1];

                gitBranch = gitRevisionPath.TrimStart(
                    "refs/heads/".ToCharArray());
            }
            else
            {
                gitBranch = onErrorPlaceholder;
            }
        }

        static void ReadGitRevision()
        {
            gitRevisionPath = Application.dataPath + "/../.git/"
                              + gitRevisionPath;

            // TODO: SOMETHING IS WRONG WITH THE PATH.
            if (File.Exists(gitRevisionPath))
            {
                gitRevision = Encoding.ASCII.GetString(
                    File.ReadAllBytes(gitRevisionPath));
            }
            else
            {
                gitRevision = onErrorPlaceholder;
            }
        }
    }
}

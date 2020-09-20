using System.IO;
using System.Text;
using UnityEngine;

namespace DebugUtils
{
    public class StatsModel
    {
        static string gitBranch;
        static string gitRevision;
        static string gitRevisionPath;

        public static string GitBranch
        {
            get { return gitBranch; }
            internal set { gitBranch = (value != null) ? value : ""; }
        }

        public static string GitRevision
        {
            get { return gitRevision; }
            internal set { gitRevision = (value != null) ? value : ""; }
        }

        public static void ReadGitRepositoryData()
        {
            string branchPath = Application.dataPath + "/../.git/HEAD";

            if (File.Exists(branchPath))
            {
                gitRevisionPath = Encoding.ASCII.GetString(
                    File.ReadAllBytes(branchPath));


                gitRevisionPath = gitRevisionPath.Split(' ')[1];

                gitBranch = gitRevisionPath.TrimStart(
                    "refs/heads/".ToCharArray());
            }
            ReadGitRevision();
        }

        static void ReadGitRevision()
        {
            gitRevisionPath = Application.dataPath + "/../.git/"
                              + gitRevisionPath;

            // TODO: FIX IT.
            if (File.Exists(gitRevisionPath))
            {
                gitRevision = Encoding.ASCII.GetString(
                    File.ReadAllBytes(gitRevisionPath));
            }
        }
    }
}

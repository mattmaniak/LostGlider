﻿#define DEBUG

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace OverlayDebug
{
    internal class OverlayModel
    {
        const string onErrorPlaceholder = "[not found]";

        static string unityProjectInfo = Application.productName + " "
                                         + Application.version + " by "
                                         + Application.companyName
                                         + " using Unity "
                                         + Application.unityVersion;
        static string gitBranch;
        static string gitRevision;
        static string gitBranchBasename;
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

            OverlayDebug.OverlayView.UpdateOverlay();
        }

        static void ReadGitBranch()
        {
            string gitHeadFilename = gitRepoPath + @"HEAD";

            if (File.Exists(gitHeadFilename))
            {
                gitBranchBasename = Encoding.ASCII.GetString(
                    File.ReadAllBytes(gitHeadFilename)).Split(' ')[1].Trim();

                // Remove the relative directory that points to a branch file.
                gitBranch = gitBranchBasename.TrimStart(
                    @"refs/heads/".ToCharArray()).Trim();
            }
            else
            {
                gitBranch = onErrorPlaceholder;
            }
        }

        static void ReadGitRevision()
        {
            string gitBranchFilename = gitRepoPath + gitBranchBasename;

            using (var stream = new StreamReader(gitBranchFilename))
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin.Essentials
{
    public static class VersionTracking
    {
        const string versionTrailKey = "VersionTracking.Trail";
        const string versionsKey = "VersionTracking.Versions";
        const string buildsKey = "VersionTracking.Builds";

        static readonly string sharedName = Preferences.GetPrivatePreferencesSharedName("versiontracking");

        static Dictionary<string, List<string>> versionTrail;

        static VersionTracking()
        {
            InitVersionTracking();
        }

        /// <summary>
        /// Initialize VersionTracking module, load data and track current version
        /// </summary>
        /// <remarks>
        /// For internal use. Usually only called once in production code, but multiple times in unit tests
        /// </remarks>
        internal static void InitVersionTracking()
        {
            IsFirstLaunchEver = !Preferences.ContainsKey(versionsKey, sharedName) || !Preferences.ContainsKey(buildsKey, sharedName);
            if (IsFirstLaunchEver)
            {
                versionTrail = new Dictionary<string, List<string>>
                {
                    { versionsKey, new List<string>() },
                    { buildsKey, new List<string>() }
                };
            }
            else
            {
                versionTrail = new Dictionary<string, List<string>>
                {
                    { versionsKey, ReadHistory(versionsKey).ToList() },
                    { buildsKey, ReadHistory(buildsKey).ToList() }
                };
            }

            IsFirstLaunchForCurrentVersion = !versionTrail[versionsKey].Contains(CurrentVersion) || CurrentVersion != LastInstalledVersion;
            if (IsFirstLaunchForCurrentVersion)
            {
                // Avoid duplicates and move current version to end of list if already present
                versionTrail[versionsKey].RemoveAll(v => v == CurrentVersion);
                versionTrail[versionsKey].Add(CurrentVersion);
            }

            IsFirstLaunchForCurrentBuild = !versionTrail[buildsKey].Contains(CurrentBuild) || CurrentBuild != LastInstalledBuild;
            if (IsFirstLaunchForCurrentBuild)
            {
                // Avoid duplicates and move current build to end of list if already present
                versionTrail[buildsKey].RemoveAll(b => b == CurrentBuild);
                versionTrail[buildsKey].Add(CurrentBuild);
            }

            if (IsFirstLaunchForCurrentVersion || IsFirstLaunchForCurrentBuild)
            {
                WriteHistory(versionsKey, versionTrail[versionsKey]);
                WriteHistory(buildsKey, versionTrail[buildsKey]);
            }
        }

        [Preserve]
        public static void Track()
        {
        }

        public static bool IsFirstLaunchEver { get; private set; }

        public static bool IsFirstLaunchForCurrentVersion { get; private set; }

        public static bool IsFirstLaunchForCurrentBuild { get; private set; }

        public static string CurrentVersion => AppInfo.VersionString;

        public static string CurrentBuild => AppInfo.BuildString;

        public static string PreviousVersion => GetPrevious(versionsKey);

        public static string PreviousBuild => GetPrevious(buildsKey);

        public static string FirstInstalledVersion => versionTrail[versionsKey].FirstOrDefault();

        public static string FirstInstalledBuild => versionTrail[buildsKey].FirstOrDefault();

        public static IEnumerable<string> VersionHistory => versionTrail[versionsKey].ToArray();

        public static IEnumerable<string> BuildHistory => versionTrail[buildsKey].ToArray();

        public static bool IsFirstLaunchForVersion(string version)
            => CurrentVersion == version && IsFirstLaunchForCurrentVersion;

        public static bool IsFirstLaunchForBuild(string build)
            => CurrentBuild == build && IsFirstLaunchForCurrentBuild;

        internal static string GetStatus()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("VersionTracking");
            sb.AppendLine($"  IsFirstLaunchEver:              {IsFirstLaunchEver}");
            sb.AppendLine($"  IsFirstLaunchForCurrentVersion: {IsFirstLaunchForCurrentVersion}");
            sb.AppendLine($"  IsFirstLaunchForCurrentBuild:   {IsFirstLaunchForCurrentBuild}");
            sb.AppendLine();
            sb.AppendLine($"  CurrentVersion:                 {CurrentVersion}");
            sb.AppendLine($"  PreviousVersion:                {PreviousVersion}");
            sb.AppendLine($"  FirstInstalledVersion:          {FirstInstalledVersion}");
            sb.AppendLine($"  VersionHistory:                 [{string.Join(", ", VersionHistory)}]");
            sb.AppendLine();
            sb.AppendLine($"  CurrentBuild:                   {CurrentBuild}");
            sb.AppendLine($"  PreviousBuild:                  {PreviousBuild}");
            sb.AppendLine($"  FirstInstalledBuild:            {FirstInstalledBuild}");
            sb.AppendLine($"  BuildHistory:                   [{string.Join(", ", BuildHistory)}]");
            return sb.ToString();
        }

        static string[] ReadHistory(string key)
            => Preferences.Get(key, null, sharedName)?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

        static void WriteHistory(string key, IEnumerable<string> history)
            => Preferences.Set(key, string.Join("|", history), sharedName);

        static string GetPrevious(string key)
        {
            var trail = versionTrail[key];
            return (trail.Count >= 2) ? trail[trail.Count - 2] : null;
        }

        static string LastInstalledVersion => versionTrail[versionsKey].LastOrDefault();

        static string LastInstalledBuild => versionTrail[buildsKey].LastOrDefault();
    }
}

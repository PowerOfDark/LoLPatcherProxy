using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

namespace LoLPatcherProxy
{
    public static class PatcherUtils
    {
        public enum RELEASE { PROJECT, SOLUTION };

        public static void FakeProjectOK(string Name, string Version)
        {
            DirectoryInfo releases = new DirectoryInfo($"RADS/projects/{Name}/releases");
            string deploy = releases.FullName + $"/{Version}/";

            if (!Directory.Exists(deploy))
                Directory.CreateDirectory(deploy);
            if (!File.Exists(deploy + "S_OK"))
                File.Create(deploy + "S_OK").Close();
            if (!File.Exists(deploy + "releasemanifest"))
                using (WebClient wc = new WebClient())
                    wc.DownloadFile($"{ManifestManager.Program.API_BASE}projects/{Name}/releases/{Version}/releasemanifest", deploy + "releasemanifest");

            /* if we don't create the deploy directory, the patcher will crash for lol_air_client with the following message: 
             *      Failed copying lol.properties file from 
             *      "RADS/projects/lol_air_client_config_eune/releases/0.0.0.44/deploy/lol.properties"
             *      to "RADS/projects/lol_air_client/releases/0.0.1.201/deploy//lol.properties"
            */
            if (!Directory.Exists(deploy + "deploy"))
                Directory.CreateDirectory(deploy + "deploy");

        }

        public static void SetVersion(RELEASE r, string name, string version)
        {
            string release = (r == RELEASE.PROJECT) ? "project" : "solution";
            string path = $"httpd/releases/{ManifestManager.Program.Realm}/{release}s/{name}/releases/releaselisting_{ManifestManager.Program.Region}";
            Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
            List<string> versions = (r == RELEASE.PROJECT) ? ManifestManager.Utils.GetProjectVersions(name) : ManifestManager.Utils.GetSolutionVersions(name);
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                for (int i = versions.IndexOf(version); i < versions.Count; i++)
                {
                    sw.WriteLine(versions[i]);
                }
            }
        }
    }
}

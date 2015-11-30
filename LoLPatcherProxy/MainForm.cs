using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LoLPatcherProxy
{
    public partial class MainForm : Form
    {
        public List<string> AIRVersions, GameVersions;
        public string AIRVersion, GameVersion;
        public bool DisableHanders = true;
        public bool SettingsLoaded = false;
        Dictionary<string, string> GameVersionDictionary = new Dictionary<string, string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void RealmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManifestManager.Program.Realm = RealmComboBox.SelectedItem.ToString();
            RegionComboBox.Enabled = true;
            if (ManifestManager.Program.Realm == "pbe")
            {
                RegionComboBox.Hide();
                ManifestManager.Program.Region = "PBE";
                UpdateVersions();
            }
            else
            {
                this.AirClientComboBox.Enabled = false;
                this.GameClientComboBox.Enabled = false;
                RegionComboBox.Show();
            }
        }

        private void RegionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            ManifestManager.Program.Region = RegionComboBox.SelectedItem.ToString();
            UpdateVersions();
        }


        private Task UpdateVersions(bool loadSettings = false)
        {
            if (ManifestManager.Program.Region != null && ManifestManager.Program.Realm != null)
            {
                GameVersionDictionary.Clear();

                return this.PerformTask(() =>
                {

                    this.AIRVersions = ManifestManager.Utils.GetProjectVersions("lol_air_client");
                    this.GameVersions = ManifestManager.Utils.GetSolutionVersions("lol_game_client_sln");
                    GC.Collect();

                }).ContinueWith((t) =>
                {
                    this.Invoke((Action)delegate
                    {

                        this.AirClientComboBox.Enabled = true;
                        this.GameClientComboBox.Enabled = true;
                        this.AirClientComboBox.DataSource = AIRVersions;
                        this.GameClientComboBox.DataSource = GameVersions;

                    });
                }).ContinueWith((t) => { if (!SettingsLoaded) LoadSettings2(); });

            }
            return null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Load settings


            this.Shown += MainForm_Shown;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (File.Exists("RADS/system/system.cfg"))
                {
                    string txt;
                    using (StreamReader sr = new StreamReader("RADS/system/system.cfg"))
                    {
                        txt = sr.ReadToEnd();
                    }

                    Regex r = new Regex("DownloadPath = \\/releases\\/(.+?)\\r.*Region = (.+)\\r", RegexOptions.Singleline);
                    var m = r.Match(txt);
                    ManifestManager.Program.Realm = m.Groups[1].Value;
                    ManifestManager.Program.Region = m.Groups[2].Value;

                    string airpath = $"httpd/releases/{ManifestManager.Program.Realm}/projects/lol_air_client/releases/releaselisting_{ManifestManager.Program.Region}";
                    if (File.Exists(airpath))
                    {
                        using (StreamReader sr = new StreamReader(airpath))
                        {
                            AIRVersion = sr.ReadLine();
                        }
                    }

                    string gamepath = $"httpd/releases/{ManifestManager.Program.Realm}/solutions/lol_game_client_sln/releases/releaselisting_{ManifestManager.Program.Region}";
                    if (File.Exists(gamepath))
                    {
                        using (StreamReader sr = new StreamReader(gamepath))
                        {
                            GameVersion = sr.ReadLine();
                        }
                    }


                }
                LoadSettings();
            });


        }

        private void LoadSettings()
        {
            RealmComboBox.SelectItem(ManifestManager.Program.Realm);
            if (ManifestManager.Program.Region != null)
            {
                RegionComboBox.SelectItem(ManifestManager.Program.Region);
            }

        }

        private void LoadSettings2()
        {
            DisableHanders = false;
            SettingsLoaded = true;
            if (AIRVersion != null)
            {
                AirClientComboBox.SelectItem(AIRVersion);
            }
            else
                AirClientComboBox_SelectedIndexChanged(this, null);

            if (GameVersion != null)
            {
                GameClientComboBox.SelectItem(GameVersion);
            }
            else
                GameClientComboBox_SelectedIndexChanged(this, null);
        }

        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }

        public void SaveSettings()
        {
            if (Directory.Exists("httpd"))
            {
                try
                {
                    RecursiveDelete(new DirectoryInfo("httpd"));
                }
                catch { }
            }
            string airpath = $"httpd/releases/{ManifestManager.Program.Realm}/projects/lol_air_client/releases/releaselisting_{ManifestManager.Program.Region}";
            string gamepath = $"httpd/releases/{ManifestManager.Program.Realm}/solutions/lol_game_client_sln/releases/releaselisting_{ManifestManager.Program.Region}";

            Directory.CreateDirectory(new FileInfo(airpath).Directory.FullName);
            Directory.CreateDirectory(new FileInfo(gamepath).Directory.FullName);
            using (StreamWriter sw = new StreamWriter(airpath, false))
            {
                for (int i = AIRVersions.IndexOf(AIRVersion); i < AIRVersions.Count; i++)
                {
                    sw.WriteLine(AIRVersions[i]);
                }
            }

            using (StreamWriter sw = new StreamWriter(gamepath, false))
            {
                for (int i = GameVersions.IndexOf(GameVersion); i < GameVersions.Count; i++)
                {
                    sw.WriteLine(GameVersions[i]);
                }
            }

            using (StreamWriter sw = new StreamWriter("RADS/system/system.cfg", false))
            {
                sw.Write($"DownloadPath = /releases/{ManifestManager.Program.Realm}\r\nDownloadURL = 127.0.0.1:80\r\nRegion = {ManifestManager.Program.Region}\r\n");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Application.Exit();
        }

        private void AirClientComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DisableHanders)
                return;
            this.Invoke((Action)delegate { AIRVersion = AirClientComboBox.SelectedItem.ToString(); });

        }

        private void GameClientComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DisableHanders)
                return;
            this.Invoke((Action)delegate
            {
                GameClientVersionLabel.Show();
                GameClientVersionLabel.Text = "Loading data...";
                this.GameVersion = GameClientComboBox.SelectedItem.ToString();
            });
            this.PerformTask((Action)delegate
            {
                string data = "";
                if (GameVersionDictionary.Count == 0)
                {
                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            string[] split;
                            string tmp = wc.DownloadString($"https://raw.githubusercontent.com/PowerOfDark/LoLPatchResolver/master/output/map_{ManifestManager.Program.Realm}_{ManifestManager.Program.Region}.txt");
                            var list = tmp.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string s in list)
                            {
                                split = s.Split(new string[] { " -> " }, StringSplitOptions.None);
                                GameVersionDictionary.Add(split[0], split[1]);
                            }
                        }
                    }
                    catch { }
                }
                if (GameVersionDictionary.ContainsKey(GameVersion))
                {
                    data = GameVersionDictionary[GameVersion];
                }
                else
                {
                    //Do it manually
                    var p = new ManifestManager.SolutionManifest("lol_game_client_sln", GameVersion).Projects[0];
                    data = ManifestManager.Utils.GetClientVersion(p.GetReleaseManifest()) + $" ({p.Version})";
                    p = null;
                    GameVersionDictionary.Add(GameVersion, data);
                }

                GC.Collect();
                this.Invoke((Action)delegate
                {
                    GameClientVersionLabel.Text = data;
                });
            });
        }


    }
}

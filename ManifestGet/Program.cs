using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ManifestGet
{
    class Program
    {
        public static readonly string CurrentDir = Directory.GetCurrentDirectory().ToString();
        public static string AppID = "359550", DepotID = "359551", ManifestID = null, username = null, filename = null, file_or_ID = null; //siege stuff
        static void Main(string[] args)
        {
            Console.WriteLine("\tManifestGet!\n");
            if (args.Length == 0)
            {
                Console.WriteLine("Please type your UserName (your steam login name)");
                username = Console.ReadLine();
                Console.WriteLine("Please type AppID (leave empty to use default: 359550)");
                AppID = Console.ReadLine();
                Console.WriteLine("Please type DepotID (leave empty to use default: 359551)");
                DepotID = Console.ReadLine();
                Console.WriteLine("Please type ManifestID or Path/filename to manifestID list (txt) \n(make sure you write the path and at the end your filename with the extension if you have the file.)");
                file_or_ID = Console.ReadLine();
                if (username == "") { Console.WriteLine("No username, no decryption, try again. Write your username again!"); username = Console.ReadLine(); } //Add anonym user, but it will be not decrypt anything
                if (AppID == "") { AppID = "359550"; } //Siege default
                if (DepotID == "") { DepotID = "359551"; } //Siege default
                //Check if ID or Path
                if (file_or_ID.Contains("txt") && file_or_ID.Contains("\\"))
                {
                    filename = file_or_ID;
                    TryManifest(AppID, filename, DepotID, username);

                }
                else
                {
                    ManifestID = file_or_ID;
                    if (ManifestID == "") { ManifestID = "0"; Console.WriteLine("Manifest 0! | NOT DECRYPTING ANYTHING"); }
                }
                GetManifestStuff(AppID, DepotID, ManifestID, username);
            }

            //Arg magic
            var _AppID = Utils.GetParameter<string>(args, "-AppID") ?? Utils.GetParameter<string>(args, "-app") ?? Utils.GetParameter<string>(args, "-a");
            var _DepotID = Utils.GetParameter<string>(args, "-DepotID") ?? Utils.GetParameter<string>(args, "-depot") ?? Utils.GetParameter<string>(args, "-d");
            var _ManifestID = Utils.GetParameter<string>(args, "-ManifestID") ?? Utils.GetParameter<string>(args, "-manifest") ?? Utils.GetParameter<string>(args, "-m");
            var _filename = Utils.GetParameter<string>(args, "-fl") ?? Utils.GetParameter<string>(args, "-filelist") ?? Utils.GetParameter<string>(args, "-manifestlist");
            var _username = Utils.GetParameter<string>(args, "-username") ?? Utils.GetParameter<string>(args, "-user") ?? Utils.GetParameter<string>(args, "-u");
            //IF to write it
            if (_AppID != null)
            {
                AppID = _AppID;
            }
            if (_DepotID != null)
            {
                DepotID = _DepotID;
            }
            //If its already null and we got null in getparm, we dont need to do anything :P
            ManifestID = _ManifestID;
            filename = _filename;
            username = _username;

            if (_username == null)
            {
                //If username Null,no decryption for that key, asking again.
                Console.WriteLine("No username, no decryption, try again. Write your username!");
                username = Console.ReadLine();
            }
            if (filename != null)
            {
                //Got FileName argument, using that filename to foreach all line
                Console.WriteLine("FileName! Using [" + filename + "] to get manifest list");
                TryManifest(AppID, filename, DepotID, username);
            }
            if (ManifestID != null)
            {
                //Got Manifest from argument, using that ID to get it.
                Console.WriteLine("Manifest [" + ManifestID +"] is avaible!");
                GetManifestStuff(AppID, DepotID, ManifestID, username);
            }
            //Copy Manifest File to Current WorkDir
            CopyManifests();
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }



        private static void GetManifestStuff(string AppID = "359550", string depotID = "359551", string manifestId = "0", string user = "")
        {
            if (manifestId == null)
            {
                return;
            }
            Console.WriteLine(AppID + " " + depotID + " " + manifestId + " " + user);
            //Steamctl manifest filename: 359550_359551_1610834739284564851
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "python.exe";
            process.StartInfo.Arguments = "steamctl\\__main__.py -l debug --user " + user + " depot download -a " + AppID + " -d " + depotID + " -m " + manifestId + " -o \"C:\\TMP\"  -re \"none\"";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();
            Console.WriteLine("\n[GetManifest] Done!\n");
        }
        private static void TryManifest(string AppID, string filename, string DepotID, string username)
        {
            try
            {
                var lines = File.ReadLines(filename);
                if (lines.Count() >= 30)
                {
                    Console.WriteLine("Line count up to 30! | You may get RateLimited!");
                    Console.ReadLine();
                }
                foreach (var line in lines)
                {
                    if (line != null)
                    {
                        line.Trim();
                        Console.WriteLine("Using this ManifestID: " + line);
                        GetManifestStuff(AppID, DepotID, line.ToString(), username);
                    }
                }
            }
            catch (FileNotFoundException exnotfound)
            {
                Console.WriteLine(exnotfound);
                Utils.WriteToFile(exnotfound.Message, "log.txt");
                Console.WriteLine("\nThis basically means that the file could not be found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Utils.WriteToFile(ex.Message, "log.txt");
                Console.WriteLine("\nRandom error, porbably your PC is broken");
            }
        }


        private static void CopyManifests()
        {
            Console.WriteLine("Your PC Name is: " + Environment.UserName);
            //Console.WriteLine(@"C:\Users\" + Environment.UserName + @"\AppData\Local\steamctl\steamctl\Cache\manifests");
            string SteamCTLmanifestsPath = "Users\\" + Environment.UserName + "\\AppData\\Local\\steamctl\\steamctl\\Cache\\manifests";
            string[] fileArray = Directory.GetFiles(@"C:\" + SteamCTLmanifestsPath, "*.*");
            Console.WriteLine("Steamctl manifest path is: " + SteamCTLmanifestsPath);
            //Check if "ManifestFiles" directory exist in curdir, if not create it
            if (!Directory.Exists(CurrentDir + "\\ManifestFiles"))  
            {  
                Directory.CreateDirectory(CurrentDir + "\\ManifestFiles");  
            }
            foreach (string file in fileArray)
            {
                //Get the FileName from the full file
                //Like: C:\asd\Yep.txt => Yep 
                string fileNAME = Path.GetFileName(file);
                if (file.Contains(".manifest"))
                {
                    //Any reason why this line need to be all three place? -slejm
                    if (!File.Exists(CurrentDir + "\\ManifestFiles\\" + fileNAME + ".manifest"))
                    {
                        File.Copy(file + ".manifest", CurrentDir + "\\ManifestFiles\\" + fileNAME + ".manifest");
                    }
                    continue;
                }
                else
                {
                    if (File.Exists(file + ".manifest") == false) //if .manifest file not exist
                    {
                        File.Move(file, file + ".manifest"); //This mean rename in other way, but it keeping the original file
                        //Any reason why this line need to be all three place? -slejm
                        if (!File.Exists(CurrentDir + "\\ManifestFiles\\" + fileNAME + ".manifest"))
                        {
                            File.Copy(file + ".manifest", CurrentDir + "\\ManifestFiles\\" + fileNAME + ".manifest");
                        }
                    }
                    else
                    {
                        //Any reason why this line need to be all three place? -slejm
                        if (!File.Exists(CurrentDir + "\\ManifestFiles\\" + fileNAME + ".manifest"))
                        {
                            File.Copy(file + ".manifest", CurrentDir + "\\ManifestFiles\\" + fileNAME + ".manifest");
                        }
                        Console.WriteLine("File already exist!");
                    }
                }
            }
            Console.WriteLine("\n[CopyFiles] Done!\n");
        }
    }
}

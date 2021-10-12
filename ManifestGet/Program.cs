using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManifestGet
{
    class Program
    {
        public static readonly string CurrentDir = Directory.GetCurrentDirectory().ToString();
        static void Main(string[] args)
        {

            string AppID = "359550", DepotID = "359551", ManifestID = "", username = "", filename = "",file_or_ID = ""; //siege stuff
            Console.WriteLine("You are: " + Environment.UserName);
            Console.WriteLine(@"C:\Users\" + Environment.UserName + @"\AppData\Local\steamctl\steamctl\Cache\manifests");
            string SteamCTLmanifestsPath = "Users\\" + Environment.UserName + "\\AppData\\Local\\steamctl\\steamctl\\Cache\\manifests";
            string[] fileArray = Directory.GetFiles(@"C:\" + SteamCTLmanifestsPath, "*.*");
            Console.WriteLine(SteamCTLmanifestsPath);
            foreach (string file in fileArray)
            {
                if (file.Contains(".manifest"))
                {
                    continue;
                }
                else
                {
                    if (File.Exists(file + ".manifest") == false)
                    {
                        File.Move(file, file + ".manifest");
                        string fileNAME = Path.GetFileName(file);
                        File.Copy(file + ".manifest", CurrentDir + "\\" +  fileNAME + ".manifest"); //just trying
                    }
                    else
                    {
                        Console.WriteLine("File already exist!");
                    }
                }

            }



            if (args.Length == 0)
            {
                Console.WriteLine("Please type your UserName");
                username = Console.ReadLine();
                Console.WriteLine("Please type AppID");
                AppID = Console.ReadLine();
                Console.WriteLine("Please type DepotID");
                DepotID = Console.ReadLine();
                Console.WriteLine("Please type ManifestID or Path/filename to manifestID list (txt)");
                file_or_ID = Console.ReadLine();
                /*
                Console.WriteLine("Please type ManifestID");
                ManifestID = Console.ReadLine();
                Console.WriteLine("Please type filename or path to ManifestID list");
                filename = Console.ReadLine();
                */
                if (username == "") { Console.WriteLine("No username, no decryption, try again. Write your username again!"); username = Console.ReadLine(); } //Add anonym user, but it will be not decrypt anything
                if (AppID == "") { AppID = "359550"; } //Siege default
                if (DepotID == "") { DepotID = "377237"; } //Siege default
                //Check if ID or Path
                if (file_or_ID.Contains("txt") && file_or_ID.Contains("\\"))
                {
                    filename = file_or_ID;
                    try
                    {
                        var lines = File.ReadLines(filename);
                        if (lines.Count() >= 30)
                        {
                            Console.WriteLine("Line count up to 30! | You may get RateLimited!");
                        }
                        foreach (var line in lines)
                        {
                            if (line != "" && line != " ")
                            {

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
                        Utils.WriteToFile(ex.Message,"log.txt");
                        Console.WriteLine("\nRandom error, porbably your PC is broken");
                    }

                }
                else
                {
                    ManifestID = file_or_ID;
                    if (ManifestID == "") { ManifestID = "0"; Console.WriteLine("Manifest 0! | NOT DECRYPTING ANYTHING"); }
                }


                GetManifestStuff(AppID, DepotID, ManifestID, username);
            }
            //TODO: Arg parser
            if (Utils.HasParameter(args, "fl"))
            {

            }
            




            Console.ReadLine();
            Environment.Exit(0);
        }



        private static void GetManifestStuff(string AppID = "359550", string depotID = "359551", string manifestId = "0", string user = "")
        {
            Console.WriteLine(AppID + " " + depotID + " " + manifestId + " " + user);
            /*string manifestPath,byte[] deckey*/
            //359550_359551_1610834739284564851
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "python.exe";
            process.StartInfo.Arguments = "steamctl\\__main__.py -l debug --user " + user + " depot download -a " + AppID + " -d " + depotID + " -m " + manifestId + " -o \"C:\\TMP\"  -re \"none\"";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();
            Console.WriteLine("\nDone!\n");
        }
    }
}

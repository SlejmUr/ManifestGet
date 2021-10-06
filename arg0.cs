            string AppID = "359550", DepotID = "359551", ManifestID = "", username = "", filename = "",file_or_ID = ""; //siege stuff
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
                if (username == "") { username = Environment.UserName; } //Add anonym user, but it will be not decrypt anything
                if (AppID == "") { AppID = "359550"; } //Siege default
                if (DepotID == "") { DepotID = "359551"; } //Siege default (Content)
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

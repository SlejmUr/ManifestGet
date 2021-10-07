            string SteamCTLmanifestsPath = "Users\\" + Environment.UserName + "\\AppData\\Local\\steamctl\\steamctl\\Cache\\manifests";
            //Windows only path ^^ should edit the steamctl to actually using different path or currently working dir
            string[] fileArray = Directory.GetFiles(@"C:\" + SteamCTLmanifestsPath, "*.*");
            Console.WriteLine(SteamCTLmanifestsPath);
            foreach (string file in fileArray)
            {
                if (file.Contains(".manifest")) //filtering manifest
                {
                    continue; //skip, checking the next file
            //Todo: add copy manifest if not exist on currently workdir
                }
                else
                {
                    if (File.Exists(file + ".manifest") == false) //probably it be good if !File.Exists() in there 
                    {
                        File.Move(file, file + ".manifest"); //this way is for renaming the manifest
                        string fileNAME = Path.GetFileName(file); //give back filenames without needing to replace strings inside it.
                        File.Copy(file + ".manifest", CurrentDir + "\\" +  fileNAME + ".manifest"); //just trying (it works, but it spammy
                    }
                    else
                    {
                        Console.WriteLine("File already exist!");
                    }
                }

            }

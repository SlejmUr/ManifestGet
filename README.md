# First Start
Getting manifest from Steam using C# and Steamctl (Python)\
Made by LordElias, SlejmUr and JVAV

### This is for the Main Branch!

# Steamctl Installing/Downloading
1. You need python, install it. (Recommending Python 3)
2. After installed and added to path, use this command\
`pip install steamctl`
3. You are completely done!\
Check steamctl for this python production!
[This Link!](https://github.com/ValvePython/steamctl)

# ManifestGet Installing/Downloading
Install from the Release section, unzip.\
Download newest steamctl, make a folder to the unzipped folder\
Name it steamctl and unzip that too in this folder.\
So:
ManifestGet/steamctl/ (all the unzipped things there, like `__init__.py`, etc)

Note: Maybe in the future it install python and install steamctl.

Congratulations! You did it!

## Using
Start the .exe (on windows) [Mac, Linux dont think it supported, not tested]

Type your Username\
Type AppID\
Type DepotID\
Type ManifestID or the path that contains manifestID's in txt file\
Use the path like: `C:\Sub\Path\ManifestList.txt` or `C:/Sub/Path/ManifestList.txt`\
(All infos can be checked on Steamdb.info)

And wait for steamctl finish it!

If it finished, all manifest is in your currently working directory + ManifestFiles\
So like : `D:\ManifestGet\ManifestFiles`

### Parameters
Parameter | Description
--------- | -----------
-a or -app or -AppID \<#> | AppID of the wanted App
-d or -depot or -DepotID \<#> | DepotID of the App
-m or -manifest or -ManifestID \<#> | ManifestID of the depot
-fl or -filelist \<path> | path of the manifestID's in txt

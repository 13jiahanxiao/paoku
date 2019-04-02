using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PrefabLabSettings
{
    public const string ERROR_MISSING_SETTINGS_FILE = "Prefab Lab Error: Couldn't load the settings file because it's missing. Creating and using default settings instead.";
    public const string ERROR_READING_SETTINGS_FILE = "Prefab Lab Error: Couldn't read the contents of the settings file because it's corrupt or been modified by something other then the Prefab Lab." +
        "\n\r" + "To fix this delete Settings.cfg and hit the \"Apply\" button in the Prefab Lab window";
    public const string WARNING_LOST_OBJECTS_FOUND = "Prefab Lab Warning: There are {0} objects found which don't have a parent anymore because it has been deleted in the model." +
        "\n\r" +
        "To recover these objects, browse to the gameobject \"{1}\" in the prefab {2}.";

    private const string SRC_FOLDER = "SourceFolder";
    private const string DEST_FOLDER = "DestinationFolder";
    private const string PREFAB_SUFFIX = "PrefabSuffix";
    private const string MODEL_METADATA = "ModelMetadata";
    private const string POST_PROCESS_ORDER = "PostprocessOrder";

    private const string FILENAME_SETTINGS_FILE = "Settings.cfg";
    private const string FILENAME_PREFABLABSETTINGS_FILE = "PrefabLabSettings.cs";

    //A dictionary is convinient for looping thru each setting while reading/writing to the settingsfile
    private Dictionary<string, string> settings = new Dictionary<string, string>();

    public PrefabLabSettings(string srcFolder, string dstFolder, string pSuffix, string mdlMetaData, int processOrder)
    {
        this.SourceFolder = srcFolder;
        this.DestinationFolder = dstFolder;
        this.PrefabSuffix = pSuffix;
        this.ModelMetadata = mdlMetaData;
        this.PostProcessOrder = processOrder;
    }

    public static PrefabLabSettings Defaults
    {
        get
        {
            string srcFolder = "Models";
            string dstFolder = "Prefabs";
            string pSuffix = "_prefab";
            string mdlMetadata = "_model";
            int processOrder = 1;

            return new PrefabLabSettings(srcFolder, dstFolder, pSuffix, mdlMetadata, processOrder);
        }
    }

    public void ReadSettingsFromDisk()
    {
        settings.Clear();

        string fp = Filepath;

        if (File.Exists(fp))
        {
            using (StreamReader reader = new StreamReader(fp))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] keypairvalue = line.Split('=');

                    if (keypairvalue.Length == 2)
                        settings.Add(keypairvalue[0], keypairvalue[1]);
                    else
                    {
                        //Error reading settings file.
                        Debug.LogError(ERROR_READING_SETTINGS_FILE);
                    }
                }
            }
        }
        else
        {
            Debug.LogError(PrefabLabSettings.ERROR_MISSING_SETTINGS_FILE);

            //Create new file and use defaults instead
            settings = Defaults.settings;
            WriteSettingsToDisk();
        }
    }

    public void WriteSettingsToDisk()
    {
        string fp = Filepath;

        if (!Directory.Exists(fp))
            Directory.CreateDirectory(new FileInfo(fp).DirectoryName);

        using (StreamWriter writer = new StreamWriter(fp))
        {
            foreach (var setting in settings)
            {
                writer.WriteLine(setting.Key + "=" + setting.Value);
            }

            writer.Flush();
            writer.Close();
        }
    }

    #region Properties

    /// <summary>
    /// Only use this importer for this folder. (relative) Example: "Models" (absolute => Project/Assets/Models)
    /// </summary>
    public string SourceFolder
    {
        get { return settings[SRC_FOLDER]; }
        set { settings[SRC_FOLDER] = value; }
    }

    /// <summary>
    /// Create the prefabs in this folder. (relative) Example: "Prefabs" (absolute => Project/Assets/Prefabs)
    /// </summary>
    public string DestinationFolder
    {
        get { return settings[DEST_FOLDER]; }
        set { settings[DEST_FOLDER] = value; }
    }

    public string PrefabSuffix
    {
        get { return settings[PREFAB_SUFFIX]; }
        set { settings[PREFAB_SUFFIX] = value; }
    }

    public string ModelMetadata
    {
        get { return settings[MODEL_METADATA]; }
        set { settings[MODEL_METADATA] = value; }
    }

    public int PostProcessOrder
    {
        get
        {
            int result;
            if (!int.TryParse(settings[POST_PROCESS_ORDER], out result))
                result = PrefabLabSettings.Defaults.PostProcessOrder;

            return result;
        }

        set
        {
            settings[POST_PROCESS_ORDER] = value.ToString();
        }
    }

    /// <summary>
    /// Expensive call!!!
    /// </summary>
    private string Filepath
    {
        get
        {
            string dirName = FindDirectoryOfPrefabLabSettings(Application.dataPath);

            if (dirName != string.Empty)
                return dirName + "/" + FILENAME_SETTINGS_FILE;
            else
                return string.Empty;
        }
    }

    #endregion

    //Recursive find of the
    private string FindDirectoryOfPrefabLabSettings(string startDir)
    {
        DirectoryInfo di = new DirectoryInfo(startDir);
        FileInfo[] fis = di.GetFiles(FILENAME_PREFABLABSETTINGS_FILE);

        if (fis.Length < 1)
        {
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                string result = FindDirectoryOfPrefabLabSettings(d.FullName);
                if (result != string.Empty)
                    return result;
            }
        }
        else
        {
            return fis[0].DirectoryName;
        }

        return string.Empty;
    }
}
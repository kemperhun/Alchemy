using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using CI.QuickSave;
[SerializeField]
public class Config
{
    [XmlElement("Language")]
    public string Lang;
    [XmlElement("Sound_Volume")]
    public float SoundVolume;
    [XmlElement("Music_Volume")]
    public float MusicVolume;
    [XmlElement("LoadDistance")]
    public int LoadDistance;
}


public class GameManager : MonoBehaviour
{
    [SerializeField]
    Config Config;
    [SerializeField]
    string SaveName;
    [SerializeField]
    WorldSave save;
    public HexGrid HexGrids;

    private void Awake()
    {
        if (!Directory.Exists(Application.dataPath + "/Configs/"))
        {
            CreateConfig();
        }
        else
        {
            LoadConfigs(Application.dataPath + "/Configs/Config");
        }
        DontDestroyOnLoad(this);
    }

    #region menuStuff
    public void SetLangName(string l)
    {
        Config.Lang = l;
        SaveConfigs(Application.dataPath + "/Configs/" + "/Config");
    }
    public string GetLangName()
    {
        return Config.Lang;
    }
    public void SetMusicVolume(Slider slider)
    {
        Config.MusicVolume = slider.value;
        SaveConfigs(Application.dataPath + "/Configs/" + "/Config");
    }
    public float GetMusicVolume()
    {
        return Config.MusicVolume;
    }
    public void SetLoadDistance(Slider slider)
    {
        Config.LoadDistance = (int)slider.value;
        SaveConfigs(Application.dataPath + "/Configs/" + "/Config");
    }
    public int GetLoadDistance()
    {
        return Config.LoadDistance;
    }
    public void SetSoundVolume(Slider slider)
    {
        Config.SoundVolume = slider.value;
        SaveConfigs(Application.dataPath + "/Configs/" + "/Config");
    }
    public float GetSoundVolume()
    {
        return Config.SoundVolume;
    }
    public void CreateConfig()
    {
        string dir = Application.dataPath + "/Configs/";
        Directory.CreateDirectory(dir);
        Config = new Config();
        Config.Lang = "English";
        Config.SoundVolume = 0.5f;
        Config.MusicVolume = 0.5f;
        SaveConfigs(dir + "/Config");
        LoadConfigs(Application.dataPath + "/Configs/Config");
    }
    public void SaveConfigs(string path)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(Config));
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(fs, Config);
        }
    }
    public void LoadConfigs(string path)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(Config));
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            Config = (Config)formatter.Deserialize(fs);
        }
    }

    public void SetSaveName(string name)
    {
        SaveName = name;
    }

    public string GetSaveName()
    {
        return SaveName;
    }
    #endregion

    #region GameStuff

    public void SetSaveState(WorldSave save)
    {
        this.save = save;
    }
    public WorldSave GetSaveState()
    {
        return save;
    }

    public void SaveChunks(string path, HexGrid chunks)
    {
        string chunksPath = Application.dataPath + "/Saves/" + GetSaveName() + "/chunks/main/" + chunks;
        QuickSaveRoot.Save(chunksPath, chunks.cells);
    }
    public string[] LoadChunksName()
    {
        string chunksPath = Application.dataPath + "/Saves/" + GetSaveName() + "/chunks/main";
        return Directory.GetDirectories(chunksPath);
    }
    public void SaveChunk(HexGrid chunks)
    {
        string chunksPath = Application.dataPath + "/Saves/" + GetSaveName() + "/chunks/main/" + chunks.name;
        QuickSaveRoot.Save(chunksPath, chunks.cells);
    }
    public HexGrid LoadChunk(string name)
    {
        string chunksPath = Application.dataPath + "/Saves/" + GetSaveName() + "/chunks/main/" + name;
        if (QuickSaveRoot.Exists(chunksPath))
        {
            QuickSaveRoot.Load<MassCells>(chunksPath);
            MassCells MassCells = QuickSaveRoot.Load<MassCells>(chunksPath);
            HexGrids.cells = MassCells;
            HexGrids.name = MassCells.name;
            return HexGrids;
        }
        return null;
    }
    public int GetRandomSeed()
    {
        return save.LevelSeed;
    }
    #endregion
}

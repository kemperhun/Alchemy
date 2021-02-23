using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

[System.Serializable]
public struct WorldSave
{
    [XmlElement("Save Name")] //для читаемости xml
    public string SaveName;
    [XmlElement("Generation_Gome")]
    public int GenerationGome;
    [XmlElement("PlayerPos")]
    public Vector3 PlayerPosition;
    [XmlElement("LevelSeed")]
    public int LevelSeed;
    public WorldSave(string SaveName, int GenerationGome, Vector3 PlayerPosition, int LevelSeed)
    {
        this.SaveName = SaveName;
        this.GenerationGome = GenerationGome;
        this.PlayerPosition = PlayerPosition;
        this.LevelSeed = LevelSeed;
    }
}
[System.Serializable]
public class WorldList
{
    [XmlArray("World"), XmlArrayItem("Structure")]
    public List<WorldSave> WorldSaves;
}
public class SaveWorldSystem : MonoBehaviour
{
    public InputField worldName;
    public Button Save;
    public GameManager GameManager;
    void Awake()
    {
        worldName.text = "New World";
        if (Directory.Exists(Application.dataPath + "/Saves/" + worldName.text))
        {
            int nmb = 1;
            while (Directory.Exists(Application.dataPath + "/Saves/" + worldName.text + nmb))
            {
                nmb++;
            }
            worldName.text = "New World " + nmb;
        }
        //GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        Save.onClick.AddListener(TaskOnClickSave);
    }
    public void TaskOnClickSave()
    {
        string dir = Application.dataPath + "/Saves/";
        if (!Directory.Exists(dir + worldName.text))
        {
            Directory.CreateDirectory(Path.Combine(dir, worldName.text));
            Directory.CreateDirectory(Path.Combine(dir + "/" + worldName.text));
            Directory.CreateDirectory(Path.Combine(dir + "/" + worldName.text + "/chunks", "main"));
            Directory.CreateDirectory(Path.Combine(dir + "/" + worldName.text + "/chunks", "preWar"));
            CreateWorld(Path.Combine(dir, worldName.text + "/") + "world.xml", worldName.text);
            Application.LoadLevel(1);
        }
    }
    public void CreateWorld(string path, string name)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(WorldSave));
        int seed = Random.Range(int.MinValue, int.MaxValue);
        WorldSave World = new WorldSave(name, 1, transform.position, seed);
        GameManager.SetSaveState(World);
        GameManager.SetSaveName(name);
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(fs, World);
            //print("Мир сохранён в: " + path);
        }
    }
    public WorldSave LoadWorld(string path)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(WorldSave));
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            WorldSave World = (WorldSave)formatter.Deserialize(fs);
            //print("Мир загружен из :" + path);
            return World;
        }
    }
    public void ChangeWorldName(Text text)
    {
        GameManager.SetSaveName(text.text);
    }
}

using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

public class GameLocaleManager : MonoBehaviour
{
    [SerializeField]
    ListItems items;
    [SerializeField]
    GameManager GameManager;
    [SerializeField]
    bool saveReq;
    void Awake()
    {
        string dir = Application.dataPath + "/Language/" + "items/";
        if (saveReq)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            SaveItemList(dir + GameManager.GetLangName());
        }
        else if (Path.GetFileName(dir + GameManager.GetLangName()) != null)
        {
            LoadItemList(dir + GameManager.GetLangName());
        }
        else
        {
            LoadItemList(dir + "English");
        }
    }

    public void SaveItemList(string path)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(ListItems));
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(fs, items);
            //print("Объект сериализован в : " + path);
        }
    }

    public void LoadItemList(string path)
    {
        items = null;
        XmlSerializer formatter = new XmlSerializer(typeof(ListItems));
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            items = (ListItems)formatter.Deserialize(fs);
            print("Объект десериализован из :" + path);
        }
    }

}

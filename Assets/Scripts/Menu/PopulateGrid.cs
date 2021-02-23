using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PopulateGrid : MonoBehaviour
{
    [SerializeField]
    GameObject UIWorldList;
    [HideInInspector]
    public GameObject[] MenuSaves;
    [SerializeField]
    SaveWorldSystem SWS;
    [SerializeField]
    MenuManager langManager;
    [SerializeField]
    Text Emptytext;
    [SerializeField]
    GameManager GameManager;
    void Awake()
    {
        string SavePath = Application.dataPath + "/Saves/";
        string[] Saves = Directory.GetDirectories(SavePath);
        MenuSaves = new GameObject[Saves.Length];
        if (Saves != null)
        {
            Emptytext.gameObject.SetActive(false);
            Populate(Saves);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    void Populate(string[] Saves)
    {
        for (int i = 0; i < Saves.Length; i++)
        {
            MenuSaves[i] = Instantiate(UIWorldList, transform);
            MenuWorldCard menuWorldCard = MenuSaves[i].GetComponent<MenuWorldCard>();
            DirectoryInfo dirInfo = new DirectoryInfo(Saves[i]);
            menuWorldCard.SetWorldName(dirInfo.Name);
            MenuSaves[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { GameManager.SetSaveName(dirInfo.Name); });
            MenuSaves[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { GetComponent<PopulateGrid>().LoadLevel(); });
            WorldSave WorldTmp = SWS.LoadWorld(Saves[i] + "/world.xml");
            MenuSaves[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { GameManager.SetSaveState(WorldTmp); });

            string dir = Application.dataPath + "/Language/menu/";

            langManager.LoadItemList(dir + Path.GetFileName(Directory.GetFiles(dir)[langManager.langSetting.value]));

            menuWorldCard.SetTotalGeneration(langManager.Menu.TranslationsLang[langManager.Menu.TranslationsLang.Count - 1].name + WorldTmp.GenerationGome.ToString());

            //menuWorldCard.SetTotalPlayedTime(langManager.Menu.TranslationsLang[langManager.Menu.TranslationsLang.Count - 2].name + WorldTmp.SaveName.ToString());

        }

    }
}
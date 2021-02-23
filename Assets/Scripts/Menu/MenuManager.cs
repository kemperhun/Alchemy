using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

public class MenuManager : MonoBehaviour
{
    public ListMenu Menu;//список текста локализации
    [SerializeField]
    GameManager GameManager;
    [SerializeField]
    Text[] MenuElement;//список объектов для перевода
    [SerializeField]
    bool SaveReq;//если требуется обновление файла перевода
    [SerializeField]
    Slider[] audioSliders;//если требуется обновление файла перевода
    public Dropdown langSetting;
    void Start()
    {
        string dir = Application.dataPath + "/Language/" + "/menu/";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            SaveReq = true;
        }//создать файл, если он не существует 
        if (SaveReq)
        {
            SaveItemList(Application.dataPath + "/Language/" + "/menu/" + "English");
        }
        else
        {
            LoadLangfromGameManager();
        }
        //Сохраняет и загружает английский
        audioSliders[0].value = GameManager.GetSoundVolume();
        audioSliders[1].value = GameManager.GetMusicVolume();
        LoadItemList(Application.dataPath + "/Language/" + "/menu/" + GameManager.GetLangName());
        lang = Directory.GetFiles(dir);
        for (int i = 0; i < lang.Length; i++)
        {
            langSetting.options.Add(new Dropdown.OptionData(Path.GetFileName(lang[i])));
        }
        //заполняет поле выбора языка
    }
    string[] lang;//список файлов из папки локализации
    public void UpdateLang()
    {
        LoadItemList(Application.dataPath + "/Language/" + "/menu/" + Path.GetFileName(lang[langSetting.value]));
        GameManager.SetLangName(Path.GetFileName(lang[langSetting.value]));
        for (int i = 0; i < MenuElement.Length; i++)
        {
            MenuElement[i].text = Menu.TranslationsLang[i].name;
        }
    }//загружает язык и обновляет текст в меню
    public void LoadLangfromGameManager()
    {
        LoadItemList(Application.dataPath + "/Language/" + "/menu/" + GameManager.GetLangName());
        for (int i = 0; i < MenuElement.Length; i++)
        {
            MenuElement[i].text = Menu.TranslationsLang[i].name;
        }
    }//загружает язык и обновляет текст в меню
    public void SaveItemList(string path)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(ListMenu));
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(fs, Menu);
        }
    }//сохранение списка текста
    public void LoadItemList(string path)
    {
        Menu = null;
        XmlSerializer formatter = new XmlSerializer(typeof(ListMenu));
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            Menu = (ListMenu)formatter.Deserialize(fs);
        }
    }//загрузка списка текста
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

class ConfigData
{
    /// <summary>
    /// 返回待机页时间
    /// </summary>
    public int Backtime;

    /// <summary>
    /// UDP端口号
    /// </summary>
    public int Port;
}


public class Config : MonoBehaviour
{
    public static Config Instance;

    private ConfigData configData  = new ConfigData();

    private string File_name = "config.txt";
    private string Path;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Path = Application.streamingAssetsPath + "/" + File_name;

        if (FileHandle.Instance.IsExistFile(Path))
        {
            string st = FileHandle.Instance.FileToString(Path);
            configData = JsonConvert.DeserializeObject<ConfigData>(st);
        }
        else
        {
            InitData();
        }

        LogMsg.Instance.Log("Time==" + configData.Backtime + "     " + "Port==" + configData.Port);
    }

    private void OnDestroy()
    {
        if(!FileHandle.Instance.IsExistFile(Path))
        {
            string st = JsonConvert.SerializeObject(configData);
            FileHandle.Instance.SaveFile(st, Path);
        }


#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void InitData()
    {
        configData.Backtime = 30;
        configData.Port = 8060;
    }
}

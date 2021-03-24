using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using System.IO;

public class WaitPanel : BasePanel
{

    protected override void Awake()
    {
        base.Awake();
        //TexturePath = Application.streamingAssetsPath + "/Photos";
        //TexturePathList = GetImagePath();
        //int num = 0;
        //if (TexturePathList.Count > 0)
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //        if (num >= TexturePathList.Count)
        //        {
        //            num = 0;
        //        }
        //        LoadByIO(TexturePathList[num]);
        //        num++;
        //    }
        //}
    }

    protected override void Start()
    {
        base.Start();



    }

    //public Text text;
    public override void InitFind()
    {
        base.InitFind();

        

        //text = FindTool.FindChildComponent<Text>(transform, "Text");
    }

    public override void Open()
    {
        base.Open();
        //text.text = "打开UdpPanel面板";
        //EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, MTFrame.EventType.DataToPanel.ToString(), callback);
    }

    //private void callback(EventParamete parameteData)
    //{
    //    //如果需要判断事件名就用parameteData.EvendName
    //    //如果需要判断数据内容就用string data = parameteData.GetParameter<string>()[0];
    //    //要接收什么类型的数据就定义什么类型的数据，这里只会获取你选择的数据类型的数据
    //    string data = parameteData.GetParameter<string>()[0];
    //    text.text = data;
    //}

    public override void Hide()
    {
        base.Hide();
        //EventManager.RemoveListener(GenericEventEnumType.Message, MTFrame.EventType.DataToPanel.ToString(), callback);

        //text.text = "AAAAAAAA";
    }

    //public void LoadByIO(string path)
    //{
    //    //创建文件读取流
    //    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
    //    fileStream.Seek(0, SeekOrigin.Begin);
    //    //创建文件长度缓冲区
    //    byte[] bytes = new byte[fileStream.Length];
    //    //读取文件
    //    fileStream.Read(bytes, 0, (int)fileStream.Length);
    //    //释放文件读取流
    //    fileStream.Close();
    //    fileStream.Dispose();
    //    fileStream = null;
    //    //创建Texture
    //    int width = 336;
    //    int height = 225;
    //    Texture2D texture2D = new Texture2D(width, height);
    //    texture2D.LoadImage(bytes);
    //    GameObject gameObject = Instantiate(RawImagePrefab, layoutGroup.gameObject.transform);
    //    gameObject.transform.localPosition = Vector3.zero;
    //    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    //    gameObject.GetComponent<RawImage>().texture = texture2D;

    //}

    //private List<string> GetImagePath()
    //{
    //    List<string> filePaths = new List<string>();
    //    string imgtype = "*.JPG|*.PNG";
    //    string[] ImageType = imgtype.Split('|');
    //    for (int i = 0; i < ImageType.Length; i++)
    //    {
    //        //获取unity根目录下的图片文件夹下的所有文件的路径 路径+ 名称全部存储在字符串数组中
    //        string[] dirs = Directory.GetFiles(TexturePath, ImageType[i]);
    //        //
    //        for (int j = 0; j < dirs.Length; j++)
    //        {
    //            filePaths.Add(dirs[j]);
    //            //Debug.Log("图片路径为     " + dirs[j]);
    //        }
    //        //Debug.Log("一共读取到" + dirs.Length + "张图片");
    //    }
    //    return filePaths;
    //}



}

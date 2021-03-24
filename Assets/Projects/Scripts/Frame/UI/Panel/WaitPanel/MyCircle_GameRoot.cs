using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using MyCirle;

public enum ImageType {
    Big,
    Move,
    Null
}

public class MyCircle_GameRoot : MonoBehaviour {

    public static MyCircle_GameRoot instance;
    //存放当前加载进来的图片和最新留言的图片
    public  List<Texture2D> showImageList = new List<Texture2D>();

    //图片类型的索引值
    int parentIndex = 0;

    bool isStartShow = false;

    //加载的图片list的索引值
    int imageListIndex = 0;

    List<Vector3> initPos = new List<Vector3>();

    bool isstart = true;

    //行
    int row = 10;
    //列
    int column = 7;

    float XOffset =5;

    float YOffset = 5;

   // [Header("起始位置")]
     Vector3 startPos=new Vector3 (4600,130,0);

    Vector3 originalPos=Vector3.zero;

    float createTimer = 0;

    float timer = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //(XOffset +320)/3/(1/Time.fixedDeltaTime)     
        //（X轴偏移量+移动图片宽）/移动速度*预设间隔的帧数（除以具体一秒的帧数(图片移动在FixedUpdate中进行)获取间隔时间（单位：秒））
        // print((XOffset + 320) / 3 / (1 / Time.fixedDeltaTime));
        createTimer = (XOffset + 320) / (2f * (1 / Time.fixedDeltaTime));
        StartCoroutine(LoadImage());
    }


    private void FixedUpdate()
    {
        if (isStartShow)
        {
            timer += Time.deltaTime;
            if (timer >= createTimer)
            {
                timer = 0;
                for (int i = 0; i < initPos.Count; i++)
                {
                    GameObject newImage = MessageEvent.instance.GetNewMoveImage();
                    newImage.SetActive(true);
                    newImage.GetComponent<MyCircle_ImageItem>().currentPos = initPos[i];
                    newImage.GetComponent<MyCircle_ImageItem>().OnEnter();
                }
            }
        }
    }


    //加载图片
    IEnumerator LoadImage()
    {
        yield return null;

        string path = Application.streamingAssetsPath + "/Photos";

        List<FileInfo> currentFileImageInfo = FileManagerScript.instance.GetAllFileinfoFromPath(path);

        for (int j = 0; j < currentFileImageInfo.Count; j++)
        {
            LoadSinglePic(currentFileImageInfo[j].FullName, currentFileImageInfo[j].Name);
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < row; i++) //行
        {
            Vector3 resetPos = Vector3.zero;
            for (int j = 0; j < column; j++) //列
            {
                //i*200f展示错开效果  320 180 移动图片的宽、高
                Vector3 newPos = new Vector3(j * (320f + XOffset) - Screen.width / 2 + startPos.x + i * 200f, i * (180f + YOffset) - Screen.height / 2 + startPos.y, 0);
                GameObject newImage = MessageEvent.instance.GetNewMoveImage();
                newImage.name = i + "_" + j;
                resetPos = new Vector3((column - 1) * (320f + XOffset) - Screen.width / 2 + startPos.x + i * 200f, i * (180f + YOffset) - Screen.height / 2 + startPos.y, 0);
                newImage.GetComponent<MyCircle_ImageItem>().currentPos = newPos;
                newImage.transform.localPosition = newPos;
                newImage.GetComponent<MyCircle_ImageItem>().OnEnter();
            }
            initPos.Add(resetPos);
        }
        isStartShow = true;
        //  print(initPos.Count);
    }

    //加载单张本地图片
    private void LoadSinglePic(string path, string texName)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //System.Drawing.Image image = System.Drawing.Image.FromStream(fileStream);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        int width = 202;
        int height = 135;
        Texture2D tmp = new Texture2D(width, height);
        tmp.LoadImage(bytes);
        // Sprite sprite = Sprite.Create(tmp, new Rect(0, 0, tmp.width, tmp.height), new Vector2(0.5f, 0.5f));  //0.5  像素点
        tmp.name = texName;
        showImageList.Add(tmp);
        bytes = null;
        fileStream = null;

        tmp = null;
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }


    public Texture2D GetImage()
    {
        if (showImageList.Count > 0)
        {
            imageListIndex = UnityEngine.Random.Range(0, 9);
            Texture2D newSprite = showImageList[imageListIndex];
            //imageListIndex++;
            //if (imageListIndex >= showImageList.Count)
            //{
            //    imageListIndex = 0;
            //}
            return newSprite;
        }
        else
        {
            return null;
        }
    }

}

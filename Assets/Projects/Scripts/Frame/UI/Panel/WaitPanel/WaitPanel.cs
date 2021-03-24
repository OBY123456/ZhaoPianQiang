using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using System.IO;
using MyCirle;

public enum ImageType
{
    Big,
    Move,
    Null
}

public class WaitPanel : BasePanel
{
    public static WaitPanel instance;

    public List<Texture2D> showImageList = new List<Texture2D>();

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

    float XOffset = 5;

    float YOffset = 5;

    // [Header("起始位置")]
    Vector3 startPos = new Vector3(4600, 130, 0);

    Vector3 originalPos = Vector3.zero;

    float createTimer = 0;

    float timer = 0;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        createTimer = (XOffset + 320) / (2f * (1 / Time.fixedDeltaTime));
        StartCoroutine(LoadImage());

    }

    public static bool IsInCircle(Vector2 CirclePoint, float r, Vector2 point)
    {
        return Mathf.Sqrt((point.x - CirclePoint.x) * (point.x - CirclePoint.x) + (point.y - CirclePoint.y) * (point.y - CirclePoint.y)) <= r;
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
                    GameObject newImage = MyCirle.MessageEvent.instance.GetNewMoveImage();
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
                GameObject newImage = MyCirle.MessageEvent.instance.GetNewMoveImage();
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

        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        int width = 240;
        int height = 135;
        Texture2D tmp = new Texture2D(width, height);
        tmp.LoadImage(bytes);

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

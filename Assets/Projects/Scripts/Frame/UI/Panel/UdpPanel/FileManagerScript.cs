using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileManagerScript : MonoBehaviour {

    public static FileManagerScript instance;

    string path;

    private void Awake()
    {
        instance = this;
        path = Application.streamingAssetsPath + "/Imags";
    }
    
    /// <summary>
    /// 得到当前文件夹根目录下所有的文件夹或文件名字
    /// path为 2019 或 2019/3 或 2019/3/15格式
    /// </summary>
    /// <param name="path"></param>
   public List<string> GetFolderRootFileName(string path)
    {
        List<string> fileNameList = new List<string>();

        string[] directoryEntries = Directory.GetFileSystemEntries(path);

        // DirectoryInfo dir = new DirectoryInfo(dirPath);
        // FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly); //获取所有文件信息       
        for (int i = 0; i < directoryEntries.Length; i++)
        {
            string fileName = directoryEntries[i].Split('\\')[1];

            if (fileName.EndsWith(".meta"))
            {
                continue;
            }
            fileNameList.Add(fileName);
        }
        return fileNameList;
    }


    /// <summary>
    /// 得到当前文件夹下所有的子文件夹内的所有文件的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
   public List<FileInfo> GetAllFileinfoFromPath(string path)
    {
        List<FileInfo> fileinfo= new List<FileInfo>();

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories); //获取所有文件信息       
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta"))
            {
                continue;
            }
          //  print(files[i].FullName); //文件路径
          //  print(files[i].Name);     //文件名字
            fileinfo.Add(files[i]);
        }
        return fileinfo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyCirle
{
    public class MessageEvent : MonoBehaviour
    {

        public static MessageEvent instance;

        public List<MyCircle_ImageItem> moveImageList = new List<MyCircle_ImageItem>();
        public List<BigImage> bigImageList = new List<BigImage>();

        public Queue<GameObject> newMoveImageQueue = new Queue<GameObject>();

        public GameObject moveImagePrefabs;


        public Transform showImageParent;

        private void Awake()
        {
            instance = this;
        }



        private void Start()
        {

        }


        public static bool IsInCircle(Vector2 CirclePoint, float r, Vector2 point)
        {
            //图片的点 （point.x - CirclePoint.x，point.y - CirclePoint.y） a=  r/2  b=r

            return Mathf.Sqrt((point.x - CirclePoint.x) * (point.x - CirclePoint.x) + (point.y - CirclePoint.y) * (point.y - CirclePoint.y)) <= r;
            //  bool isinCircle = (point.x - CirclePoint.x) * (point.x - CirclePoint.x) / (r * r) + (point.y - CirclePoint.y) * (point.y - CirclePoint.y) / (r * r / 2.25) <= 1;
            // return isinCircle;
        }

        public GameObject GetNewMoveImage()
        {
            GameObject newImage = null;
            if (newMoveImageQueue.Count > 0)
            {
                newImage = newMoveImageQueue.Dequeue();
            }
            else
            {
                newImage = Instantiate(moveImagePrefabs, showImageParent, false);
            }
            return newImage;
        }

        public void PushNewMoveImage(GameObject imageItem, Action callback)
        {
            if (newMoveImageQueue.Contains(imageItem))
            {
                return;
            }
            newMoveImageQueue.Enqueue(imageItem);
            if (callback != null)
            {
                callback();
            }
        }

        public void RegisterItem(GameObject obj, ImageType imageType)
        {
            if (imageType == ImageType.Move)
            {
                if (!moveImageList.Contains(obj.GetComponent<MyCircle_ImageItem>()))
                {
                    moveImageList.Add(obj.GetComponent<MyCircle_ImageItem>());
                }
            }
            else if (imageType == ImageType.Big)
            {
                if (!bigImageList.Contains(obj.GetComponent<BigImage>()))
                {
                    bigImageList.Add(obj.GetComponent<BigImage>());
                }
            }
        }

        public void ReleaseItem(GameObject obj, ImageType imageType)
        {
            if (imageType == ImageType.Move)
            {
                if (moveImageList.Contains(obj.GetComponent<MyCircle_ImageItem>()))
                {
                    moveImageList.Remove(obj.GetComponent<MyCircle_ImageItem>());
                }
            }
            else if (imageType == ImageType.Big)
            {
                if (bigImageList.Contains(obj.GetComponent<BigImage>()))
                {
                    bigImageList.Remove(obj.GetComponent<BigImage>());
                }
            }

        }
    }

}

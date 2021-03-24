using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyTouch : MonoBehaviour {

    /// <summary>  
    /// 定义的一个手指类  
    /// </summary>  
    class MyFinger
    {
        public int id = -1;
        public Touch touch;
        public Transform touchTrans;
         
        static private List<MyFinger> fingers = new List<MyFinger>();
        /// <summary>  
        /// 手指容器  
        /// </summary>  
        static public List<MyFinger> Fingers
        {
            get
            {
                if (fingers.Count == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        MyFinger mf = new MyFinger();
                        mf.id = -1;
                        mf.touchTrans = null;
                        fingers.Add(mf);
                    }
                }
                return fingers;
            }
        }
    }

    
    // 存储注册进来的Touch 物体
    public List<GameObject> myTouchs;
  
    void Update()
    {
        Touch[] touches = Input.touches;
        
        // 遍历所有的已经记录的手指  
        // --掦除已经不存在的手指  
        foreach (MyFinger mf in MyFinger.Fingers)
        {
            if (mf.id == -1)
            {
                continue;
            }
            bool stillExit = false;
            foreach (Touch t in touches)
            {
                if (mf.id == t.fingerId)
                {
                    stillExit = true;
                    break;
                }
            }
            // 掦除  
            if (stillExit == false)
            {
                mf.id = -1;
                mf.touchTrans = null;
            }
        }
        // 遍历当前的touches  
        // --并检查它们在是否已经记录在AllFinger中  
        // --是的话更新对应手指的状态，不是的加进去  
        foreach (Touch t in touches)
        {
            bool stillExit = false;
            // 存在--更新对应的手指  
            foreach (MyFinger mf in MyFinger.Fingers)
            {
                if (t.fingerId == mf.id)
                {
                    stillExit = true;
                    mf.touch = t;
                    break;
                }
            }
            // 不存在--添加新记录  
            if (!stillExit)
            {
                foreach (MyFinger mf in MyFinger.Fingers)
                {
                    if (mf.id == -1)
                    {
                        mf.id = t.fingerId;
                        mf.touch = t;
                        break;
                    }
                }
            }
        }

        // 记录完手指信息后，就是响应相应和状态记录了  
        for (int i = 0; i < MyFinger.Fingers.Count; i++)
        {
            MyFinger mf = MyFinger.Fingers[i];
            if (mf.id != -1)
            {
                if (mf.touchTrans==null)
                {
                    mf.touchTrans = CheckGuiRaycastObjects(mf.touch.position);
                }
               
                if (mf.touchTrans != null)
                {
                    if (mf.touch.phase == TouchPhase.Began)
                    {
                        TouchItem touchItem= mf.touchTrans.GetComponent<TouchItem>();
                        if (touchItem==null)
                        {
                            touchItem = mf.touchTrans.gameObject.AddComponent<TouchItem>();
                        }
                        mf.touchTrans.GetComponent<TouchItem>().OnTouchBegin(mf.touch);
                        
                    }
                    else if (mf.touch.phase == TouchPhase.Moved)
                    {
                        mf.touchTrans.GetComponent<TouchItem>().OnTouchDrag(mf.touch);

                    }
                    else if (mf.touch.phase == TouchPhase.Ended)
                    {
                        mf.touchTrans.GetComponent<TouchItem>().OnTouchEnd();
                        mf.id = -1;
                        mf.touchTrans = null;
                    }
                }
            }
        }
    }


    EventSystem eventSystem;
    public GraphicRaycaster RaycastInCanvas;//Canvas上有这个组件
    Transform CheckGuiRaycastObjects(Vector3 point)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = point;
        eventData.position = point;
        List<RaycastResult> list = new List<RaycastResult>();
        RaycastInCanvas.Raycast(eventData, list);
        Transform thistrnas = null;
        if (list.Count > 0)
        {
            bool isEnable = true;
            if (isEnable)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].gameObject.tag == "Player")
                    {
                            thistrnas = list[i].gameObject.transform;
                       
                        break;
                    }
                }
            }
        }
        return thistrnas;
    }

    public Vector3 GetWorldPos(Vector2 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 10));
    }


}
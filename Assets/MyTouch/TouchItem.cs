using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchItem : MonoBehaviour {
    
   public bool isBegin = false;
    Vector3 newpos;

    int fingerIndex = -1;

    MyTouch myTouch;

    Vector2 firstMousePos = Vector3.zero;

    //鼠标拖拽的位置

    Vector2 secondMousePos = Vector3.zero;
    Vector3 offsetPos = Vector3.zero;

  
    private void Awake()
    {
        myTouch = GameObject.FindObjectOfType<MyTouch>();
    }

    private void Start()
    {
        myTouch.myTouchs.Add(this.gameObject);
    }
    
  public void OnTouchBegin(Touch  mytouch) {
        try
        {
            isBegin = true;
       
            newpos = new Vector3(mytouch.position.x, mytouch.position.y, transform.localPosition.z);
       
            firstMousePos = newpos;
           }
          catch 
           {
             isBegin =false;
           }
    }
    
   public void OnTouchDrag(Touch mytouch) {
        if (isBegin)
        {
              try
              {
                newpos = new Vector3(mytouch.position.x, mytouch.position.y, transform.localPosition.z);

                secondMousePos = newpos;
                offsetPos = secondMousePos - firstMousePos;

                float x = transform.localPosition.x;

                float y = transform.localPosition.y;
                x = x + offsetPos.x; y = y + offsetPos.y;

                transform.localPosition = new Vector3(x, y, transform.localPosition.z);
                firstMousePos = secondMousePos;
            }
            catch 
            {
                isBegin = false;
            }
        }
    }

   public void OnTouchEnd() {
        isBegin = false;
        fingerIndex = -1;
    }

}

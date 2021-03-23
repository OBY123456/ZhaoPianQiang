using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyCirle;

public class BigImage : MonoBehaviour {

    public ImageType imageType = ImageType.Big;

    public List<MyCircle_ImageItem> moveImageList;

    public Vector3 currentPos;

    BigImage bigImage;

    Button quitButton;
    
    [Header("是否触摸")]
   public bool isTouch = true;
    [Header("未触摸时间")]
   public float touchTimer = 0;

    //判断是否开始检测周边移动图片
    [HideInInspector]
   public bool isCheck = false;

    float returnTimer = 200;
    
    private void Awake()
    {
        bigImage = this.GetComponent<BigImage>();
        quitButton = this.transform.GetChild(0).GetComponent<Button>();
    }

    private void OnEnable()
    {
        MessageEvent.instance.RegisterItem(this.gameObject, imageType);
        moveImageList = MessageEvent.instance.moveImageList;
        isTouch = true;
        isCheck = true;
    }
    
    private void Start()
    {
      quitButton.onClick.AddListener(CloseCircle);
    }

    private void FixedUpdate()
    {
        if (isTouch)
        {
            touchTimer += Time.fixedDeltaTime;
            if (touchTimer> returnTimer)
            {
              CloseCircle();
                touchTimer = 0;
                isTouch = false;
                isCheck = false;
            }
        }
        if (!isCheck)
        {
            return;
        }
        for (int i = 0; i < moveImageList.Count; i++)
        {
            if (MessageEvent.IsInCircle(transform.localPosition,300, moveImageList[i].currentPos))
            {
                if (!moveImageList[i].bigImageList.Contains(bigImage))
                {
                    moveImageList[i].bigImageList.Add(bigImage);
                    moveImageList[i].isInCircle = true;
                }
            }
            else {
                if (moveImageList[i].bigImageList.Contains(bigImage))
                {
                    moveImageList[i].bigImageList.Remove(bigImage);
                    if (moveImageList[i].bigImageList.Count<=0)
                    {
                        moveImageList[i].isInCircle = false;
                    }
                }
            }
        }

        currentPos = this.transform.localPosition;
    }


 
    private void OnDisable()
    {
        MessageEvent.instance.ReleaseItem(this.gameObject, imageType);
    }

    IEnumerator CloseCircleIE() {
        isCheck = false;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < moveImageList.Count; i++)
        {
            if (moveImageList[i].bigImageList.Contains(bigImage))
            {
                moveImageList[i].bigImageList.Remove(bigImage);
                if (moveImageList[i].bigImageList.Count <= 0)
                {
                    moveImageList[i].isInCircle = false;
                }
            }
        }

        MessageEvent.instance.ReleaseItem(this.gameObject, imageType);
       
        Destroy(gameObject);
    }

  public void CloseCircle() {
        StartCoroutine(CloseCircleIE());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyCirle;

public class MyCircle_ImageItem : MyUIBase {

	public bool isInCircle=false;

    public Vector3 inCirclePos;

    public ImageType imageType = ImageType.Move;

    public List<BigImage> bigImageList = new List<BigImage>();

    public Vector3 currentPos;
    public Vector3 targetPos;

    public GameObject bigImagePrefabs;
    Transform bigShowParent;

    public bool isEnable=false;

    Vector3 offsetPos;

    Vector3 targetScale = Vector3.one;

    int CircleR = 800;

    Button selfButton;

    RawImage rawImage;

    public override void OnEnter()
    {
        MessageEvent.instance.RegisterItem(this.gameObject, imageType);
        isEnable = true;
        transform.localPosition= currentPos;
        targetPos = currentPos;
        Texture2D newSprite = MyCircle_GameRoot.instance.GetImage();
        if (newSprite!=null)
        {
            transform.GetComponent<RawImage>().texture = newSprite;
        }
        bigImageList.Clear();
        transform.localScale=Vector3.one;
        targetScale = Vector3.one;
        isInCircle = false;
    }


    private void Start()
    {
        rawImage = this.GetComponent<RawImage>();
        selfButton = transform.GetComponent<Button>();

        bigShowParent = GameObject.Find("BigImagePanel").transform;
        selfButton.onClick.AddListener(() =>
        {
            GameObject newBigImage = Instantiate(bigImagePrefabs, bigShowParent, false);
            newBigImage.transform.localPosition = this.transform.localPosition;

            newBigImage.GetComponent<RawImage>().texture = this.GetComponent<RawImage>().texture;
        });
    }

   
    bool isInLine(Vector3 selfPos,Vector3 CircclePos) {

        Vector2 v2 = (selfPos - CircclePos).normalized;
        float angle2 = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        return (angle2 == 0 || angle2 == 180);
    }
    
    private void FixedUpdate()
    {
        if (isEnable)
        {

            currentPos += Vector3.left * 2f;
            targetPos += Vector3.left * 2f;

            if (currentPos.x < -3860)
            {
                OnExit();
            }

            if (isInCircle)
            {
                Vector3 nor = (currentPos - bigImageList[0].currentPos).normalized;
                currentPos = bigImageList[0].currentPos + nor * CircleR;
                if (bigImageList.Count > 0)
                {
                    //剔除空物体
                    for (int i = 0; i < bigImageList.Count; i++)
                    {
                        if (bigImageList[i]==null)
                        {
                            bigImageList.RemoveAt(i);
                        }
                    }

                    float scale = 0;
                    scale = 1 - bigImageList.Count * 0.5f;
                    scale = scale >= 0f ? scale : 0f;
                    targetScale = Vector3.one * scale;
                }
                if (isInLine(currentPos, bigImageList[bigImageList.Count - 1].currentPos))
                {
                    //当圆与图片处于同一直线上时将图片移动目标位置向上提升50,防止卡死在一个位置上
                    offsetPos = new Vector3(0, 50f, 0);
                }
            }
            else
            {
                offsetPos = Vector3.zero;
                targetScale = Vector3.one;
            }
            currentPos = Vector3.Lerp(currentPos, targetPos + offsetPos, 0.1f);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.1f);
            transform.localPosition = currentPos;
        }
    }




    public override void OnExit()
    {
        MessageEvent.instance.PushNewMoveImage(this.gameObject, () =>
        {
            gameObject.SetActive(false);
            isEnable = false;
        });
        MessageEvent.instance.ReleaseItem(this.gameObject, imageType);
    }
}

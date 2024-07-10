using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//开始界面
/*
 调用画面中的组件需要提前定义public变量 
 并在unity中将相应组件放置在对应变量上
 在这里 kinectimage对应的组件便是kinect图片
 Canv对应StartPanel的画布
 ...
     */
public class StartPanel : MonoBehaviour 
{
    public RawImage KinectImage;//实现kinectUI
    RectTransform Canv;
    public Image Hand;//手图标
    public Sprite[] HandState;//手状态数组，0：打开 1：握拳 在UI交互中 握拳相当于鼠标左键
    public Image NewGame;//新游戏按钮
    public Image Quit;//退出按钮
    public Image fruit_ng;//开始水果动画
    public Image fruit_quit;
    public int Yfall = 3000;
    PanelControl panelcontrol;

    public void Awake()
    {
        Canv = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        panelcontrol=Canv.GetComponent<PanelControl>();
    }

    void Update () 
	{
        bool isInit = KinectManager_fruit.Instance.IsInitialized();
        if (isInit)
        {
            if(KinectImage!=null && KinectImage.texture == null)
            {
                Texture2D usermap = KinectManager_fruit.Instance.GetUsersLblTex();//获得深度数据信息(检测到人）
                KinectImage.texture = usermap;//把kinect获得到的人的图像放到ui里面
            }

            //是否检测到用户
            if (KinectManager_fruit.Instance.IsUserDetected())
            {
                //获取用户id
                long UserId = KinectManager_fruit.Instance.GetPrimaryUserID();
                //获取右手关节
                int JType = (int)KinectInterop.JointType.HandRight;
                //追踪关节位置
                if (KinectManager_fruit.Instance.IsJointTracked(UserId, JType))
                {  
                    Vector3 rightjointpos= KinectManager_fruit.Instance.GetJointKinectPosition(UserId, JType);//世界坐标
                    Vector3 screenpos = Camera.main.WorldToScreenPoint(rightjointpos);//世界坐标-相机坐标
                    Vector2 screenpos2D = new Vector2(screenpos.x, screenpos.y);//3D-2D 手的相机坐标
                    Vector2 GUIPOS;//相机坐标-平面坐标
                    //手在不在画布里
                    if(RectTransformUtility.ScreenPointToLocalPointInRectangle(Canv, screenpos2D, Camera.main, out GUIPOS))
                    {
                        RectTransform HandRTF = Hand.transform as RectTransform;

                        HandRTF.anchoredPosition = GUIPOS;//手坐标赋给图标
                    }
                    Hand.sprite = HandState[0];//初始右手图标为打开

                    
                    bool isChoosed = false;//初始状态未选中
                    KinectInterop.HandState RightHandState=KinectManager_fruit.Instance.GetRightHandState(UserId);//右手状态
                    if(RightHandState== KinectInterop.HandState.Closed)
                    {
                        Hand.sprite = HandState[1];
                        isChoosed = true;
                    }
                    //如果选中
                    if (isChoosed)
                    {
                        //手坐标在new game 框里
                        if (NewGame.IsActive() && RectTransformUtility.RectangleContainsScreenPoint(NewGame.rectTransform, screenpos2D, Camera.main))
                        {
                            //隐藏环
                            NewGame.gameObject.SetActive(false);
                            Quit.gameObject.SetActive(false);
                            //开始水果向上在向下 炸弹向下
                            fruit_ng.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Yfall));
                            fruit_ng.GetComponent<Rigidbody2D>().gravityScale = 10;
                            fruit_quit.GetComponent<Rigidbody2D>().gravityScale = 10;
                            

                        }
                        else if(Quit.IsActive() && RectTransformUtility.RectangleContainsScreenPoint(Quit.rectTransform, screenpos2D, Camera.main))
                        {
                            Application.Quit();//手坐标在quit里 退出程序
                        }
                    }
                }

            }
            

        }
        //进入游戏界面
        if(fruit_ng.rectTransform.anchoredPosition.y < -300)
        {
            panelcontrol.CreateGamePanel();
            Destroy(gameObject);
        }

	}
}

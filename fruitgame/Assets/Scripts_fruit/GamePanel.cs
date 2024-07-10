using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
//此脚本为游戏过程的实现

public class GamePanel : MonoBehaviour
{
    public float timer = 0f;

    public Fruit FruitPrefab;//水果类
    Fruit CurFruit;//当前水果
    int fruitXpostion = 270;//水果x范围
    int fruitYpostion = 270;//水果y范围
    
    int forceX = 1000;
    int forceY = 7000;
    int cutForceY = 2500;
    public RawImage KinectImage;
    //拖尾
    public Transform LeftTrail;
    public Transform RightTrail;
    public RectTransform TextPrefab;
    public Life lifecon;//生命值
    int LifeNum = 3;
    public Text scoreText;//得分
    int ScoreNum = 0;
    public Text maxText;//最高分
    int maxNum=0;
    public GameObject gameover;
    PanelControl panelcontrol;
    public AudioSource audioSource;
    public AudioClip boom;
    public AudioClip cutfruit;
    public AudioClip create;
    public void Awake()
    {

        panelcontrol = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PanelControl>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("maxScore"))
        {
            maxNum = PlayerPrefs.GetInt("maxScore");

        }
        maxText.text = "最高分：" + maxNum.ToString();
        gameover.SetActive(false);
        CreateFruit();
    }
    void CreateFruit()
    {
        if (LifeNum > 0)
        {
            CurFruit = Instantiate(FruitPrefab);//克隆水果
            CurFruit.transform.SetParent(transform);//将水果的父类设置为当前界面对象

            //水果出现 
            CurFruit.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            int fruitX = Random.Range(-fruitXpostion, fruitXpostion);
            CurFruit.GetComponent<RectTransform>().anchoredPosition = new Vector2(fruitX, -fruitYpostion);
            CurFruit.transform.localScale = Vector3.one;

            int[] types = { FruitBoomR.BOOM, FruitBoomR.APPLE, FruitBoomR.BANANA, FruitBoomR.CAOMEI, FruitBoomR.PEACH, FruitBoomR.WATERMELON };
            int FruitType = types[Random.Range(0, types.Length)];
            CurFruit.SetType(FruitType);
            if (FruitType == FruitBoomR.BOOM)
            {
                CurFruit.transform.Find("BoomEffect").gameObject.SetActive(true);
            }
            else
            {
                CurFruit.transform.Find("BoomEffect").gameObject.SetActive(false);
            }


            //水果上抛
            Rigidbody2D rig = CurFruit.GetComponent<Rigidbody2D>();
            if (fruitX > 0)
            {
                rig.AddForce(new Vector2(-forceX, forceY));
            }
            else
            {
                rig.AddForce(new Vector2(forceX, forceY));
            }
            audioSource.PlayOneShot(create);
        }
    }

    void Update()
    {
        
        if (KinectImage != null && KinectImage.texture == null)
        {
            Texture2D usermap = KinectManager_fruit.Instance.GetUsersLblTex();//获得深度数据信息(检测到人）
            KinectImage.texture = usermap;//把kinect获得到的人的图像放到ui里面
        }
        if (CurFruit != null)
        {
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
                    Vector3 rightjointpos = KinectManager_fruit.Instance.GetJointKinectPosition(UserId, JType);//世界坐标
                    RightTrail.position = rightjointpos;
                    Vector3 screenpos = Camera.main.WorldToScreenPoint(rightjointpos);//世界坐标-相机坐标
                    Vector2 screenpos2D = new Vector2(screenpos.x, screenpos.y);//3D-2D 手的相机坐标
                    KinectInterop.HandState RhandState = KinectManager_fruit.Instance.GetRightHandState(UserId);
                    if (/*RhandState == KinectInterop.HandState.Open && */ RectTransformUtility.RectangleContainsScreenPoint(CurFruit.transform as RectTransform, screenpos2D, Camera.main))
                    {
                        cut();
                    }
                }
                int LeftJType = (int)KinectInterop.JointType.HandLeft;
                //追踪左手关节位置
                if (KinectManager_fruit.Instance.IsJointTracked(UserId, LeftJType))
                {
                    Vector3 leftjointpos = KinectManager_fruit.Instance.GetJointKinectPosition(UserId, LeftJType);//世界坐标
                    LeftTrail.position = leftjointpos;
                    Vector3 leftscreenpos = Camera.main.WorldToScreenPoint(leftjointpos);//世界坐标-相机坐标
                    Vector2 leftscreenpos2D = new Vector2(leftscreenpos.x, leftscreenpos.y);//3D-2D 手的相机坐标
                    KinectInterop.HandState LhandState = KinectManager_fruit.Instance.GetLeftHandState(UserId);
                    if (/*LhandState == KinectInterop.HandState.Open && */ RectTransformUtility.RectangleContainsScreenPoint(CurFruit.transform as RectTransform, leftscreenpos2D, Camera.main))
                    {
                        cut();
                    }
                }
            }
            //没切到水果
            if (CurFruit.GetComponent<RectTransform>().anchoredPosition.y < -300)
            {
                Destroy(CurFruit.gameObject);
                
                if (CurFruit.type != FruitBoomR.BOOM)
                {
                    LifeNum--;

                }
                
                CreateFruit();
            }
            scoreText.text = ScoreNum.ToString();
            if (PlayerPrefs.HasKey("maxScore"))
            {
                maxNum = PlayerPrefs.GetInt("maxScore");

            }
            maxText.text = "最高分：" + maxNum.ToString();
            lifecon.SetLife(LifeNum);
            if (LifeNum <= 0)
            {
                
                gameover.SetActive(true);
                Invoke("gameoverhandler", 2);


            }




        }
        
    }
    //结束界面处理
    void gameoverhandler()
    {
        panelcontrol.CreateStartPanel();
        Destroy(gameObject);
        /*
        //是否检测到用户
        if (KinectManager.Instance.IsUserDetected())
        {
            //获取用户id
            long UserId = KinectManager.Instance.GetPrimaryUserID();
            //获取右手关节
            int JType = (int)KinectInterop.JointType.HandRight;
            //追踪关节位置
            if (KinectManager.Instance.IsJointTracked(UserId, JType))
            {
                Vector3 rightjointpos = KinectManager.Instance.GetJointPosition(UserId, JType);//世界坐标
                Vector3 screenpos = Camera.main.WorldToScreenPoint(rightjointpos);//世界坐标-相机坐标
                Vector2 screenpos2D = new Vector2(screenpos.x, screenpos.y-200);//3D-2D 手的相机坐标
                KinectInterop.HandState RhandState = KinectManager.Instance.GetRightHandState(UserId);

                
                if (RhandState == KinectInterop.HandState.Open  &&  RectTransformUtility.RectangleContainsScreenPoint(gameover.transform as RectTransform, screenpos2D, Camera.main))

                {
                    panelcontrol.CreateStartPanel();
                    Destroy(gameObject);
                }
                
        }
    }
    */

    }

    //切到水果处理
    void cut()
    {
        if (CurFruit.type == FruitBoomR.BOOM)
        {
            Destroy(CurFruit.gameObject);
            CreateFruit();
            //碰到炸弹做相应处理
            LifeNum = 0;
            audioSource.PlayOneShot(boom);
        }
        else
        {
            ScoreNum++;
            if (!PlayerPrefs.HasKey("maxScore") || ScoreNum > PlayerPrefs.GetInt("maxScore"))
            {
                PlayerPrefs.SetInt("maxScore",ScoreNum);
            }

            ProcessText();
            cuttedFruit();
            Destroy(CurFruit.gameObject);
            audioSource.PlayOneShot(cutfruit);
            CreateFruit();

        }
    }
//生成两半的水果
    void cuttedFruit()
    {
        Fruit Leftonef = Instantiate(FruitPrefab);
        Leftonef.SetType(CurFruit.type + 1);
        Leftonef.transform.Find("FruitEffect").gameObject.SetActive(true);
        initCuttedFruit(Leftonef, true);
        Fruit Rightonef = Instantiate(FruitPrefab);
        Rightonef.SetType(CurFruit.type + 2);
        Rightonef.transform.Find("FruitEffect").gameObject.SetActive(true);
        initCuttedFruit(Rightonef, false);
    }

    /*
     fuction:初始化切成两半的水果 初始化位置、施加两边力
     fruit:切了之后的水果 左边一半 右边一半
     isleft:是否是左边一半
         */
    void initCuttedFruit(Fruit fruit,bool isleft)
    {
        fruit.transform.SetParent(transform);
        RectTransform rt = fruit.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        rt.anchoredPosition = CurFruit.GetComponent<RectTransform>().anchoredPosition;
        rt.localScale = Vector3.one;
        if (isleft)
        {
            fruit.GetComponent<Rigidbody2D>().AddForce(new Vector2(-forceX, cutForceY));//两半的水果施加加速度让其往两边走

        }
        else
        {
            fruit.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, cutForceY));//两半的水果施加加速度让其往两边走
        }
       
    }
    void ProcessText()
    {
        RectTransform Text = Instantiate(TextPrefab);
        Text.SetParent(transform);
        Text.anchoredPosition3D = Vector3.zero;
        Text.anchoredPosition=CurFruit.GetComponent<RectTransform>().anchoredPosition+new Vector2(0,50);
        Text.localScale = Vector3.one;


    }
}

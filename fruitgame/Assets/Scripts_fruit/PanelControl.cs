using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制StartPanel和GamePanel的显示
public class PanelControl : MonoBehaviour
{
    public RectTransform startPanelPrefab; //开始面板预制体
    public RectTransform gamePanelPrefab; //游戏面板预制体

    private void Start()
    {
        CreateStartPanel();
    }

    //创建开始面板
    public void CreateStartPanel()
    {
        RectTransform startPanel = Instantiate(startPanelPrefab);
        startPanel.SetParent(transform);
        startPanel.offsetMin = Vector2.zero;
        startPanel.offsetMax = Vector2.zero;
        startPanel.anchoredPosition3D = Vector3.zero;
        startPanel.localScale = Vector3.one;
    }
    //创建游戏面板
    public void CreateGamePanel()
    {
        RectTransform gamePanel = Instantiate(gamePanelPrefab);
        gamePanel.SetParent(transform);
        gamePanel.offsetMin = Vector2.zero;
        gamePanel.offsetMax = Vector2.zero;
        gamePanel.anchoredPosition3D = Vector3.zero;
        gamePanel.localScale = Vector3.one;
    }
}


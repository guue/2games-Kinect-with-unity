using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fruit : MonoBehaviour 
{
    public Sprite[] fruit;//界面上的水果数组
    Image fruitImage;//水果
    [HideInInspector]
    public int type;//水果类型
	void Awake () 
	{
        fruitImage = GetComponent<Image>();//此脚本挂在fruit上直接获取图片即可
	}
	
    public void SetType(int type)
    {
        this.type = type;
        fruitImage.sprite = fruit[type];
        
    }
    public void OnTriggerEnter2D(Collider2D collision)//检测是否触发碰撞器 
    {
        if (collision.tag == "Bound")//碰到边界 销毁 
        {
            Destroy(gameObject);
        }
    }
}

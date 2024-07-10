using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//控制生命值
public class Life : MonoBehaviour 
{
    public Toggle[] life;

    public void SetLife(int LifeNum)//0
    {
        int MaxLife = life.Length;//3
        int i = 0;
        while(i < (MaxLife-LifeNum))//0-3 0 1 2
        {
            life[i].isOn = false;
            i++;



        }
    }
}

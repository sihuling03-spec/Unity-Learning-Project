using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;
//该脚本的使用基于上文插件处理过的阿拉伯语。挂在在对应text上即可。
public class TextFixer : MonoBehaviour
{
    //The UI text 
    Text myText;
    //used in cutting the lines 
    int startIndex;
    int endIndex;
    int length;
    //*************************
    static string[] FixedText = new string[30];
    string[] Holder = new string[5];
    string TextHolder;
    void Start()
    {
        //invoke避免与其他文字处理先后位置冲突。
        Invoke("WaitFixedText", 0.1f);
    }
    public void WaitFixedText()
    {
        myText = gameObject.GetComponent<Text>();
        string tempText = "";
        if (myText.text != null)
        {
            tempText = myText.text; //查找Key
        }
        Holder = tempText.Split('\n');
        FixText();
    }

    public void FixText()
    {
        //只有一个字段的话最后一行的话不用换行。
        if (Holder.Length == 1)
        {
            int templinesCount = 0;
            myText.text = Holder[0];
            Canvas.ForceUpdateCanvases();
            for (int k = 0; k < FixedText.Length; k++)
            {
                FixedText[k] = "";
            }
            for (int k = 0; k < myText.cachedTextGenerator.lines.Count; k++)
            {
                startIndex = myText.cachedTextGenerator.lines[k].startCharIdx;
                endIndex = (k == myText.cachedTextGenerator.lines.Count - 1) ? myText.text.Length
                   : myText.cachedTextGenerator.lines[k + 1].startCharIdx;
                length = endIndex - startIndex;
                FixedText[k] = myText.text.Substring(startIndex, length);
                //在每行补上被截断的color标签
                AddColorTag(k);
                templinesCount = k;
            }
            myText.text = "";
            for (int k = FixedText.Length - 1; k >= 0; k--)
            {
                if (FixedText[k] != "" && FixedText[k] != "\n" && FixedText[k] != null)
                {
                    if (templinesCount == 0)
                    {
                        TextHolder += FixedText[k];
                    }
                    else
                    {
                        if (templinesCount != 0)
                        {
                            TextHolder += FixedText[k] + "\n";
                            templinesCount--;
                        }
                        else
                        {
                            TextHolder += FixedText[k];
                        }
                    }
                }
            }
        }
        else
        {
            //包含换行符的多行处理
            for (int i = 0; i < Holder.Length; i++)
            {
                int templinesCount = 0;
                myText.text = Holder[i];
                Canvas.ForceUpdateCanvases();
                for (int k = 0; k < FixedText.Length; k++)
                {
                    FixedText[k] = "";
                }
                for (int k = 0; k < myText.cachedTextGenerator.lines.Count; k++)
                {
                    startIndex = myText.cachedTextGenerator.lines[k].startCharIdx;
                    endIndex = (k == myText.cachedTextGenerator.lines.Count - 1) ? myText.text.Length
                       : myText.cachedTextGenerator.lines[k + 1].startCharIdx;
                    length = endIndex - startIndex;
                    FixedText[k] = myText.text.Substring(startIndex, length);
                    //在每行补上被截断的color标签
                    AddColorTag(k);
                    templinesCount = k;
                }
                myText.text = "";
                //如果是最后一个字段的最后一行的话不用换行。
                if (i == Holder.Length - 1)
                {
                    for (int k = FixedText.Length - 1; k >= 0; k--)
                    {
                        if (FixedText[k] != "" && FixedText[k] != "\n" && FixedText[k] != null)
                        {
                            if (templinesCount == 0)
                            {
                                TextHolder += FixedText[k];
                            }
                            else
                            {
                                if (templinesCount != 0)
                                {
                                    TextHolder += FixedText[k] + "\n";
                                    templinesCount--;
                                }
                                else
                                {
                                    TextHolder += FixedText[k];
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int k = FixedText.Length - 1; k >= 0; k--)
                    {

                        if (FixedText[k] != "" && FixedText[k] != "\n" && FixedText[k] != null)
                        {
                            TextHolder += FixedText[k] + "\n";
                        }
                    }
                }

            }

        }
        myText.text = TextHolder;
        //yield return new WaitForEndOfFrame();
    }
    /// <summary>
    /// 临时颜色
    /// </summary>
    public static string tempcolor = "";
    /// <summary>
    /// 是否能够组成整数组颜色
    /// </summary>
    public static int IsOneGroup = 0;
    static void AddColorTag(int k)
    {
        //通过本行中color左标签和右标签的数量判断需要在本行的那一端添加color标签
        int leftCount = Regex.Matches(FixedText[k], "<color").Count;
        int rightCount = Regex.Matches(FixedText[k], "</color>").Count;
        //判断最右侧是否缺少</color>标签
        int index1 = -1;
        int index2 = -2;
        index1 = FixedText[k].LastIndexOf("<color");
        Debug.Log("index1:" + index1);
        if (index1 != -1)
        {
            //判断index1后面还有没有</color>
            index2 = FixedText[k].IndexOf("</color>", index1);
            Debug.Log("index2:" + index2);
        }
        //左标签多于右标签 则在本行末尾添加</color>
        if (leftCount > rightCount)
        {
            IsOneGroup++;
            FixedText[k] += "</color>";
            //保存颜色减少遍历次数
            for (int i = 10; i < 20; i++)
            {
                tempcolor = FixedText[k].Substring(index1, i);
                if (tempcolor.EndsWith(">"))
                {
                    break;
                }
            }
        }
        //右标签多于左标签 则获取到上一行的最后一个左标签 添加到本行开头
        else if (rightCount > leftCount)
        {
            IsOneGroup++;
            FixedText[k] = tempcolor + FixedText[k];
        }
        //当存在</color><color=...>的情况 两边都添加
        else if (leftCount == rightCount)
        {
            if (leftCount > 0 & index2 == -1)
            {
                FixedText[k] = tempcolor + FixedText[k];

                for (int i = 10; i < 20; i++)
                {
                    tempcolor = FixedText[k].Substring(FixedText[k].LastIndexOf("<color"), i);
                    if (tempcolor.EndsWith(">"))
                    {
                        break;
                    }
                }
                FixedText[k] += "</color>";
            }
            else
            {
                if (IsOneGroup%2!=0)
                {
                    FixedText[k] = tempcolor + FixedText[k];
                    FixedText[k] += "</color>";
                }
            }
        }
    }
}

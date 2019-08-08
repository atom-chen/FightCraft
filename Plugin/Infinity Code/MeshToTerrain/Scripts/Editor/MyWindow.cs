using UnityEngine;
using System.Collections;
using UnityEditor;
public class MyWindow : EditorWindow
{
    string move;
    [MenuItem("Window/MyWindow")]//在unity菜单Window下有MyWindow选项
    static void Init()
    {
        MyWindow myWindow = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow), false, "MyWindow", false);
        myWindow.Show(true);
    }
    //void OnGUI()
    //{
    //    move = EditorWindow.mouseOverWindow ? EditorWindow.mouseOverWindow.ToString() : "Nothing";
    //    EditorGUILayout.LabelField(move);
    //}
    //void OnInspectorUpdate()
    //{
    //    if (EditorWindow.mouseOverWindow)
    //        EditorWindow.mouseOverWindow.Focus();//就是当鼠标移到那个窗口，这个窗口就自动聚焦
    //    this.Repaint();//重画MyWindow窗口，更新Label
    //}
}
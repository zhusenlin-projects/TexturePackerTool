using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    /// <summary>
    /// ���ļ�
    /// </summary>
    public string OpenFile(string fileExtension)
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        pth.filter = "All files (*.*)|*.*";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath; //Ĭ��·��
        pth.title = "����Ŀ";
        pth.defExt = fileExtension;
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file; //ѡ����ļ�·��;  
            //Debug.Log(filepath);
            return filepath;
        }
        return null;
    }


    /// <summary>
    /// �����ļ�
    /// </summary>
    public string SaveFile()
    {
        SaveFileDlg pth = new SaveFileDlg();
        pth.structSize = Marshal.SizeOf(pth);
        pth.filter = "All files (*.*)|*.*";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath; //Ĭ��·��
        pth.title = "������Ŀ";
        pth.defExt = "";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (SaveFileDialog.GetSaveFileName(pth))
        {
            string filepath = pth.file; //ѡ����ļ�·��;  
            //Debug.Log(filepath);
            return filepath;
        }
        return null;
    }
}

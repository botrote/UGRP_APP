using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class TextSerializer : MonoBehaviour
{
    public bool isLoading { get; private set; }
    public byte[] loadedText {get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        loadedText = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadTextToByte(string fileName)
    {
        isLoading = true;
        if (!fileName.ToLower().EndsWith(".txt")) 
        {
			fileName += ".txt";
		}
        string path = Path.Combine(Application.persistentDataPath + "/data/", fileName);
        int length = System.IO.File.ReadAllBytes(path).Length;

        byte[] b_fileNameLength = BitConverter.GetBytes(fileName.Length);
        byte[] b_fileName = Encoding.UTF8.GetBytes(fileName);
        byte[] b_textdata = System.IO.File.ReadAllBytes(path);
        
        loadedText = new byte[length*4 + 4 + b_fileName.Length];
        Buffer.BlockCopy(b_fileNameLength, 0, loadedText, 0, 4);
        Buffer.BlockCopy(b_fileName, 0, loadedText, 4, b_fileName.Length);
        Buffer.BlockCopy(b_textdata, 0, loadedText, 4+b_fileName.Length, loadedText.Length-4-b_fileName.Length);

        isLoading = false;

        /*
         * | filenameLength=4 byte | fileName=n byte | data=m byte |
         * 
         *
         */
    }

    public string StoreByteText(byte[] data)
    {
      //  Debug.Log(data.Length);
        byte[] b_fileNameLength = new byte[4];
    //    byte[] fileName = new byte[];

        Buffer.BlockCopy(data, 0, b_fileNameLength, 0, 4);

        int fileNameLength = BitConverter.ToInt32(b_fileNameLength, 0);

        byte[] b_fileName = new byte[fileNameLength];
        byte[] b_textdata = new byte[data.Length-4-fileNameLength];

        Buffer.BlockCopy(data, 12, b_fileName, 0, fileNameLength);
        string fileName = Encoding.UTF8.GetString(b_fileName);
        Buffer.BlockCopy(data, 4+fileNameLength, b_textdata, 0, data.Length-4-fileNameLength);
        string textData =  Encoding.UTF8.GetString(b_textdata);

        fileName = "test" + fileName;
        SavTxt.Save(fileName, textData);
        return textData;
    }

}

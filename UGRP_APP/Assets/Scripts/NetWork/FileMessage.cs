using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace FSP
{
    public enum FileType{Text, Wav};
    public class CustomMsgType
    {
        public static short FileMsgType = MsgType.Highest + 1;
    }

    public class FileMessage : MessageBase
    {
        public string fileName; 
        public FileType fileType;
        public byte[] contents;
        public int maxFrac;
        public int fracNum;
    }
}

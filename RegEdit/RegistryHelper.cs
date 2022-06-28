using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using RegEdit;

namespace TagReceiver
{
    class RegistryHelper
    {
        /// <summary>
        /// 读取指定名称的注册表的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetRegistryData(RegistryKey root, string subkey, string name)
        {
            string registData = "";
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            if (myKey != null)
            {
                registData = myKey.GetValue(name).ToString();
            }
           return registData;
        }

        public void DeleteValue(RegistryKey root, string subkey, string name)
        {
            RegistryKey mykey = root.OpenSubKey(subkey,true);

            try
            { mykey.DeleteValue(name); }
            catch (Exception)
            { return; }
      
        }

        /// <summary>
        /// 创建指定的注册表项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tovalue"></param> 
        //public void SetRegistryData(RegistryKey root, string subkey, string name, string value)
        public void SetRegistryData(RegistryKey root, string subkey)
        {
            RegistryKey aimdir = root.CreateSubKey(subkey);
            
            //aimdir.SetValue(name, value);
        }
        //仅创建键值



        //创建子项并设置键值
        public void SetRegistryValue(RegistryKey root, string subkey, string name, string value)
        {
            RegistryKey aimdir = root.CreateSubKey(subkey);
            aimdir.SetValue(name, value);
        }

        /// 重命名子项
        public void RenameRegist(RegistryKey root, string subkey )
        {
            RegistryUtilities renameKey = new RegistryUtilities();
            try
            {
                renameKey.RenameSubKey(root,subkey,subkey+1);
            }
            catch(Exception)
            { return; }
        }
        ///重命名键值名称
        public void RenameKeyValueName(RegistryKey root, string subkey,string name,string newname)
        {
            string[] subkeyValueNames;
            RegistryKey mykey = root.OpenSubKey(subkey, true);
            subkeyValueNames = mykey.GetValueNames();
            try
            {
                foreach (string i in subkeyValueNames)
                {
                    if (i == name)
                    { //RegistryKey changeValueName = mykey.OpenSubKey(i,true);
                      //changeValueName.SetValue(newname,"renamed");
                        mykey.SetValue(i,"renamed");
                    }
                }
            }
            catch (Exception)
            { return; }
        }


        /// <summary>
        /// 删除注册表中指定项下的所有项
        /// </summary>
        /// <param name="name"></param>
        public void DeleteRegist(RegistryKey root, string subkey)
        {
            try
            {
                root.DeleteSubKeyTree(subkey);
            }
            catch (Exception)
            {
             return;
            }
        }

        ///在指定项下创建键值(指定循环阈值)
        public void CreateKeyValue(RegistryKey root, string subkey, Int32 num, Int32 StressNum)
        {
            
            RegistryKey mkey = root.OpenSubKey(subkey, true);
            string name = "test";
            string value = "testing done!";
                     
            for (int i = 1; i <= num; i++)
            {
                try
                {
                    Thread.Sleep(StressNum);
                    SetRegistryValue(root,subkey,name+i,value);
                }
                catch (Exception)
                { continue; }
            }

        }

        ///删除注册表指定项下所有键值(指定循环阈值)
        
        public void DeleteKeyValue(RegistryKey root ,string subkey ,Int32 num)
        {
            string[] subkeyValueNames;
            RegistryKey mkey = root.OpenSubKey(subkey, true);
           
            subkeyValueNames = mkey.GetValueNames();
            for (int i = 0; i < subkeyValueNames.Length; i++)
            {
                try
                {
                    Thread.Sleep(num);
                    mkey.DeleteValue(subkeyValueNames[i]);
                }
                catch (Exception)
                { continue; }
            }
        }
        
        
        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsRegistryExist(RegistryKey root, string subkey, string name)
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            subkeyNames = myKey.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    _exit = true;
                    return _exit;
                }
            }

            return _exit;
        }
        public static void WriteErrLog(string errTitle, Exception ex)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\错误日志.txt";

            if (!File.Exists(path))
            {
                try { File.Create(path).Close(); }
                catch (Exception)
                { return; }
            }

            StringBuilder strBuilderErrorMessage = new StringBuilder();

            strBuilderErrorMessage.Append("________________________________________________________________________________________________________________\r\n");
            strBuilderErrorMessage.Append("日期:" + System.DateTime.Now.ToString() + "\r\n");
            strBuilderErrorMessage.Append("错误标题:" + errTitle + "\r\n");
            strBuilderErrorMessage.Append("错误信息:" + ex.Message + "\r\n");
            strBuilderErrorMessage.Append("错误内容:" + ex.StackTrace + "\r\n");
            strBuilderErrorMessage.Append("________________________________________________________________________________________________________________\r\n");
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.Write(strBuilderErrorMessage);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            { return; }

        }
    }
}
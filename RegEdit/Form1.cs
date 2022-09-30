using Microsoft.Win32;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagReceiver;

namespace RegEdit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadRegister();


            ComboxItem[] x = {
             new ComboxItem("HKEY_CLASSES_ROOT", "HKEY_CLASSES_ROOT"),
             new ComboxItem("HKEY_CURRENT_USER", "HKEY_CURRENT_USER"),
             new ComboxItem("HKEY_LOCAL_MACHINE", "HKEY_LOCAL_MACHINE"),
             new ComboxItem("HKEY_USERS","HKEY_USERS"),
             new ComboxItem("HKEY_CURRENT_CONFIG","HKEY_CURRENT_CONFIG")
            };
            comboBox1.Items.AddRange(x);
            comboBox1.Text = "HKEY_LOCAL_MACHINE";
             
        }
        //取值时需要转换一下 如下:
        //string str = ((ComboxItem)combobox2.Items[0]).Value;


        /// <summary>
        /// 判断是值还是项
        /// </summary>
        /// <param name="KeyOrValue"></param>
        /// <returns></returns>

        public void loadRegister()
        {
            
            Font MyFont = new Font("宋体",9,FontStyle.Regular);
            
            textBox1.Text = "*例:software\\test1\\test2\\test3,如选择值则“test3”为键值名称！";
            textBox1.ForeColor = Color.Gray;
            textBox1.Font = MyFont;

            textBox2.Text = "请先选择Root项，再输入子项！创建键值个数不为空！";
            textBox2.ForeColor = Color.Gray;
            textBox2.Font = MyFont;

            textBox3.Text = "请先选择Root项，再输入子项！";
            textBox3.ForeColor = Color.Gray;
            textBox3.Font = MyFont;

            textBox5.Text = "阈值(ms)";
            textBox5.ForeColor = Color.Gray;
            textBox5.Font = MyFont;

            textBox4.Text = "单位(个)";
            textBox4.ForeColor = Color.Gray;
            textBox4.Font = MyFont;




        }
        public bool IsKeyOrValue()
        {
            if (radioButton1.Checked)
            { return true; }
            else
            { return false; }
            
        }

        public RegistryKey GetRegDomain(string regDomain)
        {
            ///创建基于注册表基项的节点
            RegistryKey key;

            #region 判断注册表基项域
            switch (regDomain)
            {
                case "HKEY_CLASSES_ROOT":
                    key = Registry.ClassesRoot; break;
                case "HKEY_CURRENT_USER":
                    key = Registry.CurrentUser; break;
                case "HKEY_LOCAL_MACHINE":
                    key = Registry.LocalMachine; break;
                case "HKEY_USERS":
                    key = Registry.Users; break;
                case "HKEY_CURRENT_CONFIG":
                    key = Registry.CurrentConfig; break;
            /*
                case RegDomain.DynDa:
                    key = Registry.PerformanceData; break;
                case RegDomain.PerformanceData:
                    key = Registry.PerformanceData; break;
            */
                default:
                    key = Registry.LocalMachine; break;
            }
            #endregion

            return key;
        }
        public void DeleteRegist(string name)
        {
            string[] keys;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE",true);
            RegistryKey Zane = software.OpenSubKey("Zane", true);
            keys = Zane.GetSubKeyNames();
            foreach (string key in keys)
            {
                if (key=="zane")
                {
                    Zane.DeleteValue("zane");
                    this.Close();
                }
            }

        }

        private void CreateReg_Click(object sender, EventArgs e)
        {
            RegistryKey key_Root = GetRegDomain(comboBox1.Text);
            string initial_subkey = textBox1.Text;
            string subkey = initial_subkey.Replace(@"\",@"\\");
            
            RegistryHelper rh = new RegistryHelper();
            if (IsKeyOrValue())
            {
                try
                {
                    rh.SetRegistryData(key_Root, subkey);
                }
                catch (Exception)
                { return; }
                finally
                {
                    IDisposable disposable = rh as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
                
            }
            //如果为值，则最后一个\\后面的内容为键值，值为“testing done!”
            else
            {
                int a = subkey.LastIndexOf("\\");
                string real_subkey = subkey.Substring(0, a-1);
                string realsubkey_value = subkey.Substring(a+1);
                try
                {
                    rh.SetRegistryValue(key_Root, real_subkey, realsubkey_value, "testing done!");
                    MessageBox.Show(real_subkey);
                }
                catch (Exception)
                { return; }
                finally
                {
                    IDisposable disposable = rh as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }

            }
            
        }

        private void DelReg_Click(object sender, EventArgs e)
        {
            RegistryHelper rh = new RegistryHelper();
            RegistryKey key_Root = GetRegDomain(comboBox1.Text);
            string initial_subkey = textBox1.Text;
            string subkey = initial_subkey.Replace(@"\", @"\\");

            if (IsKeyOrValue())
            {
                ///如果为key，则删除键。
                try
                { rh.DeleteRegist(key_Root, subkey); }
                catch (Exception)
                { return; }
                finally
                {
                    IDisposable disposable = rh as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            }
            else
            {
                int a = subkey.LastIndexOf("\\");
                string real_subkey = subkey.Substring(0, a - 1);
                string realsubkey_value = subkey.Substring(a + 1);
                ///如果为值，删除值
                rh.DeleteValue(key_Root,real_subkey,realsubkey_value);
            }

        }

        private void RenameReg_Click(object sender, EventArgs e)
        {
            RegistryKey key_Root = GetRegDomain(comboBox1.Text);

            string initial_subkey = textBox1.Text;
            string subkey = initial_subkey.Replace(@"\", @"\\");
            
            RegistryHelper rh = new RegistryHelper();
            if (IsKeyOrValue())
            {
                try
                { rh.RenameRegist(key_Root, subkey); }
                catch (Exception)
                { return; }
                finally
                {
                    IDisposable disposable = rh as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            }
            else
            {
                int a = subkey.LastIndexOf("\\");                
                string first_subkey= subkey.Substring(0, a - 1);            
                string subkey_value = subkey.Substring(a+1);
                try
                {
                    rh.RenameKeyValueName(key_Root, first_subkey, subkey_value, subkey_value + 1);
                    MessageBox.Show(first_subkey + "的键值" + subkey_value + "已被重命名成功！");
                }
                catch (Exception)
                { return; }
                finally
                {
                    IDisposable disposable = rh as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            }
              
        }
        //创建多个键值准备压测条件
        private void button2_Click(object sender, EventArgs e)
        {
            
            string initial_subkey = textBox1.Text;
            string subkey = initial_subkey.Replace(@"\", @"\\");

            string RegNum = textBox4.Text;
            bool isPath = subkey.Contains("\\");
            RegistryHelper reg = new RegistryHelper();
            RegistryKey key_Root = GetRegDomain(comboBox1.Text);

            if (string.IsNullOrEmpty(subkey) || string.IsNullOrEmpty(RegNum))
            { MessageBox.Show("路径或个数不能为空！");
                return;
            }
            else
            { if (!isPath)
                { MessageBox.Show("请确认路径格式正确！"); return; }
                else
                {
                    try
                    {
                        Task t1 = Task.Run(() =>
                        {
                            button2.Enabled = false;
                            for (int i = 1; i <= Convert.ToInt32(RegNum); i++)
                            {
                                RegistryKey aimdir = key_Root.CreateSubKey(subkey);
                                try
                                {
                                    aimdir.SetValue("test" + i, "Creation KeyValue done!");
                                }
                                catch (Exception) { return; }
                                finally
                                {
                                    IDisposable disposable = aimdir as IDisposable;
                                    if (disposable != null)
                                        disposable.Dispose();
                                }
                            }
                        });
                        t1.ContinueWith(m => { MessageBox.Show("创建完成!"); button2.Enabled = true; });
                    }
                    catch
                    { return; }
                    
                }
            }
        }

        public static bool validateNum(string strNum)
        {
            return Regex.IsMatch(strNum, "^[0-9]*$");
        }
        //开始压力测试
        private async void button1_Click(object sender, EventArgs e)
        {
            //新建路径
            string initial_subkey = textBox2.Text;
            string subkey1 = initial_subkey.Replace(@"\", @"\\");

            //删除路径
            string initial_subkey2 = textBox3.Text;
            string subkey2 = initial_subkey2.Replace(@"\", @"\\");
             
            string Num = textBox5.Text;      //循环阈值
            string Quantity = textBox4.Text;  //创建数量
            RegistryKey key_Root = GetRegDomain(comboBox1.Text);
            RegistryHelper dh = new RegistryHelper();
            if (string.IsNullOrEmpty(Quantity))
            {
                MessageBox.Show("创建键值个数输入为空！");
                return;
            }
            else
            {
                if (validateNum(Num))
                {
                    int num_stress = Convert.ToInt32(Num);
                    while (true)
                    {
                        try
                        {
                            Task t1 = new Task(() => { dh.DeleteKeyValue(key_Root, subkey2, num_stress); });
                            t1.Start();
                            Task t2 = new Task(() => { dh.CreateKeyValue(key_Root, subkey1, Convert.ToInt32(Quantity), num_stress); });
                            t2.Start();
                            button1.Enabled = false; button1.Text = "执行中...";
                            await Task.Delay(num_stress);
                        }
                        catch (Exception)
                        { continue; }
                        finally
                        {
                            IDisposable disposable = dh as IDisposable;
                            if (disposable != null)
                                disposable.Dispose();
                        }
                    }
                }
                else
                { MessageBox.Show("请输入整数！"); }
            }

        }
        
        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {if (textBox1.ForeColor == Color.Gray)
            {
                textBox1.Text = "";
                textBox1.Focus();

                Font MyFont = new Font("宋体", 9, FontStyle.Regular);
                textBox1.ForeColor = Color.Black;
                textBox1.Font = MyFont;
            }
        }


        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                Font MyFont = new Font("宋体", 9, FontStyle.Regular);
                
                textBox1.Text = "*例:software\\test1\\test2\\test3,如选择值则“test3”为键值名称！";
                textBox1.ForeColor = Color.Gray;
                textBox1.Font = MyFont;
            }

        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox2.ForeColor == Color.Gray)
            {
                textBox2.Text = "";
                textBox2.Focus();

                Font MyFont = new Font("宋体", 9, FontStyle.Regular);
                textBox2.ForeColor = Color.Black;
                textBox2.Font = MyFont;
            }

        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == string.Empty)
            {
                Font MyFont = new Font("宋体", 9, FontStyle.Regular);

                textBox2.Text = "请先选择Root项，再输入子项！创建键值个数不为空！";
                textBox2.ForeColor = Color.Gray;
                textBox2.Font = MyFont;
            }
        }

        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox3.ForeColor == Color.Gray)
            {
                textBox3.Text = "";
                textBox3.Focus();

                Font MyFont = new Font("宋体", 9, FontStyle.Regular);
                textBox3.ForeColor = Color.Black;
                textBox3.Font = MyFont;
            }
        }


        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == string.Empty)
            {
                Font MyFont = new Font("宋体", 9, FontStyle.Regular);

                textBox3.Text = "请先选择Root项，再输入子项！";
                textBox3.ForeColor = Color.Gray;
                textBox3.Font = MyFont;
            }
        }

        

        private void textBox5_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox5.ForeColor == Color.Gray)
            {
                textBox5.Text = "";
                textBox5.Focus();

                Font MyFont = new Font("宋体", 9, FontStyle.Regular);
                textBox5.ForeColor = Color.Black;
                textBox5.Font = MyFont;
            }
        }
        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == string.Empty)
            {
                Font MyFont = new Font("宋体", 9, FontStyle.Regular);

                textBox5.Text = "阈值(ms)";
                textBox5.ForeColor = Color.Gray;
                textBox5.Font = MyFont;
            }
        }

        private void textBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox4.ForeColor == Color.Gray)
            {
                textBox4.Text = "";
                textBox4.Focus();

                Font MyFont = new Font("宋体", 9, FontStyle.Regular);
                textBox4.ForeColor = Color.Black;
                textBox4.Font = MyFont;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == string.Empty)
            {
                Font MyFont = new Font("宋体", 9, FontStyle.Regular);

                textBox4.Text = "单位(个)";
                textBox4.ForeColor = Color.Gray;
                textBox4.Font = MyFont;
            }
        }
    }
}

using Microsoft.Win32;

namespace DMData.Code
{
    public class KeyEngine
    {
        private const string C_Str1 = "101501";
        private const string C_Str2 = "101502";
        private const string C_Str3 = "101503";
        private const int C_Int1 = 0;
        private const int C_Int2 = 1;
        private const int C_Int3 = 2;
        private static KeyEngine ke = null;
        private int i_1 = 1;
        private int i_2;
        private string s_1;
        private bool b_1 = false;
        public static KeyEngine Instance
        {
            get
            {
                if (ke == null)
                {
                    ke = Func_1();
                }
                return ke;
            }
        }
        public int DaysCounter
        {
            get
            {
                return i_1;
            }
            set
            {
                i_1 = value;
            }
        }
        public string LicenseKey
        {
            get
            {
                return s_1;
            }
        }
        public int Version
        {
            get
            {
                return i_2;
            }
            set
            {
                i_2 = value;
            }
        }
        public bool IsRegular
        {
            get
            {
                return b_1;
            }
            set
            {
                b_1 = value;
            }
        }

        public static KeyEngine GetInstance()
        {
            if (ke == null)
            {
                ke = Func_1();
            }
            return ke;
        }
        private static KeyEngine Func_1()
        {
            if (ke != null)
            {
                return ke;
            }
            return new KeyEngine();
        }
        public int OpenSubKey(string pStr)
        {
            RegistryKey rk = null;
            try
            {
                rk = Registry.LocalMachine.OpenSubKey(pStr, !b_1);
            }
            catch
            {
                int result = 1;
                return result;
            }
            if (rk == null)
            {
                return 1;
            }
            try
            {
                string text = rk.GetValue("101503").ToString();
                i_2 = int.Parse(text);
            }
            catch
            {
                i_2 = 1010;
            }
            try
            {
                s_1 = rk.GetValue("101501").ToString();
                i_1 = int.Parse(rk.GetValue("101502").ToString());
                if (!b_1)
                {
                    rk.SetValue("101502", i_1 + 1);
                }
                rk.Close();
            }
            catch
            {
                int result = 2;
                return result;
            }
            return 0;
        }
        public ProductInfo GetProductInfo()
        {
            return DoorEngine.Instance.Open(LicenseKey);
        }
    }
}
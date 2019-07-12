using Microsoft.Win32;

namespace DMData.Code
{
    public class KeyEngine
    {
        private const string _sc1 = "101501";
        private const string _cs2 = "101502";
        private const string _cs3 = "101503";
        private const int _ci1 = 0;
        private const int _ci2 = 1;
        private const int _ci3 = 2;
        private int _i1 = 1;
        private int _i2;
        private string _s1;
        private bool _b1 = false;

        private static KeyEngine _Instance = null;
        public static KeyEngine Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = Func_1();
                }
                return _Instance;
            }
        }

        public int DaysCounter
        {
            get
            {
                return _i1;
            }
            set
            {
                _i1 = value;
            }
        }
        public string LicenseKey
        {
            get
            {
                return _s1;
            }
        }
        public int Version
        {
            get
            {
                return _i2;
            }
            set
            {
                _i2 = value;
            }
        }
        public bool IsRegular
        {
            get
            {
                return _b1;
            }
            set
            {
                _b1 = value;
            }
        }

        public static KeyEngine GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = Func_1();
            }
            return _Instance;
        }
        private static KeyEngine Func_1()
        {
            if (_Instance != null)
            {
                return _Instance;
            }
            return new KeyEngine();
        }
        public int OpenSubKey(string pStr)
        {
            RegistryKey rk = null;
            try
            {
                rk = Registry.LocalMachine.OpenSubKey(pStr, !_b1);
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
                _i2 = int.Parse(text);
            }
            catch
            {
                _i2 = 1010;
            }
            try
            {
                _s1 = rk.GetValue("101501").ToString();
                _i1 = int.Parse(rk.GetValue("101502").ToString());
                if (!_b1)
                {
                    rk.SetValue("101502", _i1 + 1);
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
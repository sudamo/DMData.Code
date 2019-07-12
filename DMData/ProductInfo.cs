using System;

namespace DMData.Code
{
    [Serializable]
    public class ProductInfo
    {
        private string _s1;
        private int _i1;
        private int _i2;
        private bool _b1;
        public bool IsEmpty
        {
            get
            {
                return _b1;
            }
        }
        public string ProductName
        {
            get
            {
                return _s1;
            }
            set
            {
                _s1 = value;
                _b1 = false;
            }
        }
        public int UserNumber
        {
            get
            {
                return _i1;
            }
            set
            {
                _i1 = value;
                _b1 = false;
            }
        }
        public int DaysNumber
        {
            get
            {
                return _i2;
            }
            set
            {
                _i2 = value;
                _b1 = false;
            }
        }

        public ProductInfo() : this("MDData", 1, 30)
        {
            _b1 = true;
        }
        public ProductInfo(string pStr, int pInt1, int pInt2)
        {
            _s1 = pStr;
            _i1 = pInt1;
            _i2 = pInt2;
            _b1 = false;
        }
    }
}
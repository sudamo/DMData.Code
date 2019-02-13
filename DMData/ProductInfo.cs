using System;

namespace DMData.Code
{
    [Serializable]
    public class ProductInfo
    {
        private string s_1;
        private int i_1;
        private int i_2;
        private bool b_1;
        public bool IsEmpty
        {
            get
            {
                return b_1;
            }
        }
        public string ProductName
        {
            get
            {
                return s_1;
            }
            set
            {
                s_1 = value;
                b_1 = false;
            }
        }
        public int UserNumber
        {
            get
            {
                return i_1;
            }
            set
            {
                i_1 = value;
                b_1 = false;
            }
        }
        public int DaysNumber
        {
            get
            {
                return i_2;
            }
            set
            {
                i_2 = value;
                b_1 = false;
            }
        }

        public ProductInfo() : this("MDData", 1, 30)
        {
            b_1 = true;
        }
        public ProductInfo(string pStr, int pInt1, int pInt2)
        {
            s_1 = pStr;
            i_1 = pInt1;
            i_2 = pInt2;
            b_1 = false;
        }
    }
}
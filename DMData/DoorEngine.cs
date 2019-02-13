using System;

namespace DMData.Code
{
    public class DoorEngine
    {
        private static DoorEngine de = null;
        private static char[] c_array = new char[]
        {
            '6','A','C','8','D','U','4','I','K','2'
            ,'L','5','Q','1','R','S','T','M','Y','Z'
            ,'3','N','V','O','7','0','H','P','X','G'
            ,'B','J','E','9','F','W'
        };
        private static int i_1 = c_array.Length;
        private static int i_2 = 5;
        private static int i_3 = 5;
        private static int i_4 = i_2 * i_3;
        private static int i_5 = 6;
        private static int i_6 = 36;
        public static DoorEngine Instance
        {
            get
            {
                if (de == null)
                {
                    de = Func_1();
                }
                return de;
            }
        }

        public ProductInfo Open(string pStr)
        {
            ProductInfo productInfo = new ProductInfo();
            if (pStr == string.Empty)
            {
                return productInfo;
            }
            if (pStr.Length != i_4 + i_2 - 1)
            {
                return productInfo;
            }
            char[] array = pStr.ToCharArray();
            for (int i = 1; i <= i_2 - 1; i++)
            {
                if (array[i * (i_3 + 1) - 1] != '-')
                {
                    return productInfo;
                }
            }
            pStr = "";
            for (int i = 0; i < i_2; i++)
            {
                pStr += new string(array, i * (i_3 + 1), i_3);
            }
            array = pStr.ToCharArray();
            int num = Func_3(array[4]);
            int num2 = 0;
            array[4] = Fucn_2(0);
            for (int i = 0; i < i_4; i++)
            {
                num2 += (int)char.GetNumericValue(array[i]);
            }
            if (num != num2 % i_1)
            {
                return productInfo;
            }
            int num3 = Func_3(array[0]);
            int num4 = Func_3(array[1]);
            int num5 = Func_3(array[2]);
            int num6 = Func_3(array[3]);
            int num7 = Func_3(array[5]);
            if (i_5 + num4 + num5 + num6 >= i_4)
            {
                return productInfo;
            }
            int num8 = i_4 - (i_5 + num4 + num5 + num7 + num6);
            char[] array2 = new char[num5 + num7 + num8];
            for (int i = 0; i < num5 + num7 + num8; i++)
            {
                array2[i] = Fucn_2(Func_3(array[i + i_5 + num4]) - num3);
            }
            if (num4 >= num6)
            {
                productInfo.UserNumber = Func_4(new string(array2, 0, num7));
                productInfo.DaysNumber = Func_4(new string(array2, num7, num8));
                productInfo.ProductName = new string(array2, num7 + num8, num5);
            }
            else
            {
                productInfo.UserNumber = Func_4(new string(array2, num5, num7));
                productInfo.DaysNumber = Func_4(new string(array2, num5 + num7, num8));
                productInfo.ProductName = new string(array2, 0, num5);
            }
            return productInfo;
        }
        public static DoorEngine GetInstance()
        {
            if (de == null)
            {
                de = Func_1();
            }
            return de;
        }
        private static DoorEngine Func_1()
        {
            if (de != null)
            {
                return de;
            }
            return new DoorEngine();
        }
        private char Fucn_2(int pInt)
        {
            while (pInt < 0)
            {
                pInt += i_1;
            }
            while (pInt >= i_1)
            {
                pInt -= i_1;
            }
            return c_array[pInt];
        }
        private int Func_3(char pChr)
        {
            for (int i = 0; i < i_1; i++)
            {
                if (c_array[i] == pChr)
                {
                    return i;
                }
            }
            return 0;
        }
        private int Func_4(string pStr)
        {
            int num = 0;
            char[] array = pStr.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                num += Func_5(array[i]) * (int)Math.Pow(i_6, (array.Length - 1 - i));
            }
            return num;
        }
        private int Func_5(char pChr)
        {
            if (pChr > '@' && pChr < '[')
            {
                return (pChr - '7');
            }
            if (pChr > '/' && pChr < ':')
            {
                return (pChr - '0');
            }
            return -1;
        }
    }
}
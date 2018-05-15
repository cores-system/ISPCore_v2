namespace ISPCore.Engine.Base
{
    public class EndOfText
    {
        /// <summary>
        /// Правильное окончание текста
        /// </summary>
        /// <param name="s1">минуту</param>
        /// <param name="s2">минуты</param>
        /// <param name="s3">минут</param>
        /// <param name="x">число</param>
        public static string get(string s1, string s2, string s3, int x)
        {
            int n = x % 100;
            if ((n > 10) && (n < 20)) { s1 = null; s2 = null; return s3; }
            else
            {
                switch (x % 10)
                {
                    case 1: return s1;
                    case 2:
                    case 3:
                    case 4: return s2;
                    case 0:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9: return s3;
                }
            }

            s1 = null; s2 = null; s3 = null;
            return "";
        }
    }
}

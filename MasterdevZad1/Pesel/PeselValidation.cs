using NuGet.Common;
using System;

namespace MasterdevZad1.Pesel
{
    public static class PeselValidator
    {
        /// <summary>
        /// Mnozniki dla PESEL
        /// </summary>
        private static readonly int[] mnozniki = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

        /// <summary>
        /// Sprawdza PESEL pod kątem poprawności
        /// </summary>
        /// <param name="pesel">PESEL string</param>
        /// <returns>true = OK; false = NOK</returns>
        public static bool ValidatePesel(string pesel)
        {
            bool toRet = false;
            try
            {
                if (pesel.Length == 11)
                {
                    toRet = CountSum(pesel).Equals(pesel[10].ToString());
                }
                
            }
            catch (Exception)
            {
                toRet = false;
            }
            return toRet;
        }

        /// <summary>
        /// Liczy sumę cyfr znaczących PESEL
        /// </summary>
        /// <param name="pesel">PESEL string</param>
        /// <returns>SUMA string</returns>
        private static string CountSum(string pesel)
        {
            int sum = 0;
            for (int i = 0; i < mnozniki.Length; i++)
            {
                sum += mnozniki[i] * int.Parse(pesel[i].ToString());
            }

            int reszta = sum % 10;
            return reszta == 0 ? reszta.ToString() : (10 - reszta).ToString();
        }
    }
}

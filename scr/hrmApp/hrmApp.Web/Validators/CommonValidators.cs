using System.Text.RegularExpressions;

// for Hungarian personal Identification data validation only

namespace hrmApp.Web.Validators
{
    public class CommonValidators
    {

        #region CheckTAJ

        //  0: Jó TAJ szám
        // -1: Rossz a kapott érték hossza (csak 9 /elválasztás nélkül/ 
        //     vagy 11 /elválasztással/ karakter lehet)
        // -2: A kapott érték nem csak számjegyet tartalmaz (kivéve: elválasztás)
        // -3: A kapott érték CDV hibás
        public static int CheckTAJ(string cTAJ)
        {
            cTAJ = Regex.Replace(cTAJ, "\\s|-", "");

            if (cTAJ.Length != 9) return -1;

            long j;
            if (!long.TryParse(cTAJ, out j)) return -2;

            int nCDV = 0;
            for (int i = 0; i < 8; i++)
            {
                nCDV += int.Parse(cTAJ[i].ToString()) * (3 + 4 * (i % 2));
            }
            if (int.Parse(cTAJ[8].ToString()) != (nCDV % 10)) return -3;

            return 0;
        }

        #endregion

        #region CheckAdoszam

        ///  0: Jó adószám
        /// -1: Rossz a kapott érték hossza (csak 11 /elválasztás nélkül/ 
        //      vagy 13 /elválasztással/ karakter lehet)
        /// -2: A kapott érték nem csak számjegyet tartalmaz (kivéve: elválasztás)
        /// -3: A 9. helyen nem 1,2 vagy 3 szerepel (adómentes, adóköteles, EVA)
        /// -4: Az utolsó két számjegy nem a következők egyike: 02-20, 41-44, 51
        /// -5: A kapott érték CDV hibás

        public static int CheckAdoszam(string cAdo)
        {
            cAdo = Regex.Replace(cAdo, "\\s|-", "");

            if (cAdo.Length != 11) return -1;

            long j;
            if (!long.TryParse(cAdo, out j)) return -2;

            if (cAdo[8] != '1' && cAdo[8] != '2' && cAdo[8] != '3') return -3;

            int nCDV = int.Parse(cAdo.Substring(9, 2));
            if (!((nCDV > 1 && nCDV < 21) ||
                 (nCDV > 42 && nCDV < 45) || nCDV == 51)) return -4;

            nCDV = 0;
            for (int i = 0; i < 7; i++)
            {
                nCDV += int.Parse(cAdo[i].ToString()) * aCDV[(i % 4)];
            }
            if (int.Parse(cAdo[7].ToString()) != ((10 - (nCDV % 10)) % 10)) return -5;

            return 0;
        }
        #endregion

        #region CheckBankSzamla

        //  0: Jó bankszámlaszám
        // -1: Rossz a kapott érték hossza (csak 16 /elválasztás nélkül/ 
        //     vagy 17 /elválasztással/ , illetve 24 /elválasztás nélkül/ 
        //     vagy 26 /elválasztással/ karakter lehet)
        // -2: A kapott érték nem csak számjegyet tartalmaz (kivéve: elválasztás)
        // -3: A kapott érték CDV hibás

        static int[] aCDV = new int[] { 9, 7, 3, 1 };

        public static int CheckBankSzamla(string cSzla)
        {
            cSzla = Regex.Replace(cSzla, "\\s|-", "");

            if (cSzla.Length != 16 && cSzla.Length != 24) return -1;

            if (cSzla.Length == 16) cSzla += "00000000";

            if (!Regex.IsMatch(cSzla, "^\\d+$")) return -2;

            int nCDV = 0;
            for (int i = 0; i < 7; i++)
            {
                nCDV += int.Parse(cSzla[i].ToString()) * aCDV[(i % 4)];
            }
            if (int.Parse(cSzla[7].ToString()) != ((10 - (nCDV % 10)) % 10)) return -3;

            nCDV = 0;
            for (int i = 8; i < 15; i++)
            {
                nCDV += int.Parse(cSzla[i].ToString()) * aCDV[((i - 8) % 4)];
            }
            if (int.Parse(cSzla[15].ToString()) != ((10 - (nCDV % 10)) % 10)) return -3;

            return 0;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using System.Globalization;

namespace Citizens
{
    public class CitizenRegister : ICitizenRegistry
    {
        public ICitizen[] citizenArray = new ICitizen[20];
        public DateTime lastRegDate;

        public ICitizen this[string id]
        {
            get
            {
                if (String.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException();
                }

                for (int j = 0; j < citizenArray.Length; j++)
                {
                    if (citizenArray[j] != null && string.Equals(citizenArray[j].VatId, id))
                    {
                        return citizenArray[j];
                    }
                }
                return null;
            }
        }

        public void Register(ICitizen citizen)
        {
            if (String.IsNullOrEmpty(citizen.VatId))
            {
                int sn = 0;
                int[] mul = { -1, 5, 7, 9, 4, 6, 10, 5, 7 };

                switch (citizen.Gender)
                {
                    case Gender.Female:
                        {
                            sn = 0;
                            break;
                        }
                    case Gender.Male:
                        {
                            sn = 1;
                            break;
                        }
                }
                string dateString = ((citizen.BirthDate - new DateTime(1899, 12, 31)).TotalDays).ToString().PadLeft(5, '0');
                string serNString;
                string checkNumber;
                do
                {
                    serNString = sn.ToString().PadLeft(4, '0');

                    int sum = 0;
                    string charArr = string.Concat(dateString, serNString);

                    for (int i = 0; i < charArr.Length; i++)
                    {
                        sum += int.Parse(charArr[i].ToString()) * mul[i];
                    }
                    checkNumber = ((sum % 11) % 10).ToString();
                    sn += 2;
                }
                while (this[string.Concat(dateString, serNString, checkNumber)] != null);

                citizen.VatId = string.Concat(dateString, serNString, checkNumber);
            }

            if (this[citizen.VatId] == null)
            {
                for (int i = 0; i < citizenArray.Length; i++)
                    if (citizenArray[i] == null)
                    {
                        citizenArray[i] = new Citizen(citizen);
                        lastRegDate = SystemDateTime.Now();
                        break;
                    }
            }
            else
            {
                throw new InvalidOperationException();
            }

        }

        public string Stats()
        {
            int maleN = 0;
            int femaleN = 0;

            foreach (ICitizen ob in citizenArray)
            {
                if (ob != null)
                    if (int.Parse(ob.VatId[8].ToString()) % 2 == 0)
                    {
                        femaleN++;
                    }
                    else
                    {
                        maleN++;
                    }
            }
            
            string retStr = String.Join(" ", "man".ToQuantity(maleN), "and", "woman".ToQuantity(femaleN));

            double diff = (lastRegDate - SystemDateTime.Now()).TotalDays;

            if ((maleN > 0 || femaleN > 0) && diff < 0)
            {
                retStr = string.Join(". ", retStr, "Last registration was " +
                    DateTimeOffset.Now.AddDays(diff).Humanize(culture: new CultureInfo("en-US")));
            }
            return retStr;
        }
    }
}

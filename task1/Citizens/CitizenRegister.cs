using System;
using Humanizer;
using System.Globalization;

namespace Citizens
{
    public class CitizenRegister : ICitizenRegistry
    {
        private readonly DateTime refDate   = new DateTime(1899, 12, 31);
        private ICitizen[] citizenArray     = new ICitizen[20];
        private int lastInd                 = 0;
        private DateTime lastRegDate;

        public ICitizen this[string id]
        {
            get
            {
                if (String.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException("Parameter cannot be null");
                }

                for (int j = 0; j < this.citizenArray.Length; j++)
                {
                    if (this.citizenArray[j] != null && string.Equals(this.citizenArray[j].VatId, id))
                    {
                        return this.citizenArray[j];
                    }
                }
                return null;
            }
        }

        public void Register(ICitizen citizen)
        {
            int sum     = 0;
            int sn      = 0;
            int[] mul   = { -1, 5, 7, 9, 4, 6, 10, 5, 7 };
            
            string serNString;
            string checkNumber;
            string dateString;

            TimeSpan livedPeriod;

            if (String.IsNullOrEmpty(citizen.VatId))
            {
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

                livedPeriod = citizen.BirthDate - this.refDate;

                dateString = livedPeriod.Days.ToString().PadLeft(5, '0');
                
                do
                {
                    serNString = sn.ToString().PadLeft(4, '0');

                    var charArr = string.Concat(dateString, serNString);

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
                for (int i = 0; i < this.citizenArray.Length; i++)
                {
                    if (this.citizenArray[i] == null)
                    {
                        this.citizenArray[i]    = new Citizen(citizen);
                        this.lastRegDate        = SystemDateTime.Now();
                        break;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Same person cannot be added twice");
            }

        }

        public string Stats()
        {
            int maleN   = 0;
            int femaleN = 0;
            string retStr;
            TimeSpan lastRegistrationOccur;

            foreach (ICitizen ob in this.citizenArray)
            {
                if (ob != null)
                {
                    if (int.Parse(ob.VatId[8].ToString()) % 2 == 0)
                    {
                        femaleN++;
                    }
                    else
                    {
                        maleN++;
                    }
                }
            }
            
            retStr = String.Join(" ", "man".ToQuantity(maleN), "and", "woman".ToQuantity(femaleN));

            lastRegistrationOccur = this.lastRegDate - SystemDateTime.Now();

            if (maleN > 0 || femaleN > 0)
            {
                retStr = string.Join(". ", retStr, "Last registration was " +
                    SystemDateTime.Now().AddDays(lastRegistrationOccur.Days).Humanize(utcDate: false, culture: new CultureInfo("en-US")));
            }
            return retStr;
        }
    }
}

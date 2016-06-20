using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace Citizens
{
    public class Citizen : ICitizen
    {
        private Gender gender;
        private DateTime dateOfBirth;
        private string lastName;
        private string firstName;
        private string vatId;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
        }

        public Gender Gender
        {
            get
            {
                return this.gender;
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return this.dateOfBirth;
            }
        }

        public string VatId
        {
            get
            {
                return this.vatId;
            }

            set
            {
                this.vatId = value;
            }
        }

        public Citizen(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
        {
            this.firstName      = firstName.Transform(To.LowerCase, To.TitleCase);
            this.lastName       = lastName.Transform(To.LowerCase, To.TitleCase);

            TimeSpan liveTime   = SystemDateTime.Now() - dateOfBirth;

            if (liveTime.Days >= 0)
            {
                this.dateOfBirth = dateOfBirth.Date;
            }
            else
            {
                throw new ArgumentException(String.Format("Person who was not born cannot be added. The passed date is {0}", dateOfBirth), "dateOfBirth");
            }

            if (Enum.IsDefined(typeof(Gender), gender))
            {
                this.gender = gender;
            }
            else
            {
                throw new ArgumentOutOfRangeException("gender", "Passed improper enum element");
            }
        }

        public Citizen(ICitizen ob)
        {
            this.firstName      = ob.FirstName;
            this.lastName       = ob.LastName;
            this.dateOfBirth    = ob.BirthDate;
            this.gender         = ob.Gender;
            this.vatId          = ob.VatId;
        }
    }
}

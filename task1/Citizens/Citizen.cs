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
                return firstName;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
        }

        public Gender Gender
        {
            get
            {
                return gender;
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return dateOfBirth;
            }
        }

        public string VatId
        {
            get
            {
                return vatId;
            }

            set
            {
                vatId = value;
            }
        }



        public Citizen()
        { }

        public Citizen(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
        {


            this.firstName = firstName.Transform(To.LowerCase, To.TitleCase);
            this.lastName = lastName.Transform(To.LowerCase, To.TitleCase);
            if ((SystemDateTime.Now() - dateOfBirth).TotalDays >= 0)
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
                throw new ArgumentOutOfRangeException();
            }
        }

        public Citizen(ICitizen ob)
        {
            this.firstName = ob.FirstName;
            this.lastName = ob.LastName;
            this.dateOfBirth = ob.BirthDate;
            this.gender = ob.Gender;
            this.vatId = ob.VatId;
        }
    }
}

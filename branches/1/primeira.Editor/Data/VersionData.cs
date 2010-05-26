using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class VersionData
    {
        [DataMember()]
        public Guid Id { get; set; }

        [DataMember()]
        public string Number { get; set; }

        [DataMember()]
        public Authorship Author { get; set; }

        public VersionData(string id, string number, string author, string info, string email, string website)
        {
            if(id != null)
                this.Id = new Guid(id);

            this.Number = number;

            this.Author = new Authorship();

            this.Author.Name = author;

            this.Author.Info = info;

            this.Author.Email = email;

            this.Author.Website = website;
        }

        public bool Filter(VersionFilter filter)
        {
            if (Id != filter.Target)
                return false;

            bool result = true;

            if (filter.Numbers != null && filter.Numbers.Length > 0)
            {
                result = filter.Numbers.Contains(Number);
            }

            if (result && filter.MinNumber != null && filter.MinNumber.Length > 0)
            {
                result = NumberCompareMinorOrEqual(Number, filter.MinNumber);
            }

            if (result && filter.MaxNumber != null && filter.MaxNumber.Length > 0)
            {
                result = NumberCompareMajorOrEqual(Number, filter.MaxNumber);
            }

            return result;
        }

        public bool FilterAny(VersionFilter[] filters)
        {
            foreach (VersionFilter filter in filters) 
            {
                if (Filter(filter))
                    return true;
            }

            return false;
        }

        private static bool NumberCompareMajorOrEqual(string Number1, string Number2)
        {
            return NumberCompare(Number1, Number2);
        }

        private static bool NumberCompareMinorOrEqual(string Number1, string Number2)
        {
            return NumberCompare(Number1, Number2, -1);
        }

        private static bool NumberCompare(string Number1, string Number2, int Direction = 1, bool ReturnTrueIfEqual = true)
        {
            if (Number1 == null && Number2 == null)
                return ReturnTrueIfEqual;

            if (Number1 == null || Number2 == null)
                return false;

            string[] ar1 = Number1.Split('.');
            string[] ar2 = Number2.Split('.');

            //use the minor length
            int length = ar1.Length < ar2.Length ? ar1.Length : ar2.Length;

            int a1, a2;
            for (int i = 0; i < length; i++)
            {
                a1 = Convert.ToByte(ar1[i]);

                a2 = Convert.ToByte(ar2[i]);

                if (a1 * Direction < a2 * Direction)
                    return false;
                else if (a1 * Direction > a2 * Direction)
                    return true;
            }

            //there is no noticiable difference comparing until the minor length array
            //so the larger array should be the major version.
            if (ar1.Length != ar2.Length) 
                return ar1.Length * Direction > ar2.Length * Direction;

            return ReturnTrueIfEqual;
        }

        public static bool operator >(VersionData v1, VersionData v2)
        {
            if (v1.Id != v2.Id)
                return false;

            return NumberCompare(v1.Number, v2.Number, 1, false);
        }

        public static bool operator <(VersionData v1, VersionData v2)
        {
            if (v1.Id != v2.Id)
                return false;

            return NumberCompare(v1.Number, v2.Number, -1, false);
        }

        public static implicit operator VersionFilter(VersionData v)
        {
            VersionFilter res = new VersionFilter();

            res.Target = v.Id;
            res.Numbers = new string[1] { v.Number };

            return res;
        }
    }
}

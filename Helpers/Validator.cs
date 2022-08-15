using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class Validator
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidName(string name)
        {
            return Regex.Match(name, @"^[a-zA-Z].*[\s\.]*$").Success;
        }

        public static bool HasMissingValues(Dictionary<string, object>.ValueCollection values)
        {
            foreach (object value in values)
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

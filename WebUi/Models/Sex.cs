using System;

namespace WebUi.Models
{
    public enum Sex
    {
        Mail, Femail, Third
    }

    public static class SexExtensions
    {
        public static String ToViewString(this Sex sex)
        {
            switch (sex)
            {
                case Sex.Mail:
                    return "Mail";
                case Sex.Femail:
                    return "Femail";
                case Sex.Third:
                    return "Thrid";
                default:
                    throw new ArgumentOutOfRangeException("sex");
            }
        }
    }
}
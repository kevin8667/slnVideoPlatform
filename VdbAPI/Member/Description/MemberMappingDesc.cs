using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.Description
{
    public class MemberMappingDesc
    {
        public class Gender
        {
            public const string Male = "M";
            public const string Female = "F";
            public static string GetDesc(string type)
            {
                switch (type)
                {
                    case Gender.Female:
                        return "女";
                    case Gender.Male:
                        return "男";
                    default:
                        return string.Empty;

                }
            }
        }

        public class Status
        {
            public const string active = "Y";
            public const string inactive = "N";

            public static string GetDesc(string Status)
            {
                switch (Status)
                {
                    case active:
                        return "有效";
                    case inactive:
                        return "失效";
                    default:
                        return Status;
                }
            }

            public static string GetButtonText(string Status)
            {
                switch (Status)
                {
                    case active:
                    case "有效":
                        return "凍結";
                    case inactive:
                    case "失效":
                        return "解凍";
                    default:
                        return Status;
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelData.Extensions
{
    public class UserSameUser : EqualityComparer<DTO.BB.User>
    {
        public override bool Equals(DTO.BB.User u1, DTO.BB.User u2)
        {
            if (u1 == null && u2 == null)
                return true;
            else if (u1 == null || u2 == null)
                return false;

            if (u1.UserId == u2.UserId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode(DTO.BB.User u)
        {
            return u.UserId.GetHashCode();
        }

    }
}

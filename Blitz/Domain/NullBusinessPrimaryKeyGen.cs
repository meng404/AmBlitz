using System;

namespace Blitz.Domain
{
    public class NullBusinessPrimaryKeyGen : IBusinessPrimaryKeyGen
    {
        public object Gen(Type BusinessPrimaryKeyType)
        {
            return null;
        }
    }
}

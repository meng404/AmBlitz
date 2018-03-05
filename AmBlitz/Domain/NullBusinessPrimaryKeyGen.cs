using System;

namespace AmBlitz.Domain
{
    public class NullBusinessPrimaryKeyGen : IBusinessPrimaryKeyGen
    {
        public object Gen(Type businessPrimaryKeyType)
        {
            return null;
        }
    }
}

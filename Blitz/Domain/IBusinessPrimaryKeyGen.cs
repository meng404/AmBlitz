using Blitz.Dependency;
using System;

namespace Blitz.Domain
{
    public interface IBusinessPrimaryKeyGen: ISingletonDependency
    {
        object Gen(Type BusinessPrimaryKeyType);
    }
}

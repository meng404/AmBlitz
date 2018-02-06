using AmBlitz.Dependency;
using System;

namespace AmBlitz.Domain
{
    public interface IBusinessPrimaryKeyGen: ISingletonDependency
    {
        object Gen(Type BusinessPrimaryKeyType);
    }
}

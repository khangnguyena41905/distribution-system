using System.Reflection;

namespace IDENTITY.INFRASTRUCTURE;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
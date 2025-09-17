using System.Reflection;

namespace IDENTITY.CONTRACT;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
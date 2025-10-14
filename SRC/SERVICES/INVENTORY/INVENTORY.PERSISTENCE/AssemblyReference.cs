using System.Reflection;

namespace INVENTORY.PERSISTENCE;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
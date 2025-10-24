using System.Reflection;

namespace INVENTORY.INFRASTRUCTURE;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
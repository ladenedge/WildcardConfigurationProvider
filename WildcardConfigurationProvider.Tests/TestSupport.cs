using System.IO;
using System.Reflection;

namespace WildcardConfigurationProvider.Tests;

public class TestSupport
{
    public static string LocateTestFileDirectory()
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        return Path.Combine(dir, "Configs");
    }

    public static string LocateTestFile(string filename)
    {
        var dir = LocateTestFileDirectory();
        return Path.Combine(dir, filename);
    }
}

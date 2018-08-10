# nunit-in-code-runner
Simple flexible library that will run any NUnit fixtures, in your own code, in your own set up.

```C#
internal static class Program
{
    public static int Main(string[] args)
    {
        // runs all classes with TextFixture attribute
        // in the current assembly
        return new TestRunner().Run();
    }
}
```


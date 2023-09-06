using System;

namespace UAS.Tests
{
    [Flags]
    public enum TestState
    {
        None = 0,
        State1 = 1 << 0,
        State2 = 1 << 1,
    }
}
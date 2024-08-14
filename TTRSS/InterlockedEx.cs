namespace TTRSS;

class InterlockedEx {
    public static T Read<T>(ref T location) where T : class
        => Interlocked.CompareExchange(ref location, null, null);
}
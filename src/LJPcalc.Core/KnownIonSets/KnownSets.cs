namespace LJPcalc.Core.KnownIonSets;

public static class KnownSets
{
    public static IKnownIonSet[] GetAll()
    {
        return System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IKnownIonSet)))
            .Select(x => (IKnownIonSet)Activator.CreateInstance(x)!)
            .ToArray();
    }
}

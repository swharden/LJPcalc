namespace LJPcalc.Core;

public static class IonSorting
{
    /// <summary>
    /// This strategy places the ion with the largest difference between C0 and CL last
    /// and the remaining ion with the largest CL second to last.
    /// </summary>
    public static Ion[] PreCalculationSort(Ion[] inputIons)
    {
        List<Ion> ionList = inputIons.Select(x => x.Clone()).ToList();

        // absolute largest cL should be last
        Ion ionWithLargestCL = ionList.OrderBy(x => x.CL).Last();
        ionList.Remove(ionWithLargestCL);

        // largest diff should be second from last
        Ion ionWithLargestCDiff = ionList.OrderBy(x => Math.Abs(x.CL - x.C0)).Last();
        ionList.Remove(ionWithLargestCDiff);

        // place the removed ions back in the proper order
        ionList.Add(ionWithLargestCDiff);
        ionList.Add(ionWithLargestCL);
        return ionList.ToArray();
    }
}

using Proxoft.Heatmaps.Core.Internals;

namespace Proxoft.Heatmaps.Core.Tests.Common;

internal static class IdwTriangleFactory
{
    public static IdwTriangle Create(decimal v0x0, decimal v60x0, decimal v30x30, decimal[] levels) =>
        IdwTriangle.FromPoints("0x0".Value(v0x0), "60x0".Value(v60x0), "30x30".Value(v30x30), levels);
}

namespace Graphs;

public sealed record Route(IReadOnlyList<string> Vertices, long DistanceKm);

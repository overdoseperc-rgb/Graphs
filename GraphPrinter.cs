namespace Graphs;

public static class GraphPrinter
{
    public static void Print(Graph graph)
    {
        Console.WriteLine("Список рёбер:");
        foreach (var edge in graph.Edges)
            Console.WriteLine($"{edge.From} — {edge.To}: {edge.DistanceKm} км");

        Console.WriteLine("\nСписок смежностей:");
        foreach (string vertex in graph.Vertices)
            Console.WriteLine($"{vertex}: {string.Join(", ", graph.GetNeighbors(vertex).Select(neighbor => $"{neighbor.Vertex} ({neighbor.DistanceKm} км)"))}");

        Console.WriteLine("\nМатрица смежностей: длины дорог в км, 0 — дороги нет");
        Console.Write("".PadRight(12));
        foreach (string vertex in graph.Vertices)
            Console.Write(vertex.PadRight(12));
        Console.WriteLine();
        int[,] matrix = graph.CreateAdjacencyMatrix();
        for (int row = 0; row < graph.Vertices.Count; row++)
        {
            Console.Write(graph.Vertices[row].PadRight(12));
            for (int column = 0; column < graph.Vertices.Count; column++)
                Console.Write(matrix[row, column].ToString().PadRight(12));
            Console.WriteLine();
        }
    }
}

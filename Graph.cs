namespace Graphs;

public sealed class Graph
{
    private readonly List<string> vertices = new();
    private readonly List<Edge> edges = new();
    private readonly Dictionary<string, List<Neighbor>> adjacency = new();

    public IReadOnlyList<string> Vertices => vertices.AsReadOnly();
    public IReadOnlyList<Edge> Edges => edges.AsReadOnly();

    public void AddVertex(string vertex)
    {
        if (string.IsNullOrWhiteSpace(vertex))
            throw new ArgumentException("Название вершины не должно быть пустым.", nameof(vertex));
        if (adjacency.TryAdd(vertex, new List<Neighbor>()))
            vertices.Add(vertex);
    }

    public void AddEdge(string from, string to, int distanceKm)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Названия вершин не должны быть пустыми.");
        if (distanceKm <= 0)
            throw new ArgumentOutOfRangeException(nameof(distanceKm), "Длина дороги должна быть положительной.");
        AddVertex(from);
        AddVertex(to);
        if (adjacency[from].Any(neighbor => neighbor.Vertex == to))
            throw new ArgumentException("Дорога между этими вершинами уже существует.");
        edges.Add(new Edge(from, to, distanceKm));
        adjacency[from].Add(new Neighbor(to, distanceKm));
        if (from != to)
            adjacency[to].Add(new Neighbor(from, distanceKm));
    }

    public IReadOnlyList<Neighbor> GetNeighbors(string vertex)
    {
        if (!adjacency.TryGetValue(vertex, out var neighbors))
            throw new ArgumentException("Вершина не найдена.", nameof(vertex));
        return neighbors.AsReadOnly();
    }

    public int[,] CreateAdjacencyMatrix()
    {
        var matrix = new int[vertices.Count, vertices.Count];
        var indexes = vertices.Select((vertex, index) => (vertex, index))
            .ToDictionary(item => item.vertex, item => item.index);
        foreach (var edge in edges)
        {
            int from = indexes[edge.From];
            int to = indexes[edge.To];
            matrix[from, to] = edge.DistanceKm;
            matrix[to, from] = edge.DistanceKm;
        }
        return matrix;
    }

    public Route? FindShortestPath(string start, string finish)
    {
        if (!adjacency.ContainsKey(start) || !adjacency.ContainsKey(finish))
            throw new ArgumentException("Начальная или конечная вершина не найдена.");
        var queue = new PriorityQueue<string, long>();
        var distances = new Dictionary<string, long> { [start] = 0 };
        var previous = new Dictionary<string, string?> { [start] = null };
        queue.Enqueue(start, 0);
        while (queue.TryDequeue(out string? current, out long distance))
        {
            if (distance != distances[current])
                continue;
            if (current == finish)
            {
                var path = new List<string>();
                for (string? vertex = finish; vertex is not null; vertex = previous[vertex])
                    path.Add(vertex);
                path.Reverse();
                return new Route(path.AsReadOnly(), distance);
            }
            foreach (var neighbor in adjacency[current])
            {
                long candidate = distance + neighbor.DistanceKm;
                if (!distances.TryGetValue(neighbor.Vertex, out long known) || candidate < known)
                {
                    distances[neighbor.Vertex] = candidate;
                    previous[neighbor.Vertex] = current;
                    queue.Enqueue(neighbor.Vertex, candidate);
                }
            }
        }
        return null;
    }
}

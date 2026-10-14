using System.Text;
using Graphs;

Console.OutputEncoding = Encoding.UTF8;
var graph = new Graph();
graph.AddEdge("Москва", "Тверь", 180);
graph.AddEdge("Москва", "Тула", 190);
graph.AddEdge("Тверь", "Псков", 600);
graph.AddEdge("Тула", "Калуга", 110);
graph.AddEdge("Калуга", "Псков", 450);

Console.WriteLine("Граф дорог между городами. Расстояния условные.\n");
GraphPrinter.Print(graph);
var route = graph.FindShortestPath("Москва", "Псков");
Console.WriteLine("\nКратчайший маршрут из Москвы в Псков по алгоритму Дейкстры:");
if (route is null)
    Console.WriteLine("Маршрут не найден.");
else
{
    Console.WriteLine(string.Join(" -> ", route.Vertices));
    Console.WriteLine($"Общая протяжённость: {route.DistanceKm} км");
}

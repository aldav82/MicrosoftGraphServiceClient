// See https://aka.ms/new-console-template for more information
using MicrosoftGraph;

Console.WriteLine("Hello, World!");
Console.WriteLine("Azure Graph Client creation.....");
var client = MicrosoftGraph.Graph.GenerateClient();
Console.WriteLine("Azure Graph Client created.....");

string userName = "SOME_USER";
string groupID = "SOME_GROUP_ID";

var belongs = await client.UserBelongsToGroup(userName, groupID );

if (belongs)
{
    Console.WriteLine($"User {userName} belongs to {groupID}");
} else
{
    Console.WriteLine($"User {userName} does not belong to {groupID}");
}

Console.ReadLine();






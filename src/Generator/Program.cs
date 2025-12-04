using Proxoft.Heatmaps.Generator.Commands;

// See https://aka.ms/new-console-template for more information
bool close = false;
string?[] verb = args;

while (!close)
{
    close = Parser.Default
        .ParseArguments<GenerateCommand, QuitCommand>(verb)
        .MapResult(
            (GenerateCommand c) => {
                return !c.Continue;
            },
            (QuitCommand q) => true,
            err => {
                return false;
            }
        );

    if (!close)
    {
        verb = Console.ReadLine()?.Split(" ") ?? [];
    }
}
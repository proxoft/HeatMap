using Proxoft.Heatmaps.Generator.Commands;
using Proxoft.Heatmaps.Generator.Handlers;

bool close = false;
string?[] verb = args;

while (!close)
{
    close = Parser.Default
        .ParseArguments<GenerateCommand, QuitCommand>(verb)
        .MapResult(
            (GenerateCommand c) => {
                Option<GenerateError> err = c.CreateHeatmap()
                    .Do(
                        error => Console.WriteLine($"Error: {error}")
                    )
                    ;

                if(err is None<GenerateError>)
                {
                    Console.WriteLine("Done");
                }

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
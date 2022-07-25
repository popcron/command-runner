# Command Runner
 A library that gives you the tools to parse user text to run commands!
 
## Installation
Add this url to your packages manifest:
```json
https://github.com/popcron-games/command-runner.git
```

## How to use
### Using the in game command runner
If the command runner sample is imported, you get a pretty basic in game command runner with these features:
1. Toggle it open using the `~` key (the one under escape).
2. Can select previous commands using the up arrow key
3. Showing the result of the command
### Adding a method as a command
```cs
ICommand command = new MethodCommand("yo", DoSomething);
Library.Singleton.Add(command);

private void DoSomething()
{
    
}
```
If it's a static method, then the `[Command]` attribute can be used.
```cs
[Command("yo")]
private static void DoSomething()
{

}
```
### Enumerate through and retrieve all commands
```cs
foreach (IBaseCommand prefab in Library.Singleton.Prefabs)
{
    Debug.Log($"{prefab.Path}");
    if (prefab is IDescription desc)
    {
        Debug.Log($"    {desc.Description}");
    }
}

Library.Singleton.GetPrefab("command");
```
### Create a brand new command runner
```cs
IParser parser = new ClassicParser();
ILibrary library = new Library(CommandFinder.FindAllCommands());
ICommandRunner runner = new CommandRunner(library, parser);

ICommand customCommand = new MethodCommand("hey", () => "hello world");
library.Add(customCommand);
runner.Run("hey");
```

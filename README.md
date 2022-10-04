# Command Runner
 A library that gives you the tools to parse user text to run commands!
 
## Quick installation
1. Add this url to your packages manifest:
```json
https://github.com/popcron-games/command-runner.git
```
2. Import the in game console and command loader samples into your project:
![image](https://user-images.githubusercontent.com/23342532/193724912-9387fc29-6415-42fb-838d-d40682175d58.png)

*more information about this is in the [In game console](#in-game-console) section*

3. Drag and drop the `In Game Console` prefab into your scene
4. Done

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

### In game console
Some basic features:
1. Toggle it open using the `~` key (the key under `esc`).
2. Can select previous commands using the up arrow key
3. Shows the Debug.Log output from the command

### Enumerate through and retrieve all commands
```cs
foreach (IBaseCommand prefab in Library.Singleton.Prefabs)
{
    Debug.Log(prefab.Path);
    if (prefab is IDescription desc)
    {
        Debug.LogFormat("    {0}", desc.Description);
    }
}

ICommand command = Library.Singleton.GetPrefab("ls commands");
```
### Create a brand new command runner
```cs
IParser parser = new ClassicParser();
ILibrary library = new Library(CommandFinder.FindAllCommands());
ICommandRunner runner = new CommandRunner(library, parser);

ICommand customCommand = new MethodCommand("hey", () => Debug.Log("hello world"));
library.Add(customCommand);

runner.Run("hey");
```

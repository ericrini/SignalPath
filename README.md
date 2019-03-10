# SignalPath Code Sample
Convert a string of hexadecimal bytes into a base64 string. The interesting source code is in [Convert.cs](https://github.com/ericrini/SignalPath/blob/master/Source/Convert.cs).

# Run
Install a [.NET Core](https://github.com/dotnet/core) SDK and run the following from the project root.

```console
> dotnet build
> dotnet test .\Test
> dotnet run --project .\Source --configuration Release -- 45766964696e74
```

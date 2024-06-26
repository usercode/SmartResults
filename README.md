# SmartResults
Lightweight .NET library to use result pattern instead of throwing exceptions.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/vpre/SmartResults.svg)](https://www.nuget.org/packages/SmartResults/)

## How to use it
- `Result` and `Result<T>` are structs to prevent memory allocation
- Support for JSON
  - Integrated JsonConverter for `Result` and `Result<T>`
  - Polymorphic `Error` objects
- Extensions for HttpResponseMessage
- Chaining flow
- Exception flow

### Create and use results
```csharp
Result result = Result.Ok();

if (result.IsSucceeded)
{
   Console.WriteLine("Succeeded");
}
else
{
   Console.WriteLine(result.Error);
}
```

### Create and use results with value
```csharp
Result<int> result = Result.Ok(100);

if (result.IsSucceeded)
{
   Console.WriteLine(result.Value);
}
else
{
   Console.WriteLine(result.Error);
}
```

### Create and use failed results
```csharp
Result<int> result = Result.Failed("Something is wrong!");
Result<int> result = Result.Failed(new Error(..));

if (result.IsFailed)
{
   Console.WriteLine(result.Error);
}

```

### Use match to evaluate the result
```csharp
Result<int> result = Result.Ok(100);

bool b = result.Match(value => true, error => false);
```
### Explicit conversion

```csharp
Result<string> result = Result.Ok(100).ToResult<string>(x => x.ToString());
```

### Implicit operator for value

```csharp
Result<int> result = 100;

int value = result;
```

### Exception flow by implicit operator
```csharp
int value = Create(); //throws exception if result is failed
```

### Chaining flow

```csharp
Result result1 = Create().Then(x => Console.WriteLine(x));

Result result2 = await Create().ThenAsync(async x => await Service.ExecuteAsync(x));

Result result3 = await CreateAsync().ThenAsync(x => Console.WriteLine(x));

Result result4 = await CreateAsync().ThenAsync(async x => await Service.ExecuteAsync(x));
```

### HttpClient extensions

```csharp
Result result1 = await httpClient.GetAsync("/").ReadResultFromJsonAsync();

Result<int> result2 = await httpClient.GetAsync("/").ReadResultFromJsonAsync<int>();
```

### Supports polymorphic error objects for JSON

```csharp
public class PermissionError : IError { }

Error.Register<PermissionError>();

Result result = Result.Failed(new PermissionError("Error"));

string json = JsonSerializer.Serialize(result);

Result result2 = JsonSerializer.Deserialize<Result>(json);

result2.Error //PermissionError
```

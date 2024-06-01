# SmartResults
Lightweight .NET library to use result pattern instead of throwing exceptions.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/vpre/SmartResults.svg)](https://www.nuget.org/packages/SmartResults/)

## How to use it
- Result and Result{T} are structs to prevent memory allocation
- Integrated JsonConverter
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
Result<int> result = Result.Failed(new Exception());

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

### Implicit conversion

```csharp
Result<int> result = 100;

int value = result;
```

### Use exception flow by implicit operator
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

### Use HttpClient extensions

```csharp
Result result1 = await httpClient.GetAsync("/").ReadResultFromJsonAsync();

Result<int> result2 = await httpClient.GetAsync("/").ReadResultFromJsonAsync<int>();
```

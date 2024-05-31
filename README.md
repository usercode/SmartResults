# SmartResults

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## How to use it
- Result and Result{T} are structs to prevent memory allocation
- Integrated JsonSerializerConverter for Result and Result<T>
- Chaining flow
- Exception flow

### Create and use results
```csharp
Result result = Result.Ok();

if (result.IsSucceeded)
{
   Console.WriteLine("Completed");
}
else
{
   Console.WriteLine(result.Error.Message);
}
```

### Create and use results with value
```csharp
Result<int> result = Result.Ok(100);

if (result.IsSucceeded)
{
   Console.WriteLine(result.Value);
}
```

### Create and use failed results
```csharp
Result<int> result = Result.Failed("Something is wrong!");
Result<int> result = Result.Failed(new Error(..));
Result<int> result = Result.Failed(new Exception());

if (result.IsFailed)
{
	Console.WriteLine(result.Error.Message);
}

```

### Use match to evaluate the result
```csharp
Result<int> r1 = Result.Ok(100);

bool i = r1.Match(value => true, error => false);
```

### Implicit conversion

```csharp
Result<int> value = 100;

int number = value;
```

### Use exception flow by implicit operator
```csharp
int value = Create(); //throw exception if result is failed
```


### Chaining flow

```csharp
Result result1 = Create().Then(x => Console.WriteLine(x));

Result result2 = await Create().ThenAsync(async x => await Service.ExecuteAsync(x));

Result result3 = await CreateAsync().ThenAsync(x => Console.WriteLine(x));

Result result4 = await CreateAsync().ThenAsync(async x => await Service.ExecuteAsync(x));
```


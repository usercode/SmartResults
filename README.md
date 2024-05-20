# SmartResults

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## How to use it
- Result and Result{T} are structs to prevent memory allocation
- Chaining flow
- Exception flow

### Create and use results
```csharp
Result r1 = Result.Ok();
Result failed1 = Result.Failed("Error");
Result failed2 = Result.Failed(new Exception());
Result failed3 = Result.Failed(new Error());

Result<int> value = Result.Ok(100);

if (r1.IsSucceeded)
{
   Console.WriteLine(r1.Value);
}

if (r1.IsFailed)
{
   Console.WriteLine(r1.Error.Message);
}
```

### Use match to evaluate the result
```csharp
Result<int> r1 = Result.Ok(100);

int i = r1.Match(value => 1, error => 0);
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

Result result2 = Create().ThenAsync(async x => await Service.ExecuteAsync(x));

Result result3 = await CreateAsync().ThenAsync(x => Console.WriteLine(x));

Result result4 = await CreateAsync().ThenAsync(async x => await Service.ExecuteAsync(x));
```


# CoroutineChainer

[Asset Package](/release/CoroutineChainer.unitypackage)

## Intro
Coroutine chain management, easry to read and able to chain coroutines to run on the fly or run later.

## Usage
### Basic
Create and Run a chain on the fly.
```csharp
void foo()
{
//A, B, C are IEnumerators
    CoroutineChainer.Start()
      .ChainWith(Coroutine())
      .Sequential(A(),B(),C()) // play one by one.
      .Parallel(A(),B(),C()) // play same time.
      .Log("Complete!");
      .Call(()=>Callback())
      .RunCoroutine();
}

```

Create a chain and run it later
```csharp
ChainBase coroutine;
void foo()
{
  coroutine = CoroutineChainer.Start()
                              .ChainWith(A())
                              .ChainWith(B());
}
void foo2()
{
  if(someCondition)
    coroutine?.RunCoroutine();
}
```

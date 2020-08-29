# How to use

**1. Use Modeles to configure autofac container builder**

```csharp
public class MyModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<B>().SingleInstance();
        builder.RegisterType<A>().SingleInstance();
    }
}
```

```csharp
public class Startup
{
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule<MyModule>();
    }
}
```

**2. Create a unit test to check dependency**

```csharp
public class TestAutofacDependency
{
    [Test]
    public void TestResolveAll()
    {
        Assert.DoesNotThrow(() => 
        {
            ScopeExtensions.TestAutofacDependency(new IModule[]
            {
                new MyModule(), 
            });        
        });
        
    }
}
```


### Check out my more recent ideas on repositories on my blog:

 - [Composable repositories](http://blog.staticvoid.co.nz/2015/composable_repositories/)

---

## A repository pattern implementation for Code First. 

The compile library is avaliable on NuGet via 
	Install-Package StaticVoid.Repository
or 
	http://nuget.org/packages/StaticVoid.Repository

More information about how to use this solution and what the package does can be found on my blog here 
(http://blog.staticvoid.co.nz/2011/10/staticvoid-repository-pattern-nuget.html) 

This is intended for usage with an IoC repository injection pattern. To bind to the repository in Ninject simply use: 
```csharp
kernel.Bind(typeof(IRepositoryDataSource<>)).To(typeof(DbContextRepositoryDataSource<>)); 
kernel.Bind(typeof(IRepository<>)).To(typeof(SimpleRepository<>)); 
```

this will allow you to inject repositories using the form `IRepository<T>` in your service implementations. 
Repository will have basic methods implemented for the type to allow crud.


Licensed for usage under LGPL, http://www.opensource.org/licenses/lgpl-3.0.html

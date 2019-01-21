# Pipeline Framework
[![Build status](https://dev.azure.com/gtmoose/Mathis%20Home/_apis/build/status/Pipeline%20Framework/Pipeline%20Framework%20-%20CI)](https://dev.azure.com/gtmoose/Mathis%20Home/_build/latest?definitionId=8) [![nuget](https://img.shields.io/nuget/v/PipelineFramework.Core.svg)](https://www.nuget.org/packages/PipelineFramework.Core/)

## What is it?

Pipeline Framework allows you to easily construct and execute linear workflows using a set of configurable components.  Using the framework to compose execution pipelines out of components promotes using [single responsiblity principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) while building code that is highly testable.

For more information please visit the [wiki](https://github.com/gtmoose32/pipeline-framework/wiki).

If you are looking for Dependency Injection (IOC) container extensions project, they have been moved to their own repository, [Pipeline Framework Dependency Injection](https://github.com/gtmoose32/pipeline-framework-di).

## Installing Pipeline Framework
You should install [Pipeline Framework with NuGet](https://www.nuget.org/packages/PipelineFramework.Core/):

```
Install-Package PipelineFramework.Core
```

or via the .NET Core command line interface:

```
dotnet add package PipelineFramework.Core
```

Either commands, from Package Manager Console or .NET Core CLI, will download and install PipelineFramework.Core and all required dependencies.

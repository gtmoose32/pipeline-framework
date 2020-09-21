# Pipeline Framework
[![Build status](https://dev.azure.com/gtmoose/Mathis%20Home/_apis/build/status/Pipeline%20Framework/Pipeline%20Framework%20-%20CI)](https://dev.azure.com/gtmoose/Mathis%20Home/_build/latest?definitionId=8) [![nuget](https://img.shields.io/nuget/v/PipelineFramework.Core.svg)](https://www.nuget.org/packages/PipelineFramework.Core/)

## What is it?

Pipeline Framework allows you to easily construct and execute linear workflows using a set of configurable components.  Using the framework to compose execution pipelines out of components promotes using [single responsiblity principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) while building code that is highly testable.

For more information please visit the [wiki](https://github.com/gtmoose32/pipeline-framework/wiki).

## Installing Pipeline Framework
```
dotnet add package PipelineFramework.Core
```

## PipelineFramework.Extensions.Microsoft.DependencyInjection

[![nuget](https://img.shields.io/nuget/v/PipelineFramework.Extensions.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/PipelineFramework.Extensions.Microsoft.DependencyInjection/)

Support for Microsoft Dependency Injection has been added to Pipeline Framework! Install the package using the dotnet cli command below.

```
dotnet add package PipelineFramework.Extensions.Microsoft.DependencyInjection
```



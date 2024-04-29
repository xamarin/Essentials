# Xamarin.Essentials

Xamarin.Essentials gives developers essential cross-platform APIs for their mobile applications. 

iOS, Android, and UWP offer unique operating system and platform APIs that developers have access to, all in C# leveraging Xamarin. It is great that developers have 100% API access in C# with Xamarin, but these APIs are different per platform. This means developers have to learn three different APIs to access platform-specific features. With Xamarin.Essentials, developers have a single cross-platform API that works with any iOS, Android, or UWP application that can be accessed from shared code no matter how the user interface is created.

# Support

Support for Xamarin.Essentials ended on **May 1, 2024** as per the [Xamarin Support Policy][xamarin-support-policy]:

> Xamarin support ended on May 1, 2024 for all Xamarin SDKs including Xamarin.Forms.

Xamarin.Forms was succeeded by .NET MAUI (which includes Essentials), in May 2022 as part of .NET 6, and is currently supported as described on the [.NET MAUI Support Policy][maui-support-policy]. Follow the [official upgrade guidance](https://learn.microsoft.com/dotnet/maui/migration) to bring your Xamarin applications to the latest version of .NET.

To all our developers and contributors, thank you so much for being a part of our Xamarin community. We'll see you all over in [.NET MAUI][dotnet-maui-repo]!

[maui-support-policy]: https://dotnet.microsoft.com/platform/support/policy/maui
[xamarin-support-policy]: https://dotnet.microsoft.com/platform/support/policy/xamarin
[dotnet-maui-repo]:https://github.com/dotnet/maui/

## Questions

Get your technical questions answered by experts on [Microsoft Q&A](https://learn.microsoft.com/answers/topics/dotnet-xamarinessentials.html?WT.mc_id=friends-0000-jamont).

## Contribution Discussion

Contributing to Xamarin.Essentials? Join our [Discord server](https://discord.com/invite/Y8828kE) and chat with the team

## Build Status

| Build Server | Type         | Platform | Status                                                                                                                                                                                 |
|--------------|--------------|----------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Azure DevOps         | Build        | Windows  | [![Build Status](https://dev.azure.com/xamarin/public/_apis/build/status/xamarin/Essentials/Xamarin.Essentials%20(Public)?repoName=xamarin%2FEssentials&branchName=main)](https://dev.azure.com/xamarin/public/_build/latest?definitionId=7&repoName=xamarin%2FEssentials&branchName=main) |

## Installation

Xamarin.Essentials is available via NuGet & is included in every template:

* NuGet Official Releases: [![NuGet](https://img.shields.io/nuget/vpre/Xamarin.Essentials.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Essentials)
* Nightly / CI Releases: https://aka.ms/xamarin-essentials-ci/index.json

Please read our [Getting Started with Xamarin.Essentials guide](https://learn.microsoft.com/xamarin/essentials/get-started?WT.mc_id=friends-0000-jamont) for full setup instructions.

## Xamarin.Essentials with .NET MAUI and iOS/Android .NET projects

Starting in .NET 6, Xamarin.Essentials was added into .NET MAUI directly. The team has worked hard to ensure that while it comes pre-configured with every .NET MAUI application, it is still available to all iOS and Android apps built with .NET. If you are building a .NET MAUI project there is nothing more for you to do, just start using the APIs. If you are migrating a Xamarin.iOS or Xamarin.Android app and want to use the Essentials APIs, then add the following into your project's csproj file:

```xml
<PropertyGroup>
  <UseMauiEssentials>true</UseMauiEssentials>
</PropertyGroup>
```

Once you update to .NET MAUI Essentials, you will need to update any `using Xamarin.Essentials;` using statements (and any other namespace references) to the new .NET MAUI Essentials namespaces, which you can find in the [documentation](https://learn.microsoft.com/dotnet/maui/platform-integration).

## Documentation

Browse our [full documentation for Xamarin.Essentials](https://learn.microsoft.com/xamarin/essentials?WT.mc_id=friends-0000-jamont), including feature guides, on how to use each feature.

## Supported Platforms

Platform support & feature support can be found on our [documentation](https://learn.microsoft.com/xamarin/essentials/platform-feature-support?WT.mc_id=friends-0000-jamont)


## Contributing

Please read through our [Contribution Guide](CONTRIBUTING.md). We are not accepting new PRs for full features, however any [issue that is marked as `up for grabs`](https://github.com/xamarin/Essentials/issues?q=is%3Aissue+is%3Aopen+label%3A%22up+for+grabs%22) are open for community contributions. We encourage creating new issues for bugs found during usage that the team will triage. Additionally, we are open for code refactoring suggestions in PRs.

## Building Xamarin.Essentials

Xamarin.Essentials is built with the new SDK-style projects with multi-targeting enabled. This means that all code for iOS, Android, and UWP exist inside of the Xamarin.Essentials project.

## Visual Studio

A minimum version of Visual Studio 2019 16.3 or Visual Studio for Mac 2019 8.3 are required to build and compile Xamarin.Essentials.

### Workloads needed:

* Xamarin
* .NET Core
* UWP

### You will need the following SDKs

* Android 10.0, 9.0, 8.1, 8.0, 7.1, 7.0, & 6.0 SDK Installed
* UWP 10.0.16299 SDK Installed

Your can run the included `android-setup.ps1` script in **Administrator Mode** and it will automatically setup your Android environment.

## FAQ

Here are some frequently asked questions about Xamarin.Essentials, but be sure to read our full [FAQ on our Wiki](https://github.com/xamarin/Essentials/wiki#feature-faq).

## License

Please see the [License](LICENSE).

## Stats
<img src="https://repobeats.axiom.co/api/embed/f917a77cbbdeee19b87fa1f2f932895d1df18b31.svg" />

# Smittestop.Mobile
Danish mobile application for Covid-19 spread tracking

<h1 align="center"> Smittestop Mobile Application <br/><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="INDSÆTSMITTESTOPLOGO"> </h1>
<br/>

[![GitHub last commit](https://img.shields.io/github/last-commit/folkehelseinstituttet/Fhi.Smittestopp.App)](https://github.com/Sundhedsdatastyrelsen/Smittestop.Mobile/commits)
[![Open pull requests](https://img.shields.io/github/issues-pr/folkehelseinstituttet/Fhi.Smittestopp.App)](https://github.com/Sundhedsdatastyrelsen/Smittestop.Mobile/pulls)
[![Open issues](https://img.shields.io/github/issues/folkehelseinstituttet/Fhi.Smittestopp.App)](https://github.com/Sundhedsdatastyrelsen/Smittestop.Mobile/issues)

The goal of this open-source project is to develop and maintain the official smitte|stop mobile app in Denmark based on the Exposure Notification API from [Apple](https://www.apple.com/covid19/contacttracing/) and [Google](https://www.google.com/covid19/exposurenotifications/). This repository contains the code for frontend mobile application (iOS and Android). The app uses Xamarin cross-platform solution for Exposure Notification: [Nuget](https://www.nuget.org/packages/Xamarin.ExposureNotification) and [Source](https://github.com/xamarin/XamarinComponents/tree/master/XPlat/ExposureNotification).

If you are interested in backend server implementation, check out https://github.com/Sundhedsdatastyrelsen/Smittestop.Backend.

## Documentation
Documentation is available on GitHub [here](https://github.com/folkehelseinstituttet/Fhi.Smittestopp.Documentation).

Common questions as well as general information about smitte|stop is available on [smittestop.dk](https://www.smittestop.dk/) (Danish) and [smittestop.en]( https://www.smittestop.dk/en/) (English) webpages.

## Azure Pipelines status (build and test)

|    Branch    | Status  |
|--------|---|
| master | [![Build Status](https://fhi.visualstudio.com/Fhi.Smittestopp/_apis/build/status/folkehelseinstituttet.Fhi.Smittestopp.App?branchName=master)](https://fhi.visualstudio.com/Fhi.Smittestopp/_build/latest?definitionId=268&branchName=master)  |
| dev    | [![Build Status](https://fhi.visualstudio.com/Fhi.Smittestopp/_apis/build/status/folkehelseinstituttet.Fhi.Smittestopp.App?branchName=dev)](https://fhi.visualstudio.com/Fhi.Smittestopp/_build/latest?definitionId=268&branchName=dev)  |
### App Center builds
|            | iOS | Android |
|------------|-----|---------|
| Production |  [![Build status](https://build.appcenter.ms/v0.1/apps/cad9c263-729f-482a-9eab-8b32d3510909/branches/master/badge)](https://appcenter.ms)   |    [![Build status](https://build.appcenter.ms/v0.1/apps/a78a302f-01af-4280-a409-d0d6975e726b/branches/master/badge)](https://appcenter.ms)     |
| PrePro |  [![Build status](https://build.appcenter.ms/v0.1/apps/e4af3bb5-44d0-47ae-9f80-714f44f231ab/branches/master/badge)](https://appcenter.ms)   |    [![Build status](https://build.appcenter.ms/v0.1/apps/ff50860e-7ced-40ce-87cd-04e3f9e5ff8a/branches/master/badge)](https://appcenter.ms)     |
| Test |  [![Build status](https://build.appcenter.ms/v0.1/apps/e4af3bb5-44d0-47ae-9f80-714f44f231ab/branches/master/badge)](https://appcenter.ms)   |    [![Build status](https://build.appcenter.ms/v0.1/apps/ff50860e-7ced-40ce-87cd-04e3f9e5ff8a/branches/master/badge)](https://appcenter.ms)     |

## Development
### Prerequisites
- Visual Studio 2019
- Xcode 12 or higher (iOS only)

### Getting started
1. Clone this repository using `git clone https://github.com/Sundhedsdatastyrelsen/Smittestop.Mobile.git` 
2. Open the solution file `NDB.Covid19.sln` in Visual Studio
3. Restore Nuget Packages
4. Build the project and run it.

### Project structure
The app is written in Xamarin (C#) and platform specific UI implementation (Android XML and UIStoryboards) for additional flexibility when working with UI.
Overall, the solution contains four projects:
- **NDB.Covid19:** Contains shared business logic between iOS and Android, i.e., Exposure Notifications handler, locales, log utils, models, viewModels, services.<br/><br/>
- **NDB.Covid19.Droid:** Android related code, UI Activities/Fragments, implementation of services and handlers (for Dependency Injection) etc.<br/><br/>
- **NDB.Covid19.iOS:** iOS related code, UI Storyboards/ViewControllers, implementation of services and handlers (for Dependency Injection) etc.<br/><br/>
- **NDB.Covid19.Test:** Unit and integration tests.

## Contributing
Feedback and contribution are always welcome. For more information about how to contribute, refer to [Contribution Guidelines](CONTRIBUTING.md). By contributing to this project, you also agree to abide by its [Code of Conduct](CODE_OF_CONDUCT.md) at all times.

## Download smitte|stop
<a href="https://play.google.com/store/apps/details?id=com.netcompany.smittestop_exposure_notification"><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.helsenorge.no/globalassets/mobilapp/badges/google-play-badge-en.png" width="200" height="60" alt="Get it on Google Play"></a>
<a href=" https://apps.apple.com/dk/app/smitte-stop/id1516581736?l=da"><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.helsenorge.no/globalassets/mobilapp/badges/apple-app-store-badge-en.png" width="180" height="60" alt="Download on the App Store"></a>


## Licence
Copyright (c) 2020 Agency for Digitisation (Denmark), 2021 Danish Agency For Digitisation (Denmark), 2021 Netcompany Group AS

Smitte|stop is Open Source software released under the [MIT license](LICENSE.md)

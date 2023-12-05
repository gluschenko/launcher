## Launcher

My launcher application for **deployment and managing versions** of Windows client applications.

Requirements: 
* .NET 6.0 LTS
* Windows 7 SP1 / Windows 8.1 / Windows 10
* WPF

## Configuration

You can customize the **logo**, **background** and **title** of the main window. All these parameters are included in Launcher.json. Icon is not configurable from json config. 

#### Launcher.json: 

```
{
    "Title": "My Launcher",
    "Logo": "Logo.png",
    "Background": "Background.png",
    "VersionsURL": "https://domain.com/version.json",
    "BuildExecutable": "Bootstrap.exe"
}
```


* `Title` - Title of main window
* `Logo` - Logo picture relative path (.png)
* `Background` - Background picture relative path (.png)
* `VersionsURL` - Meta data about latest versions (example is below)
* `BuildExecutable` - Main executable file (must be in each version folder)

#### Web response example (version.json):

```
{
    "Version": "2.3",
    "URL": "https://domain.com/prod/2.3.zip",
    "InstallerURL": "https://domain.com/prod/Setup_2.3.exe",
    "WindowsVersions": [
        "2.3",
        "2.2",
        "2.1"
    ],
    "WindowsBuilds": [
        "https://domain.com/prod/Setup_2.3.exe",
        "https://domain.com/prod/Setup_2.2.exe",
        "https://domain.com/prod/Setup_2.1.exe"
    ]
}
```

* `Version` - Latest version
* `URL` - [LEGACY FIELD]
* `InstallerURL` - Latest vesrion installer
* `WindowsVersions` - List of available versions (displays on downloads page)
* `WindowsBuilds` - List of available installers (corresponds to the previous field)

## Catalog structure

* Versions
  * 2.1
  * 2.2
  * 2.3
* Downloads
  * [...]
  * Launcher.exe
  * Launcher.Core.dll
  * Launcher.json
  * LauncherPrefs.json


## Showcase

![](/.media/screen_1.png)
![](/.media/screen_2.png)

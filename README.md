## Launcher

My launcher application for **deployment and managing versions** of Windows client applications.

Requirements: 
* .NET Framework >= 4.5
* WPF

## Configuration

You can customize the **logo**, **background** and **title** of the main window. All these parameters are included in Launcher.json. Icon is not configurable from json config. 

#### Launcher.json: 

    {
        "Title": "My Launcher",
        "Logo": "Logo.png",
        "Background": "Background.png",
        "VersionsURL": "https://domain.com/version.json",
        "BuildExecutable": "Bootstrap.exe"
    }


`Title` - Title of main window

`Logo` - Logo picture relative path (.png)

`Background` - Background picture relative path (.png)

`VersionsURL` - Meta data on latest versions (example is below)

`BuildExecutable` - Main executable file (must be in each version folder)

#### Web response example:

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

`Version` - Latest version

`URL` - [LEGACY FIELD]

`InstallerURL` - Latest vesrion installer

`WindowsVersions` - List of available versions (displays on downloads page)

`WindowsBuilds` - List of available installers (corresponds to the previous field)


## Showcase

![](/Media/screen_1.png)
![](/Media/screen_2.png)

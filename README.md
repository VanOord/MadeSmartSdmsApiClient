# MadeSmart SDMS Api Client

## Generate Client Code (MsgSdmsApiClient project) 
The code in the SdmsClient.cs class is generated using NSwag Studio that can be downloaded from the following location:

https://github.com/RicoSuter/NSwag/wiki/NSwagStudio

Depending on the target environment use the following config file in NSwag Studio to (re)generate the code for the SdmsClient:

* sdms_api.nswag (live)
* sdms_api_stage.nswag (stage)

After generating the client code using the [Generate Outputs] button, copy the client code into the SdmsClient.cs file. 
Refactor the code by renaming the [*Entity Name*]DTO classes to Published[*Entity Name*].

E.g. ChartDTO => PublishedChart

## Publish Nuget Package (Visual Studio)

The package info for the Nuget package are stored in the properties of the MadeSmartSdmsApiClient project. This info (like version number) can be updated via the properties of said project on the General page of the Package section or by updating the csproj file.
Select the Release configuration and then create the Nuget package using the Pack option from the shortcut menu that is presented by right-clicking the project.

After the package has been created, navigate to the output folder that holds the new Nuget package and open e.g a Windows Terminal window from that location. 
Publish the package into to the Inhouse_Surdev Nuget folder using the following command:

```powershell
nuget add MadeSmartSdmsApiClient.[Nuget package version].nupkg -source "R:\SUR\03 Inhouse_Surdev\VOSS.NET\Build Source Files\NuGet"
```

where [Nuget package version] = version number of the generated Nuget package, e.g. 0.3.0

The R drive is present on most dev machines. It maps to the following location on the VO network: \\\vanoord.org\groups\
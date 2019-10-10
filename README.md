# RazorClassLibraryExample

> cd WebApplicationRCLExample

###Run the fowing in console

> dotnet build
> dotnet run

Then access: https://localhost:5001/test 

Result is an HTML manualy rendeded for RCL

###When running on server, *refs* folder must be included in build along with the views
> dotnet publish -o my-api

Results a folder structure of folders with ***areas*** and ***refs***

Coding wise, is just dumb code to prove the functinality, RazorLight used to compile the view for the sake of example.

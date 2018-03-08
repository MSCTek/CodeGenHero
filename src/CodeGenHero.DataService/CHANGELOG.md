## Changelog

### 1.0.14
Bug Fixes & Enhancements:
* Minor change to BaseModel making default UNKNOWNLOGMESSAGETYPE constant and HandleLazyLoadRequest not async.

### 1.0.13
Bug Fixes & Enhancements:
* Consolidated CodeGenHero.DataService and CodeGenHero.DataService.Core projects.
* Incorporated support for BaseModel to support a generic IDataService.

### 1.0.11
Bug Fixes & Enhancements:
* Undo signed assembly via strong name key file.

### 1.0.10
New Features:
* Signed assembly via strong name key file.

### 1.0.9
Bug Fixes & Enhancements:
* Updated dependency reference for CodeGenHero.DataService.Core project.

### 1.0.8
Bug Fixes & Enhancements:
* Migrated files that are needed outside the dataservice from this project to the CodeGenHero.DataService.Core project.


### 1.0.0
Breaking Changes:
* Now .NET Standard Library

New Features:
* Added NuGet package creation upon build

Bug Fixes & Enhancements:
* Additional interfaces to enable functional replacement and facilitate test creation

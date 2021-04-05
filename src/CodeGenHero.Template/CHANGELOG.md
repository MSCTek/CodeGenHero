## Changelog

### 1.3.9
* BaseGenerator.cs - Added GetForeignKeyName(), removed IsEntityInExcludedReferenceNavigionationProperties(), added EntityNavigationsContainsNavigationName() to replace IsEntityInExcludedReferenceNavigionationProperties() in CodeGenHero.Template

### 1.3.5
* Updated CodeGenHero.Core reference to get access to new GetExcludedEntityNavigationsByRegEx() method that returns a list of excluded navigation properties based upon RegEx Exclude/Include strings.

### 1.3.0
* Added several classes to support open sourcing the Blazor, CSLA, and WebAPI FullFramework templates.

### 1.0.2
Bug Fixes & Enhancements:
* Added HiddenIndicator support.

### 1.0.1
New Features:
* Signed assembly via strong name key file.

### 1.0.0
Breaking Changes:
* Now .NET Standard Library

New Features:
* Added NuGet package creation upon build

Bug Fixes & Enhancements:
* Additional interfaces to enable functional replacement and facilitate test creation

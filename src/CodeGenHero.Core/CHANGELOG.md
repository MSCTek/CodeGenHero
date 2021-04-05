## Changelog

### 1.3.2
* Added GetExcludedEntityNavigationsByRegEx() to IModel / Model class

### 1.3.1
* Added String extension for Replace() that uses StringComparison

### 1.3.0
* Adjustments to support open sourcing the Blazor, CSLA, and WebAPI FullFramework templates.

### 1.1.27
Changes:
* Swapping out Metadatasource constants, adding ContextName, ContextNamespace, OutputCghmFilePath, OutputModelFilePath

### 1.1.26
Enhancements:
* Added IModel convenience methods to assist in filtering by regular expressions: GetEntityTypesByRegEx, GetEntityTypesFilteredByRegEx,  and GetEntityTypesMatchingRegEx.

### 1.1.21
Bug Fixes:
* Added FILEEXTENSION_ExcludedNavigationProperties value of NPE to constants.

### 1.1.20
Bug Fixes:
* Adding ModelTypeName and ModelFullyQualifiedTypeName to the IModel interface (i.e. for recording the DbContext class name).

### 1.1.19
Bug Fixes:
* Implementation of missing methods on EntityType.

### 1.1.18
Bug Fixes:
* Made the IModel/Model class serializable and changed the return type for GetEntityTypes to IList.

### 1.1.14
Bug Fixes:
* Change the Type values in PropertyBase, TypeBase, INavigation, and EntityType to use ClrType with simple string properties to avoid Json deserialization error.

### 1.0.0
New Features:
* Migrated a number of classes from MSC Libraries to Open Source Libraries.

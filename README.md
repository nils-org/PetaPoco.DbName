# PetaPoco.DbName

[![standard-readme compliant][]][standard-readme]
[![Contributor Covenant][contrib-covenantimg]][contrib-covenant]
[![Build][githubimage]][githubbuild]
[![Codecov Report][codecovimage]][codecov]
[![NuGet package][nugetimage]][nuget]

Provides access to table and colum names for PetaPoco

## Table of Contents

- [Install](#install)
- [Usage](#usage)
- [Maintainer](#maintainer)
- [Contributing](#contributing)
- [License](#license)

## Install

```ps
Install-Package PetaPoco.DbName
```

## Usage

This package adds two extension methods to `IDatabase`:

### GetTableName

Returns the Table name in the Database.

```
database.GetTableName<TestTable>();
```

### GetColumnName

Returns the name of a colum in the Database.

```
database.GetColumnName<TestTable>(x => x.Property);
```

## Maintainer

[Nils Andresen @nils-a][maintainer]

## Contributing

PetaPoco.DbName follows the [Contributor Covenant][contrib-covenant] Code of Conduct.

We accept Pull Requests.

Small note: If editing the Readme, please conform to the [standard-readme][] specification.

## License

[MIT License © Nils Andresen, Jürgen Rosenthal-Buroh][license]

[githubbuild]: https://github.com/nils-org/PetaPoco.DbName/actions/workflows/build.yaml?query=branch%3Adevelop
[githubimage]: https://github.com/nils-org/PetaPoco.DbName/actions/workflows/build.yaml/badge.svg?branch=develop
[codecov]: https://codecov.io/gh/nils-org/PetaPoco.DbName
[codecovimage]: https://img.shields.io/codecov/c/github/nils-org/PetaPoco.DbName.svg?logo=codecov&style=flat-square
[contrib-covenant]: https://www.contributor-covenant.org/version/2/0/code_of_conduct/
[contrib-covenantimg]: https://img.shields.io/badge/Contributor%20Covenant-v2.0%20adopted-ff69b4.svg
[maintainer]: https://github.com/nils-a
[nuget]: https://nuget.org/packages/PetaPoco.DbName
[nugetimage]: https://img.shields.io/nuget/v/PetaPoco.DbName.svg?logo=nuget&style=flat-square
[license]: LICENSE.txt
[standard-readme]: https://github.com/RichardLitt/standard-readme
[standard-readme compliant]: https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square
[documentation]: https://nils-org.github.io/PetaPoco.DbName/
[api]: https://cakebuild.net/api/Cake.SevenZip/
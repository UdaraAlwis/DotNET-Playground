# EFCoreVsDapperDemoApp

Playing around with EntityFramework and Dapper following the video by DataVids:
https://www.youtube.com/watch?v=w636jHpAcTs&ab_channel=DataVids

### EntityFramework Stuff:

Nugets for EF based implementation:
- ```Microsoft.EntityFrameworkCore.SqlServer```
- ```Microsoft.EntityFrameworkCore.Tools```

Command for generating Model classes in the Solution for the local NorthWind DB
```
Scaffold-DbContext "Server=.\SQLExpress;Database=NorthWind;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

###  Dappery Stuff:

Nugets for Dapper based implementation:
- ```Dapper```
- ```System.Data.SqlClient```

Oh also, I've used ```Bogus``` library for generating fake data!

Happy coding yol! ;) 
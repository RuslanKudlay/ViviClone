**Branching strategy :**

- Naming of new branches should be done using the following format

    [category]/[JIRA-task-key]-[branch-summary]

Category:
> feature (This type of branch is used when new features are added to the software)

> bugfix (The type of branch is used when a bug is being fixed)


An example could be **bugfix/BTYDP-41-fix-some-bug**



**Connection to DB**

Open appsettings.json from Application layer 

You have to have the follow code:

```
 "Data": {
    "SQLType": "2",
    "Dev": {
      "SqlLiteConnectionString": "Data Source=BeautyDneprCoreDev.db",
      "MySqlConnectionString": "server= localhost;port=3306;database=BeautyDneprCoreDev;User ID = BeautyDneprDev;Password = Adminnimda123;",
      "SqlServerConnectionString": "server=.;database=BeautyDneprDev;User ID = BeautyDneprDev;Password = Adminnimda123; MultipleActiveResultSets=true;" 
    },
    "Prod": {
      "SqlLiteConnectionString": "Data Source=BeautyDneprCoreProd.db",
      "SqlServerConnectionString": "server=212.3.101.118,1433;database=BeautyDneprCoreDevTest;User ID = BeautyDneprProd;Password = Admin123;",
      "MySqlConnectionString": "server= localhost;port=3306;database=BeautyDneprCoreDev;User ID = BeautyDneprDev;Password = Adminnimda123;"      
    }    
  }
```

**SQLType** - is type of SQL 
> 1 - SQLLite
> 2 - MSSQL
> 3 - MySQL

**Dev** or **Prod** is depend on which environment we are using.

In our example we have SQLType = 2, it mean that we use **SqlServerConnectionString**
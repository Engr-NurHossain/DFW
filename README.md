# DFW Security - Account Tools

Managed by - Shailesh Pisat | [7900085116](tel:+917900085116) | [ace.dev.100@gmail.com](mailto:ace.dev.100@gmail.com)

## Getting started

Preferred Folder Structure to be mainted

```
├── dfw
│   ├── build
│   │   ├── build_dev
│   │   │   ├── db
│   │   │   ├── publish
│   │   ├── build_uat
│   │   │   ├── db
│   │   │   ├── publish
│   ├── src
│   │   ├── src_dev
│   │   ├── src_uat
│   └── publish
```

Local base folder path (in case of different drive change the drive letter)

```bash
C:\projects\dfw
```

Publish profiles are created for C & D drive letters named below.

- build_dev_c_drive

- build_dev_d_drive

Do not change the path in the Publish profiles, instead create path on the drive as advised.



### Usage of Logs

##### Error Logs

* Use to log errors inside the catch block

```csharp
logger.Error(ex)
```



##### Trace Logs

* Use this logs for tracking the application activity. 

* Avoid using too many Trace Logs in single class file.

* In a single call 2 properties can be recorded

* Send a text message in the Trace to give more information

* Trace logs are recorded in table AppLogs

```csharp
logger.WithProperty("property1", JsonConvert.SerializeObject(filters)).WithProperty("property2","testing data").Trace("Equipment History by {employee} for {period}", CurrentUser.GetFullName(), filters.Start + " to " + filters.End);
```





##### Debug Logs

* Use this logs to get information during development or debugging. Remove the logs once the purpose is solved

* Use tags to filter information from the logs easily.

* The params property will store the information of the parameters passed to the method. If the parameter is an object convert it to JSON string

* Send a text message in the Trace to give more information

* Trace logs are recorded in table DevLogs

```csharp
logger.WithProperty("tags", "equipment,history").WithProperty("params", JsonConvert.SerializeObject(filters)).Debug("Equipment History by {employee} for {period}", CurrentUser.GetFullName(), filters.Start +" to "+ filters.End);
```

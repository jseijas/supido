# supido
## WCF based framework to make REST APIs quickly

Imagine that you can make a REST API for your database as easy as design the API in a XML file, and without having to code the Business Objects or the services, only the DTOs, having it running in less than 5 minutes.

Example of configuration file:

```xml
<?xml version="1.0" encoding="utf-8" ?
<configuration
  <security sessionManager="Supido.Demo.Service.Security.SessionManager" securityManager="Supido.Demo.Service.Security.SecurityManager"</security
  <service cors="true" apiPath="api"
    <api path="client" dtoName="ClientDto" parameterName="ClientId"
      <api path="project" dtoName="ProjectDto" parameterName="ProjectId"
        <api path="service" dtoName="ServiceDto" parameterName="ServiceId"
          <api path="task" dtoName="TaskDto" parameterName="TaskId"</api
        </api
      </api
    </api
    <api path="department" dto="DepartmentDto" parameterName="DepartmentId"</api
  </service
  </configuration>
```

This example xml file will generate this API, exposed with CORS


* GET /api/client 
* GET /api/client/{clientId}
* POST /api/client/query (it's a get but receiving a query object)
* POST /api/client/{clientId}/query (it's a get but receiving a query object)
* POST /api/client
* PUT /api/client
* DELETE /api/client/{clientId}
* GET /api/client/{clientId}/project
* GET /api/client/{clientId}/project/{projectId}
* POST /api/client/{clientId}/project/query (it's a get but receiving a query object)
* POST /api/client/{clientId}/project/{projectId}/query (it's a get but receiving a query object)
* POST /api/client/{clientId}/project
* PUT /api/client/{clientId}/project
* DELETE /api/client/{clientId}/project/{clientId}
* GET /api/client/{clientId}/project/{projectId}/service
* GET /api/client/{clientId}/project/{projectId}/service/{serviceId}
* POST /api/client/{clientId}/project/{projectId}/service/query (it's a get but receiving a query object)
* POST /api/client/{clientId}/project/{projectId}/service/{serviceId}/query (it's a get but receiving a query object)
* POST /api/client/{clientId}/project/{projectId}/service
* PUT /api/client/{clientId}/project/{projectId}/service
* DELETE /api/client/{clientId}/project/{projectId}/service/{serviceId}
* GET /api/client/{clientId}/project/{projectId}/service/{serviceId}/task
* GET /api/client/{clientId}/project/{projectId}/service/{serviceId}/task/{taskId}
* POST /api/client/{clientId}/project/{projectId}/service/{serviceId}/task (it's a get but receiving a query object)
* POST /api/client/{clientId}/project/{projectId}/service/{serviceId}/task/{taskId} (it's a get but receiving a query object)
* POST /api/client/{clientId}/project/{projectId}/service/{serviceId}/task
* PUT /api/client/{clientId}/project/{projectId}/service/{serviceId}/task
* DELETE /api/client/{clientId}/project/{projectId}/service/{serviceId}/task/{taskId}
* GET /api/department
* GET /api/department/{departmentId}
* POST /api/department/query (it's a get but receiving a query object)
* POST /api/department/{departmentId}/query (it's a get but receiving a query object)
* POST /api/department
* PUT /api/department
* DELETE /api/department

Supido is based (for now) in Telerik Data Access as the ORM to the model, so what we are exposing in this example is this model:

![](https://raw.githubusercontent.com/jseijas/supido/master/images/step06.png)

#  Secured

Supido has an abstract security system, so you can implement your security. It's based on a **ISessionManager** to take care of the sessions and a **ISecurityManager** to take care of the login, logout.
Every call to the API must receive a **sessionToken** in order to identify the session of the user and create a securized context to the user.

## Queryable from the frontend

For the GETAll and GETOne operations, are a copy with the POST operation and finalized in **/query**. This allows the front to send a query information to the back, and the back will automatically convert it to a expression to the database. The query is based on **facets**. Let's see an example:

```json
{
    "Facets": [
        {
            "Name": "Position",
            "Values": [
                {
                    "Operation": "GreaterThan",
                    "Value": "10"
                }
            ]
        },
        {
        	"Name": "Priority",
            "Values": [
            	{
                	"Operation": "Equal",
                    "Value": "1"
                },
                {
                	"Operation": "Equal",
                    "Value": "3"
                }
            ]
        }
    ],
    "Orders": [
        {
            "Name": "Detail",
            "IsAscending": true
        }
    ],
    "SkipRecords": 100,
    "TakeRecords": 50
}
```


The example query will traduce the facets to:
**WHERE (Position > 10) AND (Priority = 1 OR Priority = 3)**
Also will add the order and bring only 50 records skiping 100 records (pagination).

The names used in the facet are the DTO property names, and internally are translated to the entity expressions.


# Extendable

You can modify the behaviour of your services in several ways, but there is an easy way: use filters.

You can define a filter for your Business Objects. Each filter has two operations:
```csharp
        IQueryable ApplySecurity(IQueryable query);

        IQueryable ApplyFilter(IQueryable query, QueryInfo info);
```

When a query operation is launched, those filters are called, first for security filtering and the second for the query.
The second one has no need of beeing overrided, it automatically resolves the query information.

The other option to change the service behaviour is to create your own Business Objects inheriting form **ContextBO** and overriding the operations that you need.

# Relationship 1-n between Entity and DTO

For each entity you can define several DTOs, representing several ways of projecting the data in the front. This way, you will have a BO and Service for each DTO.

# Starting guide

## 1. Create your database
## 2. Create a new solution
## 3. Create a project for the model using Telerik
### 3.1. Create the project
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step01.png)
### 3.2. Choose your database backend
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step02.png)
### 3.3. Choose your database connection
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step03.png)
### 3.4. Select the tables to be used
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step04.png)
### 3.5. Select how you want the model code to be done
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step05.png)
### 3.6. Enjoy the model
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step06.png)
## 4. Create a project for the service
* 4.1. Create the project
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step07.png)
* 4.2. Add the references
Your model proyect
Supido.Core
Supido.Business
Supido.Service
Telerik.OpenAccess and extensions
* 4.3. Create your DTOs. Put the [Dto] attribute in the Dto class. In the attribute you can specify the Entity type, otherwise it will be autoresolved following name rules.
![](https://raw.githubusercontent.com/jseijas/supido/master/images/step08.png)
* 4.4. Modify the web.config
You have one example of web.config (CORS configured) in 
https://raw.githubusercontent.com/jseijas/supido/master/Examples/Supido.Demo.Service/Web.config
* 4.5. Add a Session Manager
Here the session manager example:
https://github.com/jseijas/supido/blob/master/Examples/Supido.Demo.Service/Security/SessionManager.cs
* 4.6. Add a Security Manager
Here the security manager example:
https://github.com/jseijas/supido/blob/master/Examples/Supido.Demo.Service/Security/SecurityManager.cs
* 4.7. Adds a Security Service
In order to do the login/logout operations.
https://github.com/jseijas/supido/blob/master/Examples/Supido.Demo.Service/Services/Security.svc.cs
* 4.8. Define your API in Supido.xml file
* 4.9. Modify Global.asax
```csharp
protected void Application_Start(object sender, EventArgs e)
{
    ServiceInitializer.Initialize(null);
}

protected void Application_BeginRequest(object sender, EventArgs e)
{
    if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
    {
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, PUT, DELETE");
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
        HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
        HttpContext.Current.Response.End();
    }
}
```

## 5. Start the service!

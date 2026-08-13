# Controllers and Routing 🎯

Before we continue building our Web API, let's quickly recap some of the concepts from the previous lesson.

---

# Looking Back... 🔙

Before we dive into new concepts, let's answer a few questions:

- What is a Web API?
- What is the difference between an MVC web application and a Web API?
- What types of applications can communicate with a Web API?
- Can a Web API communicate with smart devices (IoT), such as a smart fridge?

Understanding these concepts is important because everything we'll build from this point forward relies on them.

---

# Controllers and Actions 🔸

Controllers are responsible for handling incoming HTTP requests and returning appropriate responses.

In MVC applications, controllers are responsible for both rendering Views and handling requests. In Web APIs, controllers focus only on processing HTTP requests and returning data.

.NET provides special features that make API controllers easier to work with, such as automatic model binding, serialization and HTTP response handling.

Actions are public methods inside a controller.

Each action represents an endpoint that clients can call.

Depending on the request, an action can return:

- Data
- Status codes
- Objects
- Collections
- Files
- Error responses

The return type is usually `ActionResult` or `ActionResult<T>`, allowing us to return different types of HTTP responses depending on the outcome of the request.

---

### API Controllers

API controllers are decorated with the **ApiController** attribute.

This enables features such as:

- Automatic model validation
- Better HTTP request handling
- Automatic serialization and deserialization
- Improved error responses

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
}
```

Notice that API controllers usually inherit from **ControllerBase**, since they don't need View-related functionality provided by the MVC `Controller` class.

---

### Actions

Every public method inside a controller can become an endpoint.

Examples:

```csharp
[HttpGet]
public IActionResult Get()
{
    return Ok();
}
```

```csharp
[HttpPost]
public IActionResult Create(UserDto model)
{
    return Ok();
}
```

Each action should clearly represent the operation it performs.

---

### 🤖 Let's Ask AI

```
Explain the difference between Controller and ControllerBase.
```

```
Why should API controllers inherit from ControllerBase?
```

```
Explain what the ApiController attribute actually does.
```

```
Review this controller and suggest improvements.
```

---

# Routing in Web APIs 🔸

Routing defines how incoming requests are matched to controller actions.

Since APIs don't have Views or pages, routing becomes the primary mechanism clients use to access functionality.

Every unique URL mapped to an action is called an **endpoint**.

Routing in .NET Web APIs is very similar to MVC, but API actions must explicitly specify the HTTP method they support.

---

## Controller Routing

The route is usually defined on the controller.

```csharp
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
}
```

This creates routes such as:

```
/api/users
```

The `[controller]` token is automatically replaced with the controller name.

---

## Action Routing

Actions define which HTTP method they respond to.

```csharp
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    return new[] { "Bob", "Jill" };
}
```

```csharp
[HttpGet("{id}")]
public ActionResult<string> Get(int id)
{
    return "value";
}
```

The second example accepts an `id` as a route parameter.

Example URLs:

```
GET /api/users
```

```
GET /api/users/1
```

---

## Custom Routes

Routes can contain multiple parameters and nested resources.

```csharp
[HttpGet("{id}/books")]
public ActionResult<IEnumerable<string>> Books(int id)
{
    return new[] { "Book 1", "Book 2" };
}
```

```csharp
[HttpGet("{userId}/books/{bookId}")]
public ActionResult<string> GetBook(int userId, int bookId)
{
    return $"User: {userId} - Book: {bookId}";
}
```

Example URLs:

```
GET /api/users/1/books
```

```
GET /api/users/1/books/5
```

---

### 🤖 Let's Ask AI

```
Explain routing in .NET Web APIs with practical examples.
```

```
What's the difference between Route and HttpGet attributes?
```

```
Explain route parameters using simple examples.
```

```
Generate five different route examples for a ProductsController.
```

```
Review these routes and suggest RESTful improvements.
```

---

# Demo 💻

During the demo we'll:

- Create our first API controller.
- Add multiple actions.
- Configure routes using attributes.
- Test the endpoints using the browser and Swagger.
- Observe how different URLs map to different actions.

By the end of this demo you'll understand how requests are routed through a .NET Web API application.
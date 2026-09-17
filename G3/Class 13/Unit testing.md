# Unit Testing 🧪

## Looking Back

In the previous lesson, we learned why testing is an essential part of the software development lifecycle and explored the most common types of software testing.

In this lesson, we'll focus on **Unit Testing**—the most common type of automated testing written by developers.

---

# What is Unit Testing? 🔶

**Unit Testing** is a software testing technique where individual units or components of an application are tested in isolation.

A **unit** is the smallest testable piece of code, such as:

- A method
- A class
- A service
- A business rule

The goal of unit testing is to verify that each unit behaves exactly as expected. :contentReference[oaicite:0]{index=0}

---

### 🤖 Let's Ask AI

```text
Explain Unit Testing in simple terms.
```

```text
What is considered a unit in software development?
```

```text
Why should developers write unit tests?
```

---

# Benefits of Unit Testing 🔶

Unit testing provides several important benefits throughout the development process.

Some of the biggest advantages include:

- Increased confidence when changing existing code.
- Easier maintenance and refactoring.
- Better code quality.
- More modular and reusable code.
- Faster bug detection.
- Lower cost of fixing defects.

Finding a bug during unit testing is significantly cheaper than finding it after deployment. :contentReference[oaicite:1]{index=1}

---

### 🤖 Let's Ask AI

```text
Why does unit testing reduce development costs?
```

```text
How does unit testing make refactoring safer?
```

```text
Why does unit testing encourage modular code?
```

---

# Arrange – Act – Assert (AAA Pattern) 🔶

Most unit tests follow a simple three-step structure known as the **AAA Pattern**.

## Arrange

Prepare everything required for the test.

Examples:

- Create objects
- Prepare test data
- Initialize dependencies

## Act

Execute the functionality being tested.

Usually this means calling a single method.

## Assert

Verify that the result matches the expected outcome.

This is where the test either passes or fails. :contentReference[oaicite:2]{index=2}

---

### Example

```csharp
// Arrange
var calculator = new Calculator();
int expected = 5;

// Act
int result = calculator.Add(2, 3);

// Assert
Assert.AreEqual(expected, result);
```

---

### 🤖 Let's Ask AI

```text
Explain the Arrange-Act-Assert pattern with an example.
```

```text
Why is the AAA pattern considered a best practice?
```

```text
Show me an example of a bad unit test and improve it using AAA.
```

---

# Naming Unit Tests 🔶

Unlike production code, unit test names should be **long and descriptive**.

A good test name should clearly communicate:

- What is being tested
- Under which condition
- What result is expected

A common naming convention is:

```text
MethodName_StateUnderTest_ExpectedBehavior
```

Examples:

```text
CheckAge_AgeUnder18_False

Sum_NonNumberInput_Exception

Login_InvalidPassword_ReturnsUnauthorized
```

Good test names make it easy to understand failures without opening the test implementation. :contentReference[oaicite:3]{index=3}

---

### 🤖 Let's Ask AI

```text
Generate descriptive unit test names for a Login method.
```

```text
Why are long test names acceptable?
```

```text
Improve these unit test names using the Method_State_Expected convention.
```

---

# Unit Testing Frameworks 🔶

There are several frameworks available for writing unit tests in .NET.

The most popular are:

- MSTest
- NUnit
- xUnit

All three support:

- Test classes
- Test methods
- Assertions
- Test execution
- Test reporting

The concepts are nearly identical, with differences mostly in syntax and available features. :contentReference[oaicite:4]{index=4}

> **Note**
>
> The examples in this lesson use **MSTest**, because it was the framework used in the original course materials. Today, **xUnit** is one of the most commonly used testing frameworks in modern .NET applications.

---

### 🤖 Let's Ask AI

```text
Compare MSTest, NUnit, and xUnit.
```

```text
Why is xUnit popular in modern .NET development?
```

```text
When should I choose MSTest instead of xUnit?
```

---

# Test Projects 🔶

Visual Studio provides project templates specifically designed for automated testing.

Common test project templates include:

- MSTest Test Project
- NUnit Test Project
- xUnit Test Project

Tests can be executed directly from **Visual Studio Test Explorer**, allowing developers to quickly verify that their code still behaves correctly after changes. :contentReference[oaicite:5]{index=5}

---

### 🤖 Let's Ask AI

```text
How do I create a test project in Visual Studio?
```

```text
How do I run unit tests using Test Explorer?
```

```text
Explain the structure of a typical .NET test project.
```

---

# Example: Simple Unit Tests 🔶

The following example demonstrates a simple service together with several unit tests.

The tests verify:

- Correct return values
- Boolean conditions
- Null values
- Expected exceptions

The examples also demonstrate the Arrange–Act–Assert pattern in practice. :contentReference[oaicite:6]{index=6}

> The complete code example from the original materials is included below.

*(Keep the original `ValueService` and `ValueTests` code exactly as it appears in the README.)*

---

### 🤖 Let's Ask AI

```text
Explain what each unit test in this example is verifying.
```

```text
Generate three additional unit tests for this service.
```

```text
Suggest edge cases that should also be tested.
```

---

# Summary

In this lesson we learned:

- What Unit Testing is.
- The benefits of writing unit tests.
- The Arrange–Act–Assert pattern.
- Best practices for naming tests.
- The most common .NET testing frameworks.
- How test projects are organized.
- How to write simple unit tests.

In the next lesson, we'll learn how to isolate dependencies using **Fake objects** and **Mocks**.

---

# Extra Materials 📘

- https://learn.microsoft.com/dotnet/core/testing/
- https://xunit.net/
- https://nunit.org/
- https://learn.microsoft.com/dotnet/core/testing/unit-testing-with-mstest
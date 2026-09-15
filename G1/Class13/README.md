# Software Testing 🧪

## Looking Back

In the previous lessons, we learned how to:

- Secure APIs using Authentication and Authorization
- Work with JWT tokens
- Understand OAuth 2.0 and external authentication providers

Now that we know how to build and secure APIs, it's time to learn how to verify that they work correctly.

Testing is an essential part of software development. It helps us detect bugs early, improve code quality, and confidently make changes without breaking existing functionality.

---

# Software Development Lifecycle (SDLC) 🔶

Building software is more than just writing code. A successful application goes through several phases before it reaches end users.

A typical software development lifecycle includes:

1. Understanding the requirements
2. Designing and planning
3. Development
4. Testing
5. Deployment

Each phase is equally important. Even a well-written application can fail if it is not properly tested before deployment. :contentReference[oaicite:0]{index=0}

---

### 🤖 Let's Ask AI

```text
Explain the Software Development Lifecycle (SDLC).
```

```text
Why is testing an important phase of software development?
```

```text
What can happen if an application skips the testing phase?
```

---

# Quality Assurance (QA) 🔶

Writing clean code is only one part of building quality software.

An application should also:

- Meet the business requirements
- Solve the intended problem
- Behave correctly in different scenarios
- Provide a good user experience

This is where **Quality Assurance (QA)** plays an important role.

QA engineers verify that the application behaves as expected and satisfies the original requirements. They collaborate closely with developers, product owners, and stakeholders to ensure the delivered software is reliable and meets quality standards. :contentReference[oaicite:1]{index=1}

---

## Manual vs Automated Testing

QA teams commonly perform testing in two ways.

### Manual Testing

A QA engineer manually interacts with the application to verify that it behaves correctly.

Examples:

- Creating users
- Submitting forms
- Testing API endpoints
- Verifying UI behavior

### Automated Testing

Instead of repeating the same actions manually, QA engineers write automated test scripts that execute predefined scenarios.

Automated tests can:

- Run much faster
- Execute repeatedly
- Detect regressions
- Produce test reports automatically

Besides testing, QA engineers often:

- Write test cases
- Create testing documentation
- Report defects
- Verify bug fixes
- Provide feedback to the development team

---

### 🤖 Let's Ask AI

```text
Explain the difference between manual and automated testing.
```

```text
When should manual testing be preferred over automated testing?
```

```text
What responsibilities does a QA engineer have besides testing?
```

---

# The Testing Phase 🔶

Testing begins after a feature has been implemented.

Since the expected behavior is already known, the application can be verified against its requirements.

During this phase we check whether:

- Features work correctly
- Requirements are satisfied
- Bugs are present
- Existing functionality still works

Testing is not a one-time activity. It is repeated throughout development until the application reaches the desired quality level. :contentReference[oaicite:2]{index=2}

---

### 🤖 Let's Ask AI

```text
Why is software testing an iterative process?
```

```text
What should be verified during the testing phase?
```

---

# Types of Software Testing 🔶

There are many different types of software testing, each focusing on a different aspect of an application.

Some of the most common testing types include:

- Unit Testing
- Integration Testing
- System Testing
- UI / Interface Testing
- Regression Testing
- Performance Testing
- Load Testing

These testing levels complement each other and help ensure the application behaves correctly from individual methods all the way to the complete system. :contentReference[oaicite:3]{index=3}

---

# Developer Tests 🔶

Developers also write tests for their own code.

Unlike manual QA testing, developer tests execute application code directly and verify that individual components behave correctly.

Developer tests help validate:

- Business logic
- Individual methods
- Communication between components
- Performance
- Reliability

The three most common categories are:

- **Unit Tests** – verify individual methods or small pieces of logic.
- **Integration Tests** – verify communication between multiple components.
- **System Tests** – verify the behavior of the complete application.

:contentReference[oaicite:4]{index=4}

---

### 🤖 Let's Ask AI

```text
Explain the difference between Unit, Integration, and System tests.
```

```text
Give examples of when each type of test should be used.
```

```text
Why do developers write automated tests?
```

---

# Unit vs Integration vs System Tests 🔶

| Unit Tests | Integration Tests | System Tests |
|------------|-------------------|--------------|
| Test a single method or class | Test communication between components | Test the complete application |
| Fast execution | Moderate execution time | Usually the slowest |
| Isolated from external dependencies | Multiple components work together | Entire system is tested |
| Written mostly by developers | Written by developers | Often executed by QA teams |

Each testing level provides confidence from a different perspective.

A healthy project usually contains a combination of all three.

---

### 🤖 Let's Ask AI

```text
Compare Unit, Integration, and System testing.
```

```text
Why can't unit tests replace integration tests?
```

```text
Explain the testing pyramid.
```

---

# Summary

In this lesson we learned:

- The role of testing in the Software Development Lifecycle.
- What Quality Assurance (QA) is.
- The difference between manual and automated testing.
- The purpose of the testing phase.
- The most common types of software testing.
- The differences between Unit, Integration, and System tests.

In the next lesson, we'll focus on **Unit Testing**, the most common type of automated testing written by developers.

---

# Extra Materials 📘

- https://www.softwaretestinghelp.com/types-of-software-testing/
- https://learn.microsoft.com/dotnet/core/testing/
- https://martinfowler.com/testing/

```
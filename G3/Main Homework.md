# Loto3000 App 🎲

## Requirements ✏

A Company that has a Lottery show is planning to expand and needs a web application that will bring similar experience to a wide range of users online. Every person that thinks they have the lucky numbers can enter in numbers for the next draw. A person from the company initiates the draw, random numbers are drawn and the numbers that the people submitted until that point in time are checked if they are a winning combination. Winners should be added in a separate board where they can be accessed by anyone. All of these functionalities need to be accessible by any device so that the players can play on a different variety of devices. 
> The client side of the application will be handled by other development teams.
> There is no need for payment implementation. The payment will be handled by a third party service.

## Key Logic ✏

* User - A player that is  able to play the game
* Admin - A company employee that can initiate a draw
* Draw - The process of drawing the numbers, finding winners and starting the next session
* Session - The time between the last and next draw
* Prize - A prize for winning a combination

> Note: This is an explanation about the key logic of the game. It is not a pattern or example of how the structure of the application will look. The developer is free to structure the application in any way they see fit

## Rules ✏

* Game
  * Numbers that can be picked are from 1 to 37
  * A player must choose 7 numbers out of 1 to 37
  * Prizes are given for matching:
    * 7 ( JackPot ) - Car
    * 6 - Vacation
    * 5 - TV
    * 4 - 100$ Gift Card
    * 3 - 50$ Gift Card
  * A user can submit multiple tickets
  * The user identification can be done by any means ( Ex. username, Full name, id etc. )
* Draw
  * A draw is initiated by an admin
  * When a draw is initiated all people that entered numbers until that point in time enter in play
  * When a draw is initiated random 8 numbers are picked
  * When random numbers are picked winners are selected and added to the winners board
  * When a draw is finished and winners are selected a new draw session begins
  * When a new draw session begins players can again enter numbers until an Admin starts a draw
* Winners
  * When a user enters numbers it should get a link to the winners board and a message saying they should wait for the draw and if they get a prize they will see their name on the board
  * The winners board should display the name and last name of the winner as well as the winning numbers and the prize they have won
  * The winners board should be accessible by anyone

## Bonus

* Implement authentication
* Implement logging


# 🤖 AI Guidance

Artificial Intelligence can be a great assistant while building this project, but remember that it should help you understand the concepts rather than complete the entire assignment for you.

Try using AI for tasks like:

### Planning

```text
Help me break down this lottery application into smaller implementation tasks.
```

```text
Suggest a clean folder structure for a .NET Web API lottery application.
```

---

### Designing Models

```text
Suggest entity models for a lottery application based on these requirements.
```

```text
Explain the relationships between User, Ticket, Draw, Session and Prize.
```

---

### API Design

```text
Suggest REST endpoints for a lottery application.
```

```text
Review my API design and suggest improvements.
```

---

### Implementation

```text
Generate a controller that follows REST conventions.
```

```text
Explain how to implement random number generation in C#.
```

```text
Explain how to compare two collections and count matching numbers.
```

---

### Entity Framework

```text
Suggest an Entity Framework model for this application.
```

```text
Review my DbContext and entity relationships.
```

---

### Validation

```text
Suggest validation rules for lottery ticket submission.
```

```text
How can I prevent duplicate numbers in a ticket?
```

---

### Testing

```text
Suggest unit tests for the Draw service.
```

```text
What edge cases should I test in this application?
```

---

### Code Review

```text
Review this code and suggest improvements without rewriting the whole solution.
```

```text
Explain if my implementation follows SOLID principles.
```

---

> 💡 **Tip**
>
> Avoid prompts such as *"Build the entire application for me."* Instead, ask AI to explain concepts, review your code, or help you solve one problem at a time. You'll learn much more and build stronger development skills.


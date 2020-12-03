# Twilight

## Introduction

Twilight is an implementation of the CQRS design pattern that, 'out-of-the-box', provides an in-memory message transport that uses [Autofac](https://autofac.org/) to register its components. It is meant to be used as a learning resource for the basis of a full-featured system.

It is not intended for this repository to be published as NuGet packages as support is not possible. The expectation is to use this repository as an educational resource and fork for your own specific requirements.

### Requirements

- Microsoft Visual Studio 2019
- .NET 5.0

If using Visual Studio 2019, it is recommended that all relevant updates are applied.

## CQRS

CQRS stands for Command Query Responsibility Segregation, i.e. keep actions that change data separate from actions that query data sources.

In its essence, CQRS enables the separation of a read model from a write model. You can use different data sources for your read and write models - or not. It also makes it extremely easy to keep separate concerns just that - separate.

CQRS is often confused with Event Sourcing (ES) as CQRS can form a solid foundation to support ES (e.g. querying in ES is difficult without CQRS or some similar mechanism) but ES is not required for CQRS.

This repository currently provides an example of CQRS but remember - you don't always need to use CQRS. When implementing a solution, don't over-engineer and always ask yourself if the implemented architecture can be simplified.

The sample in this repository is trivial in order to clearly demonstrate usage.

Further information on the use-case for CQRS: [https://microservices.io/patterns/data/cqrs.html](https://microservices.io/patterns/data/cqrs.html)

## Event Sourcing (ES)

As we have touched on the topic above, a short expansion on what ES provides is warranted.

ES persists the state of a business entity as a sequence of state-changing events. There is no static 'CRUD' data model.

The current state of a domain entity is 'rebuilt' by replaying recorded events until the current state is reached. A bank account is a good example - there is no recorded account balance, rather the balance is calculated by replaying the deposit, transfer and withdrawal messages for that account until a current account balance is reached.

ES is a non-trivial pattern that requires a deep understanding of many advanced topics and patterns to implement and therefore is not suitable for all business use-cases. It can lead to an overly complicated architecture and an excessive maintenance overhead.

Further information on the use-case for ES: [https://microservices.io/patterns/data/event-sourcing.html](https://microservices.io/patterns/data/event-sourcing.html)

Note: A future sample will expand upon the use of event sourcing. The included aggregate is for convenience and is not opinionated.

## CQRS Components

CQRS revolves around the production and consumption of messages, of which there are three types: commands, queries and events.

### Messages

A message conveys intent and purpose within a software system. Messages have a unique identifier (the message id) and a correlation id that identifies a series of actions in a workflow. In addition, there is a causation identifier. The causation identifier specifies which message (if any) caused the current message to be produced. The correlation id, together with the causation id enables the tracing of the entire journey a message may take through a system.

Messages form the basis of CQRS commands, queries and events. A command, query or event is a message: something you send to a recipient.

#### Commands

Commands are messages that effect change in a system (like updating a record in a database). A command can have zero or more parameters.

Strictly, commands should be fire-and-forget. This can be a difficult concept to take on board as the retrieval of the result of a command (e.g. the identifier for a created entity) is decoupled from the process that creates it.

#### Queries

Queries request information from a system and receive a response payload containing the requested data (if found). A query may or may not have a request payload that is used to shape the data in the response payload.

#### Events

Events are often published in order to tell other parts of a system that a change has occurred, for example to update a data view for reporting analysis. An event may or may not contain a payload with information useful to a party listening for the event.

### Handlers

A handler is required to act as a middleman between messages and the intended destination(s) for their payloads. A handler will consume a message, decide what to do with it (e.g. call a downstream service) and may return a response (that can be used to indicate success or contain a payload). Any error encountered while handling a message (such as the message failing validation) causes an exception to be thrown.

There can only be one handler for a specific command or query. Unlike commands and queries, events can be consumed by multiple handlers (a powerful capability of CQRS).

## Architecture

Twilight CQRS is broken into discrete areas of functionality in order to best separate concerns. It allows the implementer to use as much or as little of the code as possible without attaching unwanted dependencies.

The following dependency graph shows the careful planning of the relationships between the components of Twilight CQRS.

![Dependencies Graph](DependenciesGraph.png)

## API Documentation

The XML documentation for the public API can be used to generate full developer documentation for Twilight.

You can use DocFX to generate the documentation. For more information, see [here](https://dotnet.github.io/docfx/).

## Sample: Console App

This simple sample exists to show the mechanics of creating, sending and handling commands, queries and events.

The structure of parameters is up to you as the implementer. Strive for as little mutability as possible. The console sample is stripped down to make it easy to follow. Keep that in mind.

Note that when run in debug, you should expect execution to stop for a validation exception. This is expected behaviour.

## Naming Twilight

This project started out some time ago with a grander aim and was going to use the Paramore Brighter and Darker repositories. When that was dropped as being unnecessarily ambitious, the name as a combination of the Brighter and Darker, Twilight, stuck with attendant *"can you see what I did there?"* gusto.

The Brighter and Darker projects are excellent examples of how complicated the topic of CQRS can get!

Paramore, incidentally, is also the name of an American rock group and *"Brighter"* is the fourth track from their first album, *"All We Know is Falling"*. Paramore also have a track called, *"Decode"* and that was the second song played in the end credits of the 2008 romantic fantasy film, *"Twilight"*. There you go.

## Sources

We all learn from the teachings and example of others and this project is no exception. The following sources provided pointers and inspiration for this repository:

- [https://github.com/jbogard/MediatR](https://github.com/jbogard/MediatR)
- [https://martinfowler.com/bliki/CQRS.html](https://martinfowler.com/bliki/CQRS.html)

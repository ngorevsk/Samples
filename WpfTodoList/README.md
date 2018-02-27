# WpfTodoList

This is a simple WPF project I completed during a weekend.  

Domain: To-Do List

Interface: Native GUI

Platform: .NET

Language: C#  

## Requirements:

TODO List

* As a user, I can add a TODO to the list.
* As a user, I can add one or more sub-TODOs to a top-level TODO. The application should only allow for a depth of 2 (parent/child relationship, no grandchildren). i.e., A child TODO should not be allowed to have children of its own.
* If a sub-TODO exist and are all closed, then the parent TODO should close.
* If a parent TODO closes, then all sub-TODOs should close.
* As a user, I can see all the TODOs on the list in an overview.
* As a user, I can drill into a TODO to see more information about the TODO.
* As a user, I can delete a TODO.
* As a user, I can mark a TODO as completed.
* As a user, when I see all the TODOs in the overview, if today's date is past the TODO's deadline, highlight it.
* A TODO consists of a task (just a simple sentence or two is fine), a deadline date, a completed flag, and an optional "more details" field that allows for more details to be given (again, a single large text area is fine for this).
The list of TODOs can be kept in memory.

## Steps to run project:

1) Open Solution in Visual Studio 2017.
2) Perform NuGet package restore on solution to satisfy dependencies.
3) Set WpfTodoList project as startup project.

## Design considerations:

* Prism - (MVVM) Framework for purpose for designing loosely coupled, maintainable, and testable application.
* Unit Tests - UnitTest project contains tests against the basic requirements of the app.  As I was working on unit tests, I ended up refactoring the MainView and moved out most logic dealing with manipulating the root collection to the TodoManager class.

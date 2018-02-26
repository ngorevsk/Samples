using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfTodoList.ViewModels;
using Prism.Regions;
using Moq;
using Microsoft.Practices.ServiceLocation;
using WpfTodoList.Models;
using System.Collections.ObjectModel;
using WpfTodoList.Services;

namespace UnitTest
{
    [TestClass]
    public class TodoManagerTests
    {
        [TestMethod]
        public void Add1ParentTodo()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo todoToAdd = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(todoToAdd);

            // Verify
            var expectedCount = 1;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);

            var expectedIsParent = true;
            var actualIsParent = todoManager.Parents[0].Parent == null;
            Assert.AreEqual(expectedIsParent, actualIsParent);
        }
        
        [TestMethod]
        public void Add1ParentWith1ChildTodo()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo childTodo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(childTodo, parentTodo);

            // Verify
            var expectedParentCount = 1;
            var actualParentCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedParentCount, actualParentCount);

            var expectedChildCount = 1;
            var actualChildCount = todoManager.GetTotalChildCount();
            Assert.AreEqual(expectedChildCount, actualChildCount);

            var expectedParent = parentTodo;
            var actualParent = childTodo.Parent;
            Assert.AreEqual(expectedParent, actualParent);

            var expectedChild = childTodo;
            var actualChild = parentTodo.Children[0];
            Assert.AreEqual(expectedParent, actualParent);
        }

        [TestMethod]
        public void Add1ParentAndRemoveIt()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo todoToAdd = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(todoToAdd);
            todoManager.RemoveTodo(todoToAdd);

            // Verify
            var expectedCount = 0;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void Add1ParentAnd1ChildAndRemoveChild()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo childTodo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(childTodo, parentTodo);
            todoManager.RemoveTodo(childTodo);

            // Verify
            var expectedCount = 1;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);

            var expectedChildCount = 0;
            var actualChildCount = todoManager.GetTotalChildCount();
            Assert.AreEqual(expectedChildCount, actualChildCount);
        }

        [TestMethod]
        public void Add1ParentAnd1ChildAndRemoveParent()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo childTodo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(childTodo, parentTodo);
            todoManager.RemoveTodo(parentTodo);

            // Verify
            var expectedCount = 0;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);

            var expectedChildCount = 0;
            var actualChildCount = todoManager.GetTotalChildCount();
            Assert.AreEqual(expectedChildCount, actualChildCount);
        }

        [TestMethod]
        public void Add1ParentWithinDeadline()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo()
            {
                Task = "Parent1",
                Deadline = DateTime.Now.Date
            };
            todoManager.AddTodo(parentTodo);

            // Verify
            var expectedIsHighlighted = false;
            var actualIsHighlighted = parentTodo.IsHighlighted;
            Assert.AreEqual(expectedIsHighlighted, actualIsHighlighted);
        }

        [TestMethod]
        public void Add1ParentPastDeadline()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo()
            {
                Task = "Parent1",
                Deadline = DateTime.Now.Date.Subtract(new TimeSpan(1,0,0,0))
            };
            todoManager.AddTodo(parentTodo);

            // Verify
            var expectedIsHighlighted = true;
            var actualIsHighlighted = parentTodo.IsHighlighted;
            Assert.AreEqual(expectedIsHighlighted, actualIsHighlighted);
        }

        [TestMethod]
        public void Add1Parent1Child1Grandchild()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo childTodo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(childTodo, parentTodo);
            ITodo grandchildTodo = new Todo() { Task = "Grandchild1", };
            todoManager.AddTodo(grandchildTodo, childTodo);

            // Verify
            var expectedCount = 2;
            var actualCount = todoManager.GetTotalTodoCount();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void AddNullParentTodo()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = null;
            todoManager.AddTodo(parentTodo);
            
            // Verify
            var expectedCount = 0;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void Add1ParentAndRemoveNullTodo()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo todoToRemove = null;
            todoManager.RemoveTodo(todoToRemove);

            // Verify
            var expectedCount = 1;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void Add1ParentAndRemoveUnaddedTodo()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo todoToRemove = new Todo() { Task = "Parent1", };
            todoManager.RemoveTodo(todoToRemove);

            // Verify
            var expectedCount = 1;
            var actualCount = todoManager.GetTotalParentCount();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void Add1ParentAnd1ChildAndCompleteParent()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo childTodo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(childTodo, parentTodo);
            parentTodo.IsCompleted = true;

            // Verify
            var expectedParentIsCompleted = true;
            var actualParentIsCompleted = parentTodo.IsCompleted;
            Assert.AreEqual(expectedParentIsCompleted, actualParentIsCompleted);

            var expectedChildIsCompleted = true;
            var actualChildIsCompleted = childTodo.IsCompleted;
            Assert.AreEqual(expectedChildIsCompleted, actualChildIsCompleted);
        }

        [TestMethod]
        public void Add1ParentAnd1ChildAndCompleteChild()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo childTodo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(childTodo, parentTodo);
            childTodo.IsCompleted = true;

            // Verify
            var expectedParentIsCompleted = true;
            var actualParentIsCompleted = parentTodo.IsCompleted;
            Assert.AreEqual(expectedParentIsCompleted, actualParentIsCompleted);

            var expectedChildIsCompleted = true;
            var actualChildIsCompleted = childTodo.IsCompleted;
            Assert.AreEqual(expectedChildIsCompleted, actualChildIsCompleted);
        }

        [TestMethod]
        public void Add1ParentAnd2ChildrenAndComplete1Child()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo child1Todo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(child1Todo, parentTodo);
            ITodo child2Todo = new Todo() { Task = "Child2", };
            todoManager.AddTodo(child2Todo, parentTodo);
            child1Todo.IsCompleted = true;

            // Verify
            var expectedParentIsCompleted = false;
            var actualParentIsCompleted = parentTodo.IsCompleted;
            Assert.AreEqual(expectedParentIsCompleted, actualParentIsCompleted);

            var expectedChild1IsCompleted = true;
            var actualChild1IsCompleted = child1Todo.IsCompleted;
            Assert.AreEqual(expectedChild1IsCompleted, actualChild1IsCompleted);

            var expectedChild2IsCompleted = false;
            var actualChild2IsCompleted = child2Todo.IsCompleted;
            Assert.AreEqual(expectedChild2IsCompleted, actualChild2IsCompleted);
        }

        [TestMethod]
        public void Add1ParentAnd2ChildrenAndCompleteParent()
        {
            // Arrange
            ITodoManager todoManager = new TodoManager();

            // Act
            ITodo parentTodo = new Todo() { Task = "Parent1", };
            todoManager.AddTodo(parentTodo);
            ITodo child1Todo = new Todo() { Task = "Child1", };
            todoManager.AddTodo(child1Todo, parentTodo);
            ITodo child2Todo = new Todo() { Task = "Child2", };
            todoManager.AddTodo(child2Todo, parentTodo);
            parentTodo.IsCompleted = true;

            // Verify
            var expectedParentIsCompleted = true;
            var actualParentIsCompleted = parentTodo.IsCompleted;
            Assert.AreEqual(expectedParentIsCompleted, actualParentIsCompleted);

            var expectedChild1IsCompleted = true;
            var actualChild1IsCompleted = child1Todo.IsCompleted;
            Assert.AreEqual(expectedChild1IsCompleted, actualChild1IsCompleted);

            var expectedChild2IsCompleted = true;
            var actualChild2IsCompleted = child2Todo.IsCompleted;
            Assert.AreEqual(expectedChild2IsCompleted, actualChild2IsCompleted);
        }
    }
}

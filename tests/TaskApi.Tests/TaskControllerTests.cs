using TaskApi.Controllers;
using TaskApi.Models;
using TaskApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace TaskApi.Tests.Controllers;
public class TaskControllerTests{

    private readonly TasksController _ctrl;
    private readonly Mock<ITaskRepository> _mockRepo;

    public TaskControllerTests(){
        _mockRepo = new Mock<ITaskRepository>();
        _ctrl = new TasksController(_mockRepo.Object);
    }

    //probar GetAll cuando hay tarea retorna Ok con lista de tareas
    [Fact]
    public void GetAll_RepositorioConTareas_RetornaOkConListaDeTareas(){
        //Arrange
        var tareas = new List<TaskItem> {
            new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1", IsCompleted = false },
            new TaskItem { Id = 2, Title = "Tarea 2", Description = "Descripción de la tarea 2", IsCompleted = true }
        };
        _mockRepo.Setup(r => r.GetAll()).Returns(tareas);
        //Act
        var resultado = _ctrl.GetAll();
        //Assert
        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        var returnedTareas = okResult.Value.Should().BeAssignableTo<IEnumerable<TaskItem>>().Subject;
        returnedTareas.Should().HaveCount(2);
    }

    //Get by id cuando la tarea existe retorna Ok con la tarea
    [Fact]
    public void GetById_TareaExistente_RetornaOkConTarea(){
        //Arrange
        var tarea = new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1", IsCompleted = false };
        _mockRepo.Setup(r => r.GetById(1)).Returns(tarea);
        //Act
        var resultado = _ctrl.GetById(1);
        //Assert
        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        var returnedTarea = okResult.Value.Should().BeAssignableTo<TaskItem>().Subject;
        returnedTarea.Should().Be(tarea);
    }

    //Get by id cuando la tarea no existe retorna NotFound result
    [Fact]    
    public void GetById_TareaNoExistente_RetornaNotFound(){
        //Arrange
        _mockRepo.Setup(r => r.GetById(1)).Returns((TaskItem?)null);
        //Act
        var resultado = _ctrl.GetById(1);
        //Assert
        resultado.Should().BeOfType<NotFoundResult>();

    }

    


}
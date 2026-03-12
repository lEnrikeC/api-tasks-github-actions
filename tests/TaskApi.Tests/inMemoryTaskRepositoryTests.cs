using TaskApi.Repositories;
using TaskApi.Models;
using FluentAssertions;
namespace TaskApi.Tests.Repositories;
public class InMemoryTaskRepositoryTests {
    private readonly InMemoryTaskRepository _repo;
    public InMemoryTaskRepositoryTests(){
        _repo = new();
    }
    
    [Fact]
    public void Add_TareaValida_AsignaIdYRetornaTarea(){
        //Arrange        
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"
        };
        //Act
        var resultado = _repo.Add(tarea);
        //Assert
        resultado.Id.Should().BeGreaterThan(0);
        resultado.Title.Should().Be("Comprar Guitarra");
    }

    [Fact]
    public void Add_TareaValida_AumentaConteoDeTareas(){
        //Arrange
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"
        };
        var conteoInicial = _repo.GetAll().Count();
        //Act
        _repo.Add(tarea);
        var conteoFinal = _repo.GetAll().Count();
        //Assert
        conteoFinal.Should().Be(conteoInicial + 1);
    }   

    //GetAll
    [Fact]    
    public void GetAll_RepositorioVacio_RetornaColeccionVacia(){
        //Arrange    

        //Act    
        var tareas = _repo.GetAll();
        //Assert
        tareas.Should().BeEmpty();
    }

    //Cuando se agregan dos tareas se regresen ambas al llamar GetAll
    [Fact]    
    public void GetAll_RepositorioConTareas_RetornaColeccionConTareas(){
        //Arrange    
        var tarea1 = new TaskItem {
            Title = "Comprar Guitarra1",
            Description= "Comprar Guitarra para ser Feliz1"
        };
        var tarea2 = new TaskItem {
            Title = "Comprar Guitarra2",
            Description= "Comprar Guitarra para ser Feliz2"
        };
        _repo.Add(tarea1);
        _repo.Add(tarea2);
        //Act    
        var tareas = _repo.GetAll();
        //Assert
        tareas.Should().HaveCount(2);
    }


    //GETBYID
    [Fact]
    public void GetById_TareaExistente_RetornaTarea(){
        //Arrange    
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"
        };
        _repo.Add(tarea);
        //Act    
        var resultado = _repo.GetById(tarea.Id);
        //Assert
        resultado.Should().Be(tarea);
        resultado.Should().NotBeNull();
        resultado!.Title.Should().Be("Comprar Guitarra");
    }

    //Si no existe la tarea, debe regresar null
    [Fact]
    public void GetById_TareaNoExistente_RetornaNull(){
        //Arrange    
        //Act    
        var resultado = _repo.GetById(35);
        //Assert
        resultado.Should().BeNull();
    }

    //Probar la funcion update, verificar que la tarea existe y se actualice sus propiedades
    [Fact]
    public void Update_TareaExistente_ActualizaTarea(){
        //Arrange    
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"
        };
        _repo.Add(tarea);
        var tareaActualizada = new TaskItem {
            Title = "Comprar Guitarra Actualizada :)",
            Description= "Comprar Guitarra para ser Feliz Actualizada :D",
          
        };
        //Act    
        var resultado = _repo.Update(tarea.Id, tareaActualizada);
        //Assert
        resultado.Should().NotBeNull();
        resultado!.Title.Should().Be("Comprar Guitarra Actualizada :)");
        resultado.Description.Should().Be("Comprar Guitarra para ser Feliz Actualizada :D");
       
    }

    //si el id no existe, debe regresar null
    [Fact]
    public void Update_TareaNoExistente_RetornaNull(){
        //Arrange    
        var tareaActualizada = new TaskItem {
            Title = "Comprar Guitarra Actualizada :)",
            Description= "Comprar Guitarra para ser Feliz Actualizada :D",
          
        };
        //Act    
        var resultado = _repo.Update(35, tareaActualizada);
        //Assert
        resultado.Should().BeNull();    
    }

    //Delete si la tarea existe, debe eliminarla y regresar true
    [Fact]
    public void Delete_TareaExistente_EliminaTarea(){
        //Arrange    
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"
        };
        _repo.Add(tarea);
        //Act    
        var resultado = _repo.Delete(tarea.Id);
        //Assert
        resultado.Should().BeTrue();
        _repo.GetById(tarea.Id).Should().BeNull();
    }

    //Delete si la tarea no existe, debe regresar false
    [Fact]
    public void Delete_TareaNoExistente_RetornaFalse(){
        //Arrange    
        //Act    
        var resultado = _repo.Delete(35);
        //Assert
        resultado.Should().BeFalse();
    }

    
}
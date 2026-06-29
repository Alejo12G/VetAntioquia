using Xunit;
using VeterinariaAntioquia.Shared.Models;
using VeterinariaAntioquia.Shared.Models.Styles;


namespace VeterinariaAntioquia.Tests;


public class UnitTest1
{


    [Fact]
    public void LoginRequest_DebeGuardarEmailYPassword()
    {
        var login = new LoginRequest
        {
            Email = "test@test.com",
            Password = "123456"
        };


        Assert.Equal("test@test.com", login.Email);
        Assert.Equal("123456", login.Password);
    }




    [Fact]
    public void RegisterRequest_DebeGuardarDatosUsuario()
    {

        var registro = new RegisterRequest
        {
            Nombre = "Juan",
            Email = "juan@test.com",
            Password = "123"
        };


        Assert.Equal("Juan", registro.Nombre);
        Assert.Equal("juan@test.com", registro.Email);
        Assert.Equal("123", registro.Password);

    }




    [Fact]
    public void LoginResponse_DebeContenerUsuarioYToken()
    {

        var response = new LoginResponse
        {
            Token = "token123",

            Usuario = new UsuarioDto
            {
                Id = 1,
                Nombre = "Carlos",
                Rol = "cliente"
            }
        };


        Assert.NotNull(response.Usuario);
        Assert.Equal("token123", response.Token);
        Assert.Equal("cliente", response.Usuario.Rol);

    }




    [Fact]
    public void Cita_DebeTenerEstadoProgramadaPorDefecto()
    {

        var cita = new Cita();


        Assert.Equal("programada", cita.Estado);

    }




    [Fact]
    public void Cita_DebeGuardarInformacionCorrectamente()
    {

        var cita = new Cita
        {
            Id = 10,
            IdVeterinario = 2,
            IdServicio = 5,
            Fecha = new DateTime(2026, 6, 20),
            DuracionMinutos = 30,
            Motivo = "Vacuna"
        };


        Assert.Equal(10, cita.Id);
        Assert.Equal(2, cita.IdVeterinario);
        Assert.Equal(5, cita.IdServicio);
        Assert.Equal("Vacuna", cita.Motivo);
        Assert.Equal(30, cita.DuracionMinutos);

    }




    [Fact]
    public void Mascota_DebeCalcularEdadCorrectamente()
    {

        var mascota = new Mascota
        {
            Nombre = "Firulais",
            FechaNacimiento = new DateTime(2000, 1, 1)
        };


        Assert.True(mascota.EdadAnios >= 25);

    }





    [Fact]
    public void Mascota_SinFechaNacimientoDebeRetornarNull()
    {

        var mascota = new Mascota
        {
            Nombre = "Luna"
        };


        Assert.Null(mascota.EdadAnios);

    }




    [Fact]
    public void ToastModel_DebeCrearIdAutomaticamente()
    {

        var toast = new ToastModel
        {
            Message = "Guardado correctamente",
            Type = ToastType.Success
        };


        Assert.NotEqual(Guid.Empty, toast.Id);

        Assert.Equal(
            "Guardado correctamente",
            toast.Message
        );

        Assert.Equal(
            ToastType.Success,
            toast.Type
        );

    }




    [Fact]
    public void Servicio_DebeGuardarPrecioYDuracion()
    {

        var servicio = new Servicio
        {
            Nombre = "Vacunación",
            PrecioBase = 50000,
            DuracionEstimadaMin = 30
        };


        Assert.Equal(
            "Vacunación",
            servicio.Nombre
        );


        Assert.Equal(
            50000,
            servicio.PrecioBase
        );


        Assert.Equal(
            30,
            servicio.DuracionEstimadaMin
        );

    }


}
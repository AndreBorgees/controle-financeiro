using System;
using Xunit;
using ControleFinanceiro.Models;

namespace ProjetoTestesUnitario
{
    public class UnitTestModel
    {
        [Fact]
        public void TestValidaUsuario()
        {
            UsuarioModel objUsuario = new UsuarioModel();

            objUsuario.Email = "borges.andree@gmail.com";
            objUsuario.Senha = "1234";

            Assert.True(objUsuario.validarLogin());
        }

        [Fact]
        public void TestRegistraUsuario()
        {
            UsuarioModel objUsuario = new UsuarioModel();

            objUsuario.Nome = "teste";
            objUsuario.Senha = "1234";
            objUsuario.Email = "teste.andree@gmail.com"; 
            objUsuario.dataNascimento = "2018/11/17";
            objUsuario.RegistrarUsuario();

            objUsuario.Email = "teste.andree@gmail.com";
            objUsuario.Senha = "1234";

            Assert.True(objUsuario.validarLogin());
        }
    }
}

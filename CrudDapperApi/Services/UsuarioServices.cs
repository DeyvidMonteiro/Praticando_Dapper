using AutoMapper;
using CrudDapperApi.Dto;
using CrudDapperApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CrudDapperApi.Services
{
    public class UsuarioServices : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UsuarioServices(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                var usuariosBanco = await connection.QueryAsync<Usuario>("select * from usuarios");

                if (usuariosBanco.Count() == 0)
                {
                    response.Mensagem = "Nehum usuário localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuarios localizados com sucesso!";
            }
            return response;
        }

        public async Task<ResponseModel<UsuarioListarDto>> BuscarUsuariosPorId(int usuarioId)
        {
            ResponseModel<UsuarioListarDto> response = new ResponseModel<UsuarioListarDto>();

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                var usuariosBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("select * from usuarios where id = @Id", new { Id = usuarioId });

                if (usuariosBanco == null)
                {
                    response.Mensagem = "Nehum usuário localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<UsuarioListarDto>(usuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuario localizados com sucesso!";

            }
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuarioCriarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuariosBanco = await connection.ExecuteAsync("insert into usuarios (NomeCompleto, Email, Cargo, CPF, Salario, Situacao, Senha) " +
                                                                  "values(@NomeCompleto, @Email, @Cargo, @CPF, @Salario, @Situacao, @Senha)", usuarioCriarDto);

                if (usuariosBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuário listado com sucesso!";
            }
            return response;

        }

        private static async Task<IEnumerable<Usuario>> ListarUsuarios(MySqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>("select * from usuarios");
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuariosBanco = await connection.ExecuteAsync(
                @" UPDATE usuarios SET 
                    NomeCompleto = @NomeCompleto, 
                    Email = @Email, 
                    Cargo = @Cargo, 
                    Salario = @Salario, 
                    Situacao = @Situacao,
                    CPF = @CPF
                where Id = @Id", usuarioEditarDto
                );

                if(usuariosBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao processar";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuarios listados com sucesso";

            }

            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuario(int usuarioId)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuariosBanco = await connection.ExecuteAsync("delete from usuarios where id = @Id", new {Id = usuarioId});

                if (usuariosBanco == 0)
                {
                    response.Mensagem = "Nehum usuário localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuarios localizados com sucesso!";
            }
            return response;


        }
    }
}

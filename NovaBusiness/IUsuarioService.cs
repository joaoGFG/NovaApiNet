using NovaModel;

namespace NovaBusiness.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioModel>> ListarTodos();
        Task<UsuarioModel?> ObterPorId(int id);
        Task<UsuarioModel> Criar(UsuarioModel usuario);
        Task<UsuarioModel?> Atualizar(int id, UsuarioModel usuario);
        Task<bool> Deletar(int id);
        Task<List<UsuarioModel>> Buscar(string? nome, string? areaInteresse, int pageNumber, int pageSize, string? orderBy);
    }
}
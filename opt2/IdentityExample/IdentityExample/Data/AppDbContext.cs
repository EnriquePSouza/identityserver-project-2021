using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Data
{
    // Essa classe informa todas as funcionalidades para a comunicação com o banco de dados.
    // O IdentityDbContext contém todas as tabelas dos usuarios.
    // Se vc tem sua propria table, vc precisa especificar ela aqui, com 'IdDbCont<minhatabela>'.
    // Precisa definir a tabela no 'UserManager' la no controller tbm.
    // E nas configs. de service no startup tbm.
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
    }
}
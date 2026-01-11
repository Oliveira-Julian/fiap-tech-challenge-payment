using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos.Interfaces;

public interface IProdutoImagemRepository : IRepositoryBase<ProdutoImagemDocument>
{
    Task<ICollection<ProdutoImagemDocument>> GetByProductIdAsync(Guid idProduct, CancellationToken cancellationToken, bool tracking = false);
}

// Documento separado para imagens quando armazenadas separadamente
public class ProdutoImagemDocument : EntityBase
{
    public Guid? IdProduto { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public decimal Tamanho { get; set; }
    public byte[] Conteudo { get; set; }
}

namespace moreno.myrep.domain.Dtos;

public class PaginacaoEntradaDto
{
    private const int QTD_ITEMS_PAGINA = 10;

    private int _pagina = 1;
    public int Pagina
    {
        get => _pagina;
        set => _pagina = value;
    }

    private int _tamanho = QTD_ITEMS_PAGINA;
    public int Tamanho
    {
        get => _tamanho;
        set => _tamanho = value <= 0 ? QTD_ITEMS_PAGINA : value;
    }

    public int Skip() { return QTD_ITEMS_PAGINA * (_pagina - 1); }
    public int Take() { return Tamanho; }
}
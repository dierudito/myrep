using moreno.myrep.domain.Entities.Base;
using moreno.myrep.domain.Enums;

namespace moreno.myrep.domain.Entities;

public class DiaTrabalho (DateTime dataDiaTrabalho, MesTrabalho mesTrabalho) : Entity
{
    public DateTime DataDiaTrabalho { get; private set; } = dataDiaTrabalho;
    public string? Descricao { get; private set; }
    public string? Cliente { get; private set; }
    public Guid MesTrabalhoId { get; private set; } = mesTrabalho.Id;
    public MesTrabalho MesTrabalho { get; set; } = mesTrabalho;
    public IList<Apontamento> Apontamentos { get; private set; } = [];

    public void DefinirDescricao(string descricao) => Descricao = descricao;

    public void DefinirCliente(string cliente) => Cliente = cliente;

    public override async Task<bool> EhValido() => true;

    public async Task AdicionarApontamentoAsync(Apontamento apontamento)
    {
        if (!await apontamento.EhValido())
        {
            AdicionarErrosValidacao(apontamento.ValidationResult);
            return;
        }

        Apontamentos.Add(apontamento);
    }

    public async Task AdicionarApontamentoParaEtapaAsync(EEtapa eEtapa)
    {
        var apontamento = new Apontamento(
            CriarData(
                DataDiaTrabalho,
                eEtapa switch
                {
                    EEtapa.InicioDoDia => 9,
                    EEtapa.SaidaAlmoco => 12,
                    EEtapa.EntradaPosAlmoco => 13,
                    EEtapa.FimDoDia => 18
                }), eEtapa, this);

        await AdicionarApontamentoAsync(apontamento);
    }

    public async Task AdicionarApontamentoDoDiaAsync(DateTime dia)
    {
        if (!Apontamentos.Any()) await AdicionarApontamentoAsync(new(dia, EEtapa.InicioDoDia, this));
        var ultimaEtapa = Apontamentos.Max(a => a.Etapa);

        if (ultimaEtapa == EEtapa.FimDoDia)
        {
            AdicionarErroValidacao("", "Não é possível realizar mais apontamentos hoje");
            return;
        }

        var etapaAtual = ultimaEtapa + 1;
        await AdicionarApontamentoAsync(new(dia, etapaAtual, this));
    }

    public Apontamento? ObterUltimoApontamento()
    {
        if (!Apontamentos.Any()) return null;

        var ultimaHora = Apontamentos.Max(a => a.Hora);
        return Apontamentos.FirstOrDefault(a => a.Hora == ultimaHora);
    }

    private static DateTime CriarData(DateTime data, int hora) =>
        new(data.Year, data.Month, data.Day, hora, 0, 0);
}

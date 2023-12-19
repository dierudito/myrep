using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Enums;
using moreno.myrep.domain.Interfaces.Repositories;
using moreno.myrep.domain.Interfaces.Services;
using moreno.myrep.domain.Services.Base;
using moreno.myrep.domain.Validations.MesesTrabalho;

namespace moreno.myrep.domain.Services;

public class MesTrabalhoService(IMesTrabalhoRepository MesTrabalhoRepository) :
    BaseService<MesTrabalho>(MesTrabalhoRepository), IMesTrabalhoService
{
    public override async Task<MesTrabalho> Adicionar(MesTrabalho mesTrabalho)
    {
        if (!await mesTrabalho.EhValido()) return mesTrabalho;
        mesTrabalho.ValidationResult = await new MesTrabalhoEstaAptoParaCadastroValidation(MesTrabalhoRepository)
            .ValidateAsync(mesTrabalho);

        if (!mesTrabalho.ValidationResult.IsValid) return mesTrabalho;
        return await base.Adicionar(mesTrabalho);
    }

    public async Task<MesTrabalho> CriarApontamentoParaMesAsync(MesTrabalho mesTrabalho)
    {
        if (!await mesTrabalho.EhValido()) return mesTrabalho;

        var mesResult = await new MesTrabalhoEstaAptoParaCadastroValidation(MesTrabalhoRepository)
            .ValidateAsync(mesTrabalho);

        mesTrabalho.ValidationResult = mesResult;

        if(!mesTrabalho.ValidationResult.IsValid) return mesTrabalho;

        var dataTerminoDoMes = mesTrabalho.TerminoPeriodo;
        var dataInicioDoMes = mesTrabalho.InicioPeriodo;

        var diaAtual = DateTime.Now;
        if (dataTerminoDoMes > diaAtual) dataTerminoDoMes = diaAtual;

        for (int i = dataInicioDoMes.Day; i <= dataTerminoDoMes.Day; i++)
        {
            if (diaAtual is { DayOfWeek: DayOfWeek.Saturday or DayOfWeek.Sunday })
            {
                diaAtual = diaAtual.AddDays(1);
                continue;
            }
            var diaTrabalho = new DiaTrabalho(diaAtual, mesTrabalho);
            diaTrabalho.DefinirCliente("Banco CSF");
            diaTrabalho.DefinirDescricao("Apag");

            await diaTrabalho.AdicionarApontamentoParaEtapaAsync(EEtapa.InicioDoDia);
            await diaTrabalho.AdicionarApontamentoParaEtapaAsync(EEtapa.SaidaAlmoco);
            await diaTrabalho.AdicionarApontamentoParaEtapaAsync(EEtapa.EntradaPosAlmoco);
            await diaTrabalho.AdicionarApontamentoParaEtapaAsync(EEtapa.FimDoDia);

            await mesTrabalho.AdicionarDiaTrabalho(diaTrabalho);
            diaAtual = diaAtual.AddDays(1);
        }

        await MesTrabalhoRepository.Adicionar(mesTrabalho);
        await MesTrabalhoRepository.CommitAsync();

        return mesTrabalho;
    }
}
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;
using moreno.myrep.domain.Interfaces.Services;
using moreno.myrep.domain.Services.Base;

namespace moreno.myrep.domain.Services;

public class ApontamentoService(IApontamentoRepository ApontamentoRepository,
    IDiaTrabalhoRepository DiaTrabalhoRepository,
    IMesTrabalhoRepository MesTrabalhoRepository) :
    BaseService<Apontamento>(ApontamentoRepository), IApontamentoService
{
    public async Task<Apontamento> BaterPontoAsync(string? descricao, string? cliente)
    {
        var dataAtual = DateTime.Now;
        var apont = new Apontamento();

        var diaTrabalho = await DiaTrabalhoRepository.ObterPorDataAsync(dataAtual);

        if(diaTrabalho == null)
        {
            var mesTrabalho = await MesTrabalhoRepository.ObterPorDataAsync(dataAtual);

            if (mesTrabalho == null)
            {
                apont.AdicionarErroValidacao("", "Não foi encontrado o mês de trabalho para o apontamento de hoje. Faça o cadastro do mesmo");
                return apont;
            }

            diaTrabalho = new(dataAtual, mesTrabalho);

            if (!string.IsNullOrWhiteSpace(cliente)) diaTrabalho.DefinirCliente(cliente);
            if (!string.IsNullOrWhiteSpace(descricao)) diaTrabalho.DefinirDescricao(descricao);

            await DiaTrabalhoRepository.Adicionar(diaTrabalho);
        }

        await diaTrabalho.AdicionarApontamentoDoDiaAsync(dataAtual);

        if (!diaTrabalho.ValidationResult.IsValid)
        {
            apont.AdicionarErrosValidacao(diaTrabalho.ValidationResult);
            return apont;
        }

        var apontamento = diaTrabalho.ObterUltimoApontamento();

        if(apontamento.ValidationResult.IsValid)
            await ApontamentoRepository.CommitAsync();

        return apontamento;
    }
}


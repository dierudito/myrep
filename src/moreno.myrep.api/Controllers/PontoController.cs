using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moreno.myrep.api.Data;
using moreno.myrep.api.Models;
using moreno.myrep.api.Models.Dto;

namespace moreno.myrep.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PontoController(MyrepDbContext _context) : ControllerBase
    {
        [HttpPost("/baterponto")]
        public async Task<ActionResult<bool>> BaterPontoAsync([FromBody] BaterPontoRequest baterPontoRequest)
        {
            var dataAtual = DateTime.Now;
            var diaTrabalho = await _context.DiaTrabalho.FirstOrDefaultAsync(d => d.DataDiaTrabalho.Date == dataAtual.Date);

            if (diaTrabalho == null)
            {
                var mesTrabalho = await _context.MesTrabalho.FirstOrDefaultAsync(m => m.InicioPeriodo <= dataAtual && m.TerminoPeriodo >= dataAtual);

                if (mesTrabalho == null)
                    return BadRequest("Não foi encontrado o mês de trabalho para o apontamento de hoje. Faça o cadastro do mesmo");

                diaTrabalho = new()
                {
                    Cliente = baterPontoRequest.Cliente,
                    DataDiaTrabalho = dataAtual.Date,
                    Descricao = baterPontoRequest.Descricao,
                    MesTrabalhoId = mesTrabalho.Id
                };
                await _context.DiaTrabalho.AddAsync(diaTrabalho);
                await _context.SaveChangesAsync();
            }

            var apontamentos = _context.Apontamento.Where(a => a.DiaTrabalhoId == diaTrabalho.Id);
            var apontamentoAtual = new Apontamento
            {
                Hora = dataAtual,
                Etapa = EEtapa.InicioDoDia,
                DiaTrabalhoId = diaTrabalho.Id
            };

            if (apontamentos.Any())
            {
                var maxDate = apontamentos.Max(a => a.Hora);
                var ultimoApontamento = apontamentos.FirstOrDefault(a => a.Hora == maxDate);

                if (ultimoApontamento!.Etapa == EEtapa.FimDoDia)
                {
                    return NoContent();
                }

                apontamentoAtual.Etapa = (EEtapa)((int)ultimoApontamento.Etapa + 1);
            }

            apontamentoAtual.Hora = apontamentoAtual.Etapa switch
            {
                EEtapa.InicioDoDia or EEtapa.EntradaPosAlmoco =>
                    apontamentoAtual.Hora.AddSeconds((-1)* apontamentoAtual.Hora.Second),
                EEtapa.SaidaAlmoco or EEtapa.FimDoDia =>
                    apontamentoAtual.Hora.AddMinutes(1).AddSeconds((-1) * apontamentoAtual.Hora.Second),
                _ => apontamentoAtual.Hora
            };

            await _context.Apontamento.AddAsync(apontamentoAtual);
            await _context.SaveChangesAsync();

            return Ok(apontamentoAtual);
        }

        [HttpGet("/PlanilhaApontamento/{mes:int}")]
        public async Task<ActionResult<PlanilhaDeAtividadesRelacaoDiaResponse>> ObterPlanilhaApontamentoAsync([FromRoute] int mes)
        {
            var dataDoMes = new DateTime(DateTime.Now.Year, mes, 10);

            var mesCorrente =
                await _context.MesTrabalho.FirstOrDefaultAsync(mes => mes.InicioPeriodo <= dataDoMes && mes.TerminoPeriodo >= dataDoMes);

            if (mesCorrente == null)
                return BadRequest("Mês não encontrado");

            var diasDoMes = _context.DiaTrabalho.Where(dia => dia.MesTrabalhoId == mesCorrente.Id).ToList();

            if (!diasDoMes.Any())
                return BadRequest($"Não há apontamentos para o mês {dataDoMes:MMMM}");

            var response = new PlanilhaDeAtividadesResponse
            {
                Cliente = mesCorrente.Cliente,
                Consultoria = mesCorrente.Consultoria,
                PeriodoInicio = mesCorrente.InicioPeriodo,
                PeriodoTermino = mesCorrente.TerminoPeriodo
            };

            foreach (var diaDoMes in diasDoMes)
            {
                var apontamentos = _context.Apontamento.Where(apontamento => apontamento.DiaTrabalhoId == diaDoMes.Id).ToList();

                var tempoEtapaUm = apontamentos.FirstOrDefault(apontamento => apontamento.Etapa == EEtapa.SaidaAlmoco)!.Hora - apontamentos.FirstOrDefault(apontamento => apontamento.Etapa == EEtapa.InicioDoDia)!.Hora;

                var tempoEtapaDois = apontamentos.FirstOrDefault(apontamento => apontamento.Etapa == EEtapa.FimDoDia)!.Hora - apontamentos.FirstOrDefault(apontamento => apontamento.Etapa == EEtapa.EntradaPosAlmoco)!.Hora;

                var tempoTotal = tempoEtapaDois + tempoEtapaUm;

                var relacaoResponse = new PlanilhaDeAtividadesRelacaoDiaResponse
                {
                    Cliente = diaDoMes!.Cliente,
                    DescricaoDeAtividades = diaDoMes!.Descricao,
                    HorasDeServicoHora = tempoTotal.Hours,
                    HorasDeServicoMinuto = tempoTotal.Minutes
                };

                relacaoResponse.DefinirData(apontamentos.FirstOrDefault()!.Hora);
                relacaoResponse.DefinirDiaSemana(apontamentos.FirstOrDefault()!.Hora);

                response.RelacaoDia.Add(relacaoResponse);
            }

            response.RelacaoDia = response.RelacaoDia.OrderBy(r => r.Data).ToList();
            response.DefinirTotalHorasDeServico();

            return Ok(response);
        }

        [HttpPost("/criarMes/{mes:int}")]
        public async Task<ActionResult<bool>> CriarApontamentoParaMesAsync([FromRoute] int mes)
        {
            var dataInicioDoMes = new DateTime(DateTime.Now.Year, mes, 1);
            var dataTerminoDoMes = dataInicioDoMes.AddMonths(1).AddDays(-1);


            var dataDoMes = new DateTime(DateTime.Now.Year, mes, 10);

            var mesCorrente =
                await _context.MesTrabalho.FirstOrDefaultAsync(mes => mes.InicioPeriodo <= dataDoMes && mes.TerminoPeriodo >= dataDoMes);

            if (mesCorrente != null)
                return BadRequest("Mês já existe");

            var mesTrabalho = new MesTrabalho
            {
                Cliente = "Banco CSF",
                Consultoria = "OPAH",
                PrestadorServico = "Diego Ferreira Moreno",
                InicioPeriodo = dataInicioDoMes,
                TerminoPeriodo = dataTerminoDoMes
            };

            await _context.MesTrabalho.AddAsync(mesTrabalho);
            await _context.SaveChangesAsync();

            var diaAtual = dataInicioDoMes;

            if (dataTerminoDoMes > DateTime.Now) dataTerminoDoMes = DateTime.Now;

            for (int i = dataInicioDoMes.Day; i <= dataTerminoDoMes.Day; i++)
            {
                if (diaAtual is { DayOfWeek: DayOfWeek.Saturday or DayOfWeek.Sunday})
                {
                    diaAtual = diaAtual.AddDays(1);
                    continue;
                }
                var diaTrabalho = new DiaTrabalho
                {
                    Cliente = "Banco CSF",
                    Descricao = "apag",
                    DataDiaTrabalho = diaAtual,
                    MesTrabalhoId = mesTrabalho.Id
                };

                await _context.DiaTrabalho.AddAsync(diaTrabalho);
                await _context.SaveChangesAsync();

                var inicioTrabalho = new DateTime(diaAtual.Year, diaAtual.Month, diaAtual.Day, 9, 0, 0);
                var saidaAlmoco = new DateTime(diaAtual.Year, diaAtual.Month, diaAtual.Day, 12, 0, 0);
                var retornoAlmoco = new DateTime(diaAtual.Year, diaAtual.Month, diaAtual.Day, 13, 0, 0);
                var saidaTrabalho = new DateTime(diaAtual.Year, diaAtual.Month, diaAtual.Day, 18, 0, 0);

                await _context.Apontamento.AddAsync(new()
                {
                    DiaTrabalhoId = diaTrabalho.Id,
                    Etapa = EEtapa.InicioDoDia,
                    Hora = inicioTrabalho
                });

                await _context.Apontamento.AddAsync(new()
                {
                    DiaTrabalhoId = diaTrabalho.Id,
                    Etapa = EEtapa.SaidaAlmoco,
                    Hora = saidaAlmoco
                });

                await _context.Apontamento.AddAsync(new()
                {
                    DiaTrabalhoId = diaTrabalho.Id,
                    Etapa = EEtapa.EntradaPosAlmoco,
                    Hora = retornoAlmoco
                });

                await _context.Apontamento.AddAsync(new()
                {
                    DiaTrabalhoId = diaTrabalho.Id,
                    Etapa = EEtapa.FimDoDia,
                    Hora = saidaTrabalho
                });
                await _context.SaveChangesAsync();
                diaAtual = diaAtual.AddDays(1);
            }

            return Ok(true);
        }

        // GET: api/Ponto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MesTrabalho>>> GetMesTrabalho()
        {
            return await _context.MesTrabalho.ToListAsync();
        }

        // GET: api/Ponto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MesTrabalho>> GetMesTrabalho(Guid id)
        {
            var mesTrabalho = await _context.MesTrabalho.FindAsync(id);

            if (mesTrabalho == null)
            {
                return NotFound();
            }

            return mesTrabalho;
        }

        // PUT: api/Ponto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMesTrabalho(Guid id, MesTrabalho mesTrabalho)
        {
            if (id != mesTrabalho.Id)
            {
                return BadRequest();
            }

            _context.Entry(mesTrabalho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MesTrabalhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ponto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MesTrabalho>> PostMesTrabalho(MesTrabalho mesTrabalho)
        {
            _context.MesTrabalho.Add(mesTrabalho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMesTrabalho", new { id = mesTrabalho.Id }, mesTrabalho);
        }

        // DELETE: api/Ponto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesTrabalho(Guid id)
        {
            var mesTrabalho = await _context.MesTrabalho.FindAsync(id);
            if (mesTrabalho == null)
            {
                return NotFound();
            }

            _context.MesTrabalho.Remove(mesTrabalho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MesTrabalhoExists(Guid id)
        {
            return _context.MesTrabalho.Any(e => e.Id == id);
        }
    }
}

namespace moreno.myrep.api.Models.Dto
{
    public class PlanilhaDeAtividadesResponse
    {
        public string PrestradorServico { get; set; }
        public string Consultoria { get; set; }
        public string Cliente { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoTermino { get; set; }
        public List<PlanilhaDeAtividadesRelacaoDiaResponse> RelacaoDia { get; set; } = [];
        public string TotalHorasDeServico { get; set; }

        public void DefinirTotalHorasDeServico()
        {
            int horas = 0, minutos = 0;

            foreach (var item in RelacaoDia)
            {
                minutos += item.HorasDeServicoMinuto;
                horas += item.HorasDeServicoHora;

                if (minutos >= 60)
                {
                    while (minutos >= 60)
                    {
                        minutos -= 60;
                        horas++;
                    }
                }
            }

            TotalHorasDeServico = $"{horas}:{minutos.ToString().PadLeft(2, '0')}";
        }
    }

    public class PlanilhaDeAtividadesRelacaoDiaResponse
    {
        public string Data { get; set; }
        public string DiaSemana { get; set; }
        public string HorasDeServico =>
            $"{HorasDeServicoHora.ToString().PadLeft(2, '0')}:{HorasDeServicoMinuto.ToString().PadLeft(2, '0')}";
        public int HorasDeServicoHora { get; set; }
        public int HorasDeServicoMinuto { get; set; }
        public string DescricaoDeAtividades { get; set; }
        public string Cliente { get; set; }

        public void DefinirData(DateTime data)
        {
            Data = data.ToString("dd-MMMM");
        }

        public void DefinirDiaSemana(DateTime data)
        {
            DiaSemana = data.ToString("ddd");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_Plazos
{
    public class Items
    {
        public int id { get; set; }
        public string contribuyente { get; set; }
        public string fecha_inicio { get; set; }
        public string plazo_extendido { get; set; }
        public string entrega_expediente { get; set; }
        public string comite { get; set; }
        public string notificacion_atenta { get; set; }
        public string vencimiento_10_dias { get; set; }
        public string entrega_borrador_uap { get; set; }
        public string entrega_uap { get; set; }
        public string recepcion_uap { get; set; }
        public string sengunda_vuelta_uap { get; set; }
        public string levantamiento_uap { get; set; }
        public string vence_uap { get; set; }
        public string levantamiento_acta_final { get; set; }
        public string dias_estrados { get; set; }
        public string vence_plazo { get; set; }
        public string dias_sobrantes { get; set; }
        public string liquidacion { get; set; }
        public string entrega_borrador_liquidacion { get; set; }
        public string entrega_liquidacion { get; set; }
        public string recepcion_liquidacion { get; set; }
        public string segunda_vuelta_liquidacion { get; set; }
        public string impresion_firma { get; set; }
        public string fecha_cierre { get; set; }
        public string plazo_prodecon { get; set; }
    }
}

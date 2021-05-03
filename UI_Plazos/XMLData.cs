using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UI_Plazos
{
    class XMLData
    {
        XmlDocument document = new XmlDocument();
        static string path = @"PlazosVisitaDomiciliaria.xml";

        public XmlNode Add(int _index, string _contribuyente, string _fecha_inicio, string _plazo_extendido,
            string _entrega_expediente, string _comite, string _notificacion_atenta, string _vencimiento_10_dias,
            string _entrega_borrador_uap, string _entrega_uap, string _recepcion_uap, string _sengunda_vuelta_uap,
            string _levantamiento_uap, string _vence_uap, string _levantamiento_acta_final, string _dias_estrados,
            string _vence_plazo, string _dias_sobrantes, string _liquidacion, string _entrega_borrador_liquidacion,
            string _entrega_liquidacion, string _recepcion_liquidacion, string _segunda_vuelta_liquidacion,
            string _impresion_firma, string _fecha_cierre, string _plazo_prodecon)
        {
            document.Load(path);
            XmlNode _contribuyente_ = document.CreateElement("Contribuyente");

            XmlNode id = document.CreateElement("Id");
            id.InnerText = Convert.ToString(_index);
            XmlNode contribuyente = document.CreateElement("Contribuyente");
            contribuyente.InnerText = _contribuyente;
            XmlNode fecha_inicio = document.CreateElement("FechaInicio");
            fecha_inicio.InnerText = _fecha_inicio;
            XmlNode plazo_extendido = document.CreateElement("PlazoExtendido");
            plazo_extendido.InnerText = _plazo_extendido;
            XmlNode entrega_expediente = document.CreateElement("EntregaExpediente");
            entrega_expediente.InnerText = _entrega_expediente;
            XmlNode comite = document.CreateElement("Comite");
            comite.InnerText = _comite;
            XmlNode notificacion_atenta = document.CreateElement("NotificacionAtenta");
            notificacion_atenta.InnerText = _notificacion_atenta;
            XmlNode vencimiento_10_dias = document.CreateElement("Vencimiento10Dias");
            vencimiento_10_dias.InnerText = _vencimiento_10_dias;
            XmlNode entrega_borrador_uap = document.CreateElement("EntregaBorradorUAP");
            entrega_borrador_uap.InnerText = _entrega_borrador_uap;
            XmlNode entrega_uap = document.CreateElement("EntregaUAP");
            entrega_uap.InnerText = _entrega_uap;
            XmlNode recepcion_uap = document.CreateElement("RecepcionUAP");
            recepcion_uap.InnerText = _recepcion_uap;
            XmlNode sengunda_vuelta_uap = document.CreateElement("SengundaVueltaUAP");
            sengunda_vuelta_uap.InnerText = _sengunda_vuelta_uap;
            XmlNode levantamiento_uap = document.CreateElement("LevantamientoUAP");
            levantamiento_uap.InnerText = _levantamiento_uap;
            XmlNode vence_uap = document.CreateElement("VenceUAP");
            vence_uap.InnerText = _vence_uap;
            XmlNode levantamiento_acta_final = document.CreateElement("LevantamientoActaFinal");
            levantamiento_acta_final.InnerText = _levantamiento_acta_final;
            XmlNode dias_estrados = document.CreateElement("DiasEstrados");
            dias_estrados.InnerText = _dias_estrados;
            XmlNode vence_plazo = document.CreateElement("VencePlazo");
            vence_plazo.InnerText = _vence_plazo;
            XmlNode dias_sobrantes = document.CreateElement("DiasSobrantes");
            dias_sobrantes.InnerText = _dias_sobrantes;
            XmlNode liquidacion = document.CreateElement("Liquidacion");
            liquidacion.InnerText = _liquidacion;
            XmlNode entrega_borrador_liquidacion = document.CreateElement("EntregaBorradorLiquidacion");
            entrega_borrador_liquidacion.InnerText = _entrega_borrador_liquidacion;
            XmlNode entrega_liquidacion = document.CreateElement("EntregaLiquidacion");
            entrega_liquidacion.InnerText = _entrega_liquidacion;
            XmlNode recepcion_liquidacion = document.CreateElement("RecepcionLiquidacion");
            recepcion_liquidacion.InnerText = _recepcion_liquidacion;
            XmlNode segunda_vuelta_liquidacion = document.CreateElement("SegundaVueltaLiquidacion");
            segunda_vuelta_liquidacion.InnerText = _segunda_vuelta_liquidacion;
            XmlNode impresion_firma = document.CreateElement("ImpresionFirma");
            impresion_firma.InnerText = _impresion_firma;
            XmlNode fecha_cierre = document.CreateElement("FechaCierre");
            fecha_cierre.InnerText = _fecha_cierre;
            XmlNode plazo_prodecon = document.CreateElement("PlazoProdecon");
            plazo_prodecon.InnerText = _plazo_prodecon;

            _contribuyente_.AppendChild(id);
            _contribuyente_.AppendChild(contribuyente);
            _contribuyente_.AppendChild(fecha_inicio);
            _contribuyente_.AppendChild(plazo_extendido);
            _contribuyente_.AppendChild(entrega_expediente);
            _contribuyente_.AppendChild(comite);
            _contribuyente_.AppendChild(notificacion_atenta);
            _contribuyente_.AppendChild(vencimiento_10_dias);
            _contribuyente_.AppendChild(entrega_borrador_uap);
            _contribuyente_.AppendChild(entrega_uap);
            _contribuyente_.AppendChild(recepcion_uap);
            _contribuyente_.AppendChild(sengunda_vuelta_uap);
            _contribuyente_.AppendChild(levantamiento_uap);
            _contribuyente_.AppendChild(vence_uap);
            _contribuyente_.AppendChild(levantamiento_acta_final);
            _contribuyente_.AppendChild(dias_estrados);
            _contribuyente_.AppendChild(vence_plazo);
            _contribuyente_.AppendChild(dias_sobrantes);
            _contribuyente_.AppendChild(liquidacion);
            _contribuyente_.AppendChild(entrega_borrador_liquidacion);
            _contribuyente_.AppendChild(entrega_liquidacion);
            _contribuyente_.AppendChild(recepcion_liquidacion);
            _contribuyente_.AppendChild(segunda_vuelta_liquidacion);
            _contribuyente_.AppendChild(impresion_firma);
            _contribuyente_.AppendChild(fecha_cierre);
            _contribuyente_.AppendChild(plazo_prodecon);

            document.SelectSingleNode("VisitaDomiciliaria").AppendChild(_contribuyente_);
            document.Save(path);
            return _contribuyente_;
        }

        public List<Items> Read()
        {
            List<Items> item = new List<Items>();
            document.Load(path);

            foreach (XmlNode unContribuyente in document.DocumentElement.ChildNodes)
            {
                item.Add(new Items
                {
                    id = Convert.ToInt32(unContribuyente.SelectSingleNode("Id").InnerText),
                    contribuyente = unContribuyente.SelectSingleNode("Contribuyente").InnerText,
                    fecha_inicio = unContribuyente.SelectSingleNode("FechaInicio").InnerText,
                    plazo_extendido = unContribuyente.SelectSingleNode("PlazoExtendido").InnerText,
                    entrega_expediente = unContribuyente.SelectSingleNode("EntregaExpediente").InnerText,
                    comite = unContribuyente.SelectSingleNode("Comite").InnerText,
                    notificacion_atenta = unContribuyente.SelectSingleNode("NotificacionAtenta").InnerText,
                    vencimiento_10_dias = unContribuyente.SelectSingleNode("Vencimiento10Dias").InnerText,
                    entrega_borrador_uap = unContribuyente.SelectSingleNode("EntregaBorradorUAP").InnerText,
                    entrega_uap = unContribuyente.SelectSingleNode("EntregaUAP").InnerText,
                    recepcion_uap = unContribuyente.SelectSingleNode("RecepcionUAP").InnerText,
                    sengunda_vuelta_uap = unContribuyente.SelectSingleNode("SengundaVueltaUAP").InnerText,
                    levantamiento_uap = unContribuyente.SelectSingleNode("LevantamientoUAP").InnerText,
                    vence_uap = unContribuyente.SelectSingleNode("VenceUAP").InnerText,
                    levantamiento_acta_final = unContribuyente.SelectSingleNode("LevantamientoActaFinal").InnerText,
                    dias_estrados = unContribuyente.SelectSingleNode("DiasEstrados").InnerText,
                    vence_plazo = unContribuyente.SelectSingleNode("VencePlazo").InnerText,
                    dias_sobrantes = unContribuyente.SelectSingleNode("DiasSobrantes").InnerText,
                    liquidacion = unContribuyente.SelectSingleNode("Liquidacion").InnerText,
                    entrega_borrador_liquidacion = unContribuyente.SelectSingleNode("EntregaBorradorLiquidacion").InnerText,
                    entrega_liquidacion = unContribuyente.SelectSingleNode("EntregaLiquidacion").InnerText,
                    recepcion_liquidacion = unContribuyente.SelectSingleNode("RecepcionLiquidacion").InnerText,
                    segunda_vuelta_liquidacion = unContribuyente.SelectSingleNode("SegundaVueltaLiquidacion").InnerText,
                    impresion_firma = unContribuyente.SelectSingleNode("ImpresionFirma").InnerText,
                    fecha_cierre = unContribuyente.SelectSingleNode("FechaCierre").InnerText,
                    plazo_prodecon = unContribuyente.SelectSingleNode("PlazoProdecon").InnerText
                });
            }
            return item;
        }

        public int ReadIndex()
        {
            document.Load(path);
            XmlNodeList list = document.GetElementsByTagName("Id");
            int count = list.Count;
            return count;
        }

        public List<AddLabel> ReadVencimientoPlazos()
        {
            List<AddLabel> dates = new List<AddLabel>();
            document.Load(path);
            foreach (XmlNode item in document.DocumentElement.ChildNodes)
            {
                dates.Add(new AddLabel
                {
                    contribuyente = item.SelectSingleNode("Contribuyente").InnerText,
                    vence_plazo = Convert.ToDateTime(item.SelectSingleNode("VencePlazo").InnerText)
                });
            }
            return dates;
        }

        public void Update(int _index, string _contribuyente, string _fecha_inicio, string _plazo_extendido,
            string _entrega_expediente, string _comite, string _notificacion_atenta, string _vencimiento_10_dias,
            string _entrega_borrador_uap, string _entrega_uap, string _recepcion_uap, string _sengunda_vuelta_uap,
            string _levantamiento_uap, string _vence_uap, string _levantamiento_acta_final, string _dias_estrados,
            string _vence_plazo, string _dias_sobrantes, string _liquidacion, string _entrega_borrador_liquidacion,
            string _entrega_liquidacion, string _recepcion_liquidacion, string _segunda_vuelta_liquidacion,
            string _impresion_firma, string _fecha_cierre, string _plazo_prodecon)
        {
            document.Load(path);
            XmlNode nuevoContribuyente = Add(_index, _contribuyente, _fecha_inicio, _plazo_extendido, _entrega_expediente, 
                _comite, _notificacion_atenta, _vencimiento_10_dias, _entrega_borrador_uap, _entrega_uap, _recepcion_uap,
                _sengunda_vuelta_uap, _levantamiento_uap, _vence_uap, _levantamiento_acta_final, _dias_estrados, 
                _vence_plazo, _dias_sobrantes, _liquidacion, _entrega_borrador_liquidacion, _entrega_liquidacion, 
                _recepcion_liquidacion, _segunda_vuelta_liquidacion, _impresion_firma, _fecha_cierre, _plazo_prodecon);

            foreach (XmlNode item in document.DocumentElement.ChildNodes)
            {
                if (item.FirstChild.InnerText == Convert.ToString(_index))
                {
                    XmlNode ViejoContribuyente = item;
                    document.DocumentElement.ReplaceChild(nuevoContribuyente, ViejoContribuyente);
                }
            }
            document.Save(path);
        }

        public void Delete(int _index)
        {
            document.Load(path);

            foreach (XmlNode item in document.DocumentElement.ChildNodes)
            {
                if (item.SelectSingleNode("Id").InnerText == Convert.ToString(_index))
                {
                    XmlNode ViejoContribuyente = item;
                    document.DocumentElement.RemoveChild(ViejoContribuyente);
                }
            }
            document.Save(path);
        }
    }

    public class AddLabel
    {
        public string contribuyente;
        public DateTime vence_plazo;
    }
}

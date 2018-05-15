namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public class Jurnal200 : JurnalBase
    {
        /// <summary>
        /// Тип журнала - "AntiBot/2FA/IPtables"
        /// </summary>
        public TypeJurn200 typeJurn { get; set; }  = TypeJurn200.Unknown;
    }
}

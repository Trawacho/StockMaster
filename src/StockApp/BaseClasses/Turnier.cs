namespace StockApp.BaseClasses
{
    public class Turnier : TBaseClass
    {
        /// <summary>
        /// Organisatorische Daten jedes Turniers
        /// </summary>
        public OrgaDaten OrgaDaten { get; set; }


        private TBaseBewerb _wettbewerb;
        /// <summary>
        /// Kann ein <see cref="TeamBewerb"/> oder <see cref="Zielbewerb"/> sein
        /// </summary>
        public TBaseBewerb Wettbewerb
        {
            get
            {
                return _wettbewerb;
            }
            set
            {
                if (value == _wettbewerb)
                    return;

                _wettbewerb = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Default Konstruktor
        /// </summary>
        public Turnier()
        {
            this.OrgaDaten = new OrgaDaten();
        }
    }
}

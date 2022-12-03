namespace Battleships
{
    public class Coordinate
    {
        #region Properties

        public double Xpos { get; set; }
        public double Ypos { get; set; }

        #endregion

        #region Constructor

        public Coordinate(double xpos, double ypos)
        {
            Xpos = xpos;
            Ypos = ypos;
        }

        #endregion

        #region Methods

        public bool Compare(Coordinate other)
        {
            return Xpos == other.Xpos && Ypos == other.Ypos;
        } 

        #endregion
    }
}

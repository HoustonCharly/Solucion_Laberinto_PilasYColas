namespace WFmatGrid
{
    public class CoordinateAndValue
    {
        int x, y;
        int v;
        public CoordinateAndValue(int x, int y, int value)
        {
            this.x = x;
            this.y = y;
            v = value;
        }
        public int posX()
        {
            return this.x;
        }
        public int posY()
        {
            return this.y;
        }
        public int value()
        {
            return this.v;
        }
        public CoordinateAndValue Arriba()
        {
            return new CoordinateAndValue(this.x - 1, this.y, this.v);
        }
        public CoordinateAndValue Abajo()
        {
            return new CoordinateAndValue(this.x + 1, this.y, this.v);
        }
        public CoordinateAndValue Derecha()
        {
            return new CoordinateAndValue(this.x, this.y + 1, this.v);
        }
        public CoordinateAndValue Izquierda()
        {
            return new CoordinateAndValue(this.x, this.y - 1, this.v);
        }
        public static bool operator ==(CoordinateAndValue c1, CoordinateAndValue c2) => c1.v == c2.v && c1.y == c2.y && c1.x == c2.x;
        public static bool operator !=(CoordinateAndValue c1, CoordinateAndValue c2) => c1.v != c2.v || c1.y != c2.y || c1.x != c2.x;

    }

}

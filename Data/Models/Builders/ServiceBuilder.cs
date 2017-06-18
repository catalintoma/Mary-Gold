namespace Marigold
{
    //Generic, so we can also use it for rooms
    public class ServiceBuilder<T> where T : Service, new()
    {
        private T _output;

        public ServiceBuilder()
        {
            _output = new T();
        }

        public ServiceBuilder<T> WithId(string id)
        {
            _output.ServiceId = id;
            return this;
        }

        public ServiceBuilder<T> WithUnit(Unit unit)
        {
            _output.Unit = unit;
            return this;
        }

        public ServiceBuilder<T> Priced(int price)
        {
            _output.UnitPrice = price;
            return this;
        }

        public ServiceBuilder<T> Named(string name)
        {
            _output.Name = name;
            return this;
        }

        public T Create()
        {
            return _output;
        }
    }
}
namespace TestNinja.Mocking {
    public interface IEmployeeStorage {
        void Delete(int id);
    }

    public class EmployeeStorage : IEmployeeStorage {
        private EmployeeContext _db;

        public EmployeeStorage() {
            _db = new EmployeeContext();
        }

        // We don't do unit test for this method, because here 
        // we're working directly with external resource. So 
        // the proper way to test this is using integration test
        public void Delete(int id) {
            var employee = _db.Employees.Find(id);
            if (employee != null) {
                _db.Employees.Remove(employee);
                _db.SaveChanges();
            }
        }
    }
}
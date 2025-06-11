import { useState, useEffect } from 'react';
import { apiService, Client, Car, Mechanic, Task, Report } from './api/api';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import axios from 'axios';

function App() {
  const [tab, setTab] = useState('clients');
  const [clients, setClients] = useState<Client[]>([]);
  const [cars, setCars] = useState<Car[]>([]);
  const [mechanics, setMechanics] = useState<Mechanic[]>([]);
  const [tasks, setTasks] = useState<Task[]>([]);
  const [reports, setReports] = useState<Report[]>([]);
  const [error, setError] = useState<string | null>(null);

  // Форми
  const [clientName, setClientName] = useState('');
  const [clientPhone, setClientPhone] = useState('');
  const [carClientId, setCarClientId] = useState('');
  const [carModel, setCarModel] = useState('');
  const [carLicensePlate, setCarLicensePlate] = useState('');
  const [carLat, setCarLat] = useState('');
  const [carLng, setCarLng] = useState('');
  const [mechanicName, setMechanicName] = useState('');
  const [taskCarId, setTaskCarId] = useState('');
  const [taskDescription, setTaskDescription] = useState('');
  const [taskMechanicId, setTaskMechanicId] = useState('');
  const [reportTaskId, setReportTaskId] = useState('');
  const [reportDescription, setReportDescription] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const clientsData = await apiService.getClients();
        const carsData = await apiService.getCars();
        const mechanicsData = await apiService.getMechanics();
        const tasksData = await apiService.getTasks();

        setClients(Array.isArray(clientsData) ? clientsData : []);
        setCars(Array.isArray(carsData) ? carsData : []);
        setMechanics(Array.isArray(mechanicsData) ? mechanicsData : []);
        setTasks(Array.isArray(tasksData) ? tasksData : []);
      } catch (err: unknown) {
        const message = err instanceof Error ? err.message : 'Unknown Error';
        setError(message);
      }
    };
    fetchData();
  }, []);

  const handleCreateClient = async () => {
    try {
      setError(null);
      if (!clientName) throw new Error('CLients name is neccessary');
      const res = await apiService.createClient({ name: clientName, phone: clientPhone });
      setClients([...clients, res]);
      setClientName('');
      setClientPhone('');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Unknown Error';
      setError(message);
    }
  };

  const handleCreateCar = async () => {
    try {
      setError(null);
      if (!carClientId || !carModel || !carLicensePlate) {
        throw new Error('Client model and number must be entered');
      }
      const res = await apiService.createCar({
        clientId: Number(carClientId),
        model: carModel,
        licensePlate: carLicensePlate,
        locationLat: carLat ? Number(carLat) : 0,
        locationLng: carLng ? Number(carLng) : 0,
      });
      setCars([...cars, res]);
      setCarClientId('');
      setCarModel('');
      setCarLicensePlate('');
      setCarLat('');
      setCarLng('');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Unknown Error';
      setError(message);
    }
  };

  const handleCreateMechanic = async () => {
    try {
      setError(null);
      if (!mechanicName) throw new Error('Mechanics name must be entered');
      const res = await apiService.createMechanic({ name: mechanicName, isAvailable: true });
      setMechanics([...mechanics, res]);
      setMechanicName('');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Unknown Error';
      setError(message);
    }
  };

  const handleCreateTask = async () => {
    try {
      setError(null);
      if (!taskCarId) throw new Error('Car is neccessary');
      if (!taskDescription.trim()) throw new Error('Description must be entered');

      const data = {
        carId: Number(taskCarId),
        description: taskDescription.trim(),
        mechanicId: taskMechanicId ? Number(taskMechanicId) : null,
      };
      console.log('Sending task data:', data); // Логування даних
      const res = await apiService.createTask(data);
      setTasks([...tasks, res]);
      setTaskCarId('');
      setTaskDescription('');
      setTaskMechanicId('');
    } catch (err: unknown) {
      let message = 'Unknown Error';
      if (axios.isAxiosError(err)) {
        const errors = err.response?.data?.errors || err.response?.data || err.message;
        message = typeof errors === 'object' ? JSON.stringify(errors) : errors;
      } else if (err instanceof Error) {
        message = err.message;
      }
      setError('Error creating task: ' + message);
    }
  };

  const handleCreateReport = async () => {
    try {
      setError(null);
      if (!reportTaskId || !reportDescription) {
        throw new Error('Task and description are neccessary');
      }
      const res = await apiService.createReport({
        taskId: Number(reportTaskId),
        description: reportDescription,
      });
      setReports([...reports, res]);
      setReportTaskId('');
      setReportDescription('');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Unknown Error';
      setError(message);
    }
  };

  return (
    <div className="container mt-4">
      <h1 className="text-center mb-4">AutoSelect</h1>
      {error && <div className="alert alert-danger">{error}</div>}
      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <button className={`nav-link ${tab === 'clients' ? 'active' : ''}`} onClick={() => setTab('clients')}>
            Clients
          </button>
        </li>
        <li className="nav-item">
          <button className={`nav-link ${tab === 'cars' ? 'active' : ''}`} onClick={() => setTab('cars')}>
            Cars
          </button>
        </li>
        <li className="nav-item">
          <button className={`nav-link ${tab === 'mechanics' ? 'active' : ''}`} onClick={() => setTab('mechanics')}>
            Mechanics
          </button>
        </li>
        <li className="nav-item">
          <button className={`nav-link ${tab === 'tasks' ? 'active' : ''}`} onClick={() => setTab('tasks')}>
            Tasks
          </button>
        </li>
        <li className="nav-item">
          <button className={`nav-link ${tab === 'reports' ? 'active' : ''}`} onClick={() => setTab('reports')}>
            Raports
          </button>
        </li>
      </ul>

      {tab === 'clients' && (
        <div>
          <h2>Clients</h2>
          <div className="mb-3">
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Name"
              value={clientName}
              onChange={(e) => setClientName(e.target.value)}
            />
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Phone number"
              value={clientPhone}
              onChange={(e) => setClientPhone(e.target.value)}
            />
            <button className="btn btn-primary" onClick={handleCreateClient}>
              Add Client
            </button>
          </div>
          {clients.length === 0 ? (
            <p>No clients</p>
          ) : (
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                  <th>Phone number</th>
                </tr>
              </thead>
              <tbody>
                {clients.map((client) => (
                  <tr key={client.id}>
                    <td>{client.id}</td>
                    <td>{client.name}</td>
                    <td>{client.phone || '-'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}

      {tab === 'cars' && (
        <div>
          <h2>Cars</h2>
          <div className="mb-3">
            <select
              className="form-select mb-2"
              value={carClientId}
              onChange={(e) => setCarClientId(e.target.value)}
            >
              <option value="">Choose client</option>
              {clients.map((client) => (
                <option key={client.id} value={client.id}>
                  {client.name}
                </option>
              ))}
            </select>
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Model"
              value={carModel}
              onChange={(e) => setCarModel(e.target.value)}
            />
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Car plate"
              value={carLicensePlate}
              onChange={(e) => setCarLicensePlate(e.target.value)}
            />
            <input
              type="number"
              className="form-control mb-2"
              placeholder="Width"
              value={carLat}
              onChange={(e) => setCarLat(e.target.value)}
            />
            <input
              type="number"
              className="form-control mb-2"
              placeholder="Height"
              value={carLng}
              onChange={(e) => setCarLng(e.target.value)}
            />
            <button className="btn btn-primary" onClick={handleCreateCar}>
              Add car
            </button>
          </div>
          {cars.length === 0 ? (
            <p>No cars...</p>
          ) : (
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Model</th>
                  <th>Nmbers</th>
                  <th>Client ID</th>
                  <th>Coordinates</th>
                </tr>
              </thead>
              <tbody>
                {cars.map((car) => (
                  <tr key={car.id}>
                    <td>{car.id}</td>
                    <td>{car.model}</td>
                    <td>{car.licensePlate}</td>
                    <td>{car.clientId}</td>
                    <td>
                      {car.locationLat}, {car.locationLng}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}

      {tab === 'mechanics' && (
        <div>
          <h2>Mechanics</h2>
          <div className="mb-3">
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Name"
              value={mechanicName}
              onChange={(e) => setMechanicName(e.target.value)}
            />
            <button className="btn btn-primary" onClick={handleCreateMechanic}>
              Add mechanic
            </button>
          </div>
          {mechanics.length === 0 ? (
            <p>No workers...</p>
          ) : (
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {mechanics.map((mechanic) => (
                  <tr key={mechanic.id}>
                    <td>{mechanic.id}</td>
                    <td>{mechanic.name}</td>
                    <td>{mechanic.isAvailable ? 'Free' : 'Busy'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}

      {tab === 'tasks' && (
        <div>
          <h2>Task</h2>
          <div className="mb-3">
            <select
              className="form-select mb-2"
              value={taskCarId}
              onChange={(e) => setTaskCarId(e.target.value)}
            >
              <option value="">Choose car</option>
              {cars.map((car) => (
                <option key={car.id} value={car.id}>
                  {car.model} (ID: {car.id})
                </option>
              ))}
            </select>
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Description"
              value={taskDescription}
              onChange={(e) => setTaskDescription(e.target.value)}
            />
            <select
              className="form-select mb-2"
              value={taskMechanicId}
              onChange={(e) => setTaskMechanicId(e.target.value)}
            >
              <option value="">Choose mechanic (or not)</option>
              {mechanics.map((mechanic) => (
                <option key={mechanic.id} value={mechanic.id}>
                  {mechanic.name} ({mechanic.isAvailable ? 'Free' : 'Busy'})
                </option>
              ))}
            </select>
            <button className="btn btn-primary" onClick={handleCreateTask}>
              Add task
            </button>
          </div>
          {tasks.length === 0 ? (
            <p>No tasks...</p>
          ) : (
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Description</th>
                  <th>Car</th>
                  <th>Mechanic</th>
                  <th>Status</th>
                  <th>Date</th>
                </tr>
              </thead>
              <tbody>
                {tasks.map((task) => (
                  <tr key={task.id}>
                    <td>{task.id}</td>
                    <td>{task.description}</td>
                    <td>{task.car?.model || '-'}</td>
                    <td>{task.mechanic?.name || '-'}</td>
                    <td>{task.status}</td>
                    <td>{new Date(task.createdAt).toLocaleDateString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}

      {tab === 'reports' && (
        <div>
          <h2>Reports</h2>
          <div className="mb-3">
            <select
              className="form-select mb-2"
              value={reportTaskId}
              onChange={(e) => setReportTaskId(e.target.value)}
            >
              <option value="">Choose task</option>
              {tasks.map((task) => (
                <option key={task.id} value={task.id}>
                  {task.description}
                </option>
              ))}
            </select>
            <input
              type="text"
              className="form-control mb-2"
              placeholder="Description"
              value={reportDescription}
              onChange={(e) => setReportDescription(e.target.value)}
            />
            <button className="btn btn-primary" onClick={handleCreateReport}>
              Add raport
            </button>
          </div>
          {reports.length === 0 ? (
            <p>No raports</p>
          ) : (
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Description</th>
                  <th>Task ID</th>
                  <th>Complete date</th>
                </tr>
              </thead>
              <tbody>
                {reports.map((report) => (
                  <tr key={report.id}>
                    <td>{report.id}</td>
                    <td>{report.description}</td>
                    <td>{report.taskId}</td>
                    <td>{new Date(report.completedAt).toLocaleDateString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}
    </div>
  );
}

export default App;
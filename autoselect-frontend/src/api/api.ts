import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5623/api',
});

interface ApiResponse<T> {
  $id?: string;
  $values: T[];
}

export interface Client {
  id: number;
  name: string;
  phone?: string;
  cars?: any;
}

export interface Car {
  id: number;
  clientId: number;
  model: string;
  licensePlate: string;
  locationLat: number;
  locationLng: number;
}

export interface Mechanic {
  id: number;
  name: string;
  isAvailable: boolean;
}

export interface Task {
  id: number;
  mechanicId: number;
  mechanic: Mechanic;
  carId: number;
  car: Car;
  description: string;
  status: string;
  createdAt: string;
}

export interface Report {
  id: number;
  taskId: number;
  description: string;
  completedAt: string;
}

export const apiService = {
  getClients: async () => {
    try {
      const response = await api.get<ApiResponse<Client>>('/clients');
      return response.data.$values || [];
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Can nit access client: ' + message);
    }
  },
  createClient: async (data: { name: string; phone?: string }) => {
    try {
      const response = await api.post<Client>('/clients', data);
      return response.data;
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Error reating client: ' + message);
    }
  },
  getCars: async () => {
    try {
      const response = await api.get<ApiResponse<Car>>('/cars');
      return response.data.$values || [];
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Can not access cars: ' + message);
    }
  },
  createCar: async (data: { clientId: number; model: string; licensePlate: string; locationLat?: number; locationLng?: number }) => {
    try {
      const response = await api.post<Car>('/cars/create', data);
      return response.data;
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Error creating car: ' + message);
    }
  },
  getMechanics: async () => {
    try {
      const response = await api.get<ApiResponse<Mechanic>>('/mechanics');
      return response.data.$values || [];
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Can not access mechanic: ' + message);
    }
  },
  createMechanic: async (data: { name: string; isAvailable?: boolean }) => {
    try {
      const response = await api.post<Mechanic>('/mechanics/create', data);
      return response.data;
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Error creating mechanic: ' + message);
    }
  },
  getTasks: async () => {
    try {
      const response = await api.get<ApiResponse<Task>>('/tasks');
      return response.data.$values || [];
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Dont have access to task: ' + message);
    }
  },
  createTask: async (data: { carId: number; description: string; mechanicId?: number | null }) => {
    try {
      const response = await api.post<Task>('/tasks/create', data);
      return response.data;
    } catch (error: unknown) {
      throw error; // Кидаємо помилку для обробки в компоненті
    }
  },
  createReport: async (data: { taskId: number; description: string }) => {
    try {
      const response = await api.post<Report>('/reports/create', data);
      return response.data;
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : JSON.stringify(error);
      throw new Error('Error creating report: ' + message);
    }
  },
};
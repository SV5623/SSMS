### 🌐 `frontend/README.md` – React + Vite

This is the frontend for the AutoSelect system, built using **React** with **Vite** for fast development.  
It allows users to view and interact with car service tasks, mechanics, and reports.

## 🛠️ Technologies

- React 18+
- Vite
- Axios
- React Router
- TailwindCSS (optional)
- TypeScript (if used)

## 📁 Project Structure
```
AutoSelect.Frontend/
├── public/
├── src/
│ ├── components/
│ ├── pages/
│ ├── services/
│ ├── App.jsx
│ └── main.jsx
├── index.html
└── vite.config.js
```

## ▶️ Running the Frontend

1. Install dependencies:
   ```bash
   npm install
   ```
    Start the development server:
   ```bash
   npm run dev
   ```
Visit the app at:

    http://localhost:5173

🔌 API Connection

The frontend expects the backend API to be available at:

const BASE_URL = "http://localhost:5000/api"; // or change in .env file

Make sure CORS is enabled in your backend!
📋 Features

    View list of cars and their tasks

    Assign mechanics

    View reports and parts

    UI designed for future expansion

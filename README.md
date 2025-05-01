# To-Do App

A full-stack To-Do application built with Vue.js and .NET Core, featuring a factory pattern implementation for selecting data providers, debounced search functionality, and complete CRUD operations.

## Features

- **Factory Pattern Implementation**: Choose between two different to-do providers (Entity Framework Database or In-Memory) directly from the UI
- **Smart Searching with Debounce**: Optimized search functionality to filter to-dos with debounce mechanism to prevent excessive server requests
- **Complete CRUD Operations**:
  - Add new to-dos
  - Update existing to-dos
  - Delete to-dos
  - Mark to-dos as completed
- **Responsive UI**: Clean and intuitive user interface

## Technologies Used

### Frontend
- Vue.js 3 (Composition API)
- HTML/CSS
- Axios for API requests

### Backend
- C# / .NET Core 7.0
- Entity Framework Core
- SQLite database
- REST API

## Project Structure

### Frontend (Vue.js)
```
/frontend
├── src/
│   ├── assets/
│   ├── components/
│   │   ├── TodoForm.vue
│   │   └── TodoItem.vue
│   ├── services/
│   │   └── todoService.js
│   ├── utils/
│   │   └── debounce.js
│   ├── App.vue
│   └── main.js
├── public/
└── package.json
```

### Backend (.NET Core)
```
/backend
├── Controllers/
│   └── TodosController.cs
├── Data/
│   └── TodoDbContext.cs
├── Models/
│   └── Todo.cs
├── Services/
│   ├── ITodoProvider.cs
│   ├── EntityFrameworkTodoProvider.cs
│   ├── InMemoryTodoProvider.cs
│   └── TodoProviderFactory.cs
├── Program.cs
└── appsettings.json
```

## Getting Started

### Prerequisites
- .NET 7.0 SDK
- Node.js and npm
- Vue CLI

### Backend Setup

1. Clone the repository
   ```bash
   git clone https://github.com/Krupa2071/ToDo_App
   cd todo-app/backend
   ```

2. Restore dependencies
   ```bash
   dotnet restore
   ```

3. Run the API
   ```bash
   dotnet run
   ```
   The API will be available at https://localhost:5204 or http://localhost:5000

### Frontend Setup

1. Navigate to the frontend directory
   ```bash
   cd ../frontend
   ```

2. Install dependencies
   ```bash
   npm install
   ```

3. Run the development server
   ```bash
   npm run serve
   ```
   The application will be available at http://localhost:8080

## Design Patterns Used

### Factory Pattern
The factory pattern is implemented in the backend services to create and manage different to-do providers:

- `ITodoProvider`: The interface that defines the contract for all to-do providers
- `EntityFrameworkTodoProvider`: Provider that uses Entity Framework and SQLite database
- `InMemoryTodoProvider`: Provider that uses in-memory storage
- `TodoProviderFactory`: Factory class that creates the appropriate provider based on the selected type

The UI allows users to switch between these providers seamlessly, demonstrating the value of the factory pattern in managing varying implementations.

### Debounce Pattern
Implemented in the search functionality to optimize performance by limiting how frequently API calls are made during typing:

```javascript
// Debounce utility
export function debounce(func, wait) {
  let timeout;
  
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
}
```

## Project Approach & Challenges

### Approach
My approach to this project was to first establish a clean architecture with clear separation of concerns:

1. Started with defining the backend models and interfaces
2. Implemented the factory pattern to handle different data providers
3. Created the RESTful API endpoints
4. Built the Vue.js frontend with a focus on user experience
5. Integrated the debounce mechanism for the search functionality
6. Implemented the CRUD operations on both frontend and backend

### Challenges
- **Factory Pattern Implementation**: Ensuring the factory pattern was correctly implemented while maintaining scalability for additional providers
- **State Management**: Managing the application state across components while ensuring real-time updates
- **Debounce Mechanism**: Fine-tuning the debounce timeout to balance responsiveness and performance

### Resources Used
- Microsoft documentation for .NET Core and Entity Framework
- Vue.js official documentation
- Factory pattern design references
- Stack Overflow for specific implementation challenges

## Future Improvements
- Add user authentication
- Implement to-do categories or tags
- Add due dates and priority levels for to-dos
- Enhance UI with animations and transitions
- Improve mobile responsiveness

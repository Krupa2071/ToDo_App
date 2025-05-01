<template>
  <div class="app-container">
    <!-- Sidebar -->
    <div class="sidebar" :class="{ 'collapsed': sidebarCollapsed }">
      <div class="sidebar-header">
        <span>🚀</span>
        <div class="sidebar-user">Krupa's List</div>
      </div>
      <div class="sidebar-menu">
        <div 
          class="sidebar-menu-item" 
          :class="{ 'active': activeView === 'all' }"
          @click="setView('all')"
        >
          <span class="sidebar-menu-item-icon">📋</span>
          <span>Tasks</span>
        </div>
        <div 
          class="sidebar-menu-item" 
          :class="{ 'active': activeView === 'completed' }"
          @click="setView('completed')"
        >
          <span class="sidebar-menu-item-icon">✅</span>
          <span>Completed</span>
        </div>
        <div 
          class="sidebar-menu-item" 
          :class="{ 'active': activeView === 'pending' }"
          @click="setView('pending')"
        >
          <span class="sidebar-menu-item-icon">⏱️</span>
          <span>Pending</span>
        </div>
        <div 
          class="sidebar-menu-item" 
          :class="{ 'active': activeView === 'tags' }"
          @click="setView('tags')"
        >
          <span class="sidebar-menu-item-icon">🏷️</span>
          <span>Tags</span>
        </div>
      </div>
    </div>

    <!-- Main Content -->
    <div class="main-content">
      <!-- Navbar -->
      <div class="navbar">
        <div class="navbar-left">
          <button @click="toggleSidebar" class="sidebar-toggle">
            <div class="hamburger-icon">
              <span></span>
              <span></span>
              <span></span>
            </div>
          </button>
          <div class="navbar-title">
            <span v-if="activeView === 'all'">📋 Tasks</span>
            <span v-else-if="activeView === 'completed'">✅ Completed Tasks</span>
            <span v-else-if="activeView === 'pending'">⏱️ Pending Tasks</span>
            <span v-else-if="activeView === 'tags'">🏷️ Tags</span>
          </div>
        </div>
        <div class="navbar-actions">
          <div class="provider-selector">
            <select id="provider" v-model="selectedProvider" @change="changeProvider">
              <option value="EntityFramework">Database Provider</option>
              <option value="InMemory">In-Memory Provider</option>
            </select>
          </div>
          <button @click="toggleTheme" class="theme-toggle">
            <span v-if="theme === 'light'">🌙</span>
            <span v-else>☀️</span>
          </button>
        </div>
      </div>

      <!-- Page Content -->
      <div class="page-content">
        <div class="search-container">
          <input 
            type="text" 
            v-model="searchTerm" 
            @input="handleSearchInput" 
            placeholder="Search tasks..." 
            class="search-input"
          />
        </div>

        <div v-if="activeView !== 'all'" class="active-filter">
          <span>
            Filtering: 
            <strong>{{ 
              activeView === 'completed' ? 'Completed Tasks' : 
              activeView === 'pending' ? 'Pending Tasks' : 
              'Tagged Tasks'
            }}</strong>
          </span>
          <button @click="clearFilter" class="clear-filter">Clear filter</button>
        </div>

        <!-- Todo Form -->
        <div class="todo-form">
          <div class="todo-form-header">Add New Task</div>
          <div class="todo-form-content">
            <form @submit.prevent="handleSubmit">
              <div class="form-group">
                <label for="title">Title</label>
                <input 
                  type="text" 
                  id="title" 
                  v-model="newTodo.title" 
                  required
                  placeholder="Task title"
                  class="form-control"
                />
              </div>
              
              <div class="form-group">
                <label for="description">Description</label>
                <textarea 
                  id="description" 
                  v-model="newTodo.description" 
                  placeholder="Add details about this task"
                  class="form-control"
                ></textarea>
              </div>
              
              <button type="submit" class="btn-submit">Add Task</button>
            </form>
          </div>
        </div>

        <!-- Todo List -->
        <div class="todos-container">
          <div v-if="loading" class="loading"></div>
          <template v-else>
            <div 
              v-for="todo in filteredTodos" 
              :key="todo.id" 
              class="todo-item"
              :class="{ 'completed': todo.isCompleted }"
            >
              <div v-if="!editingId || editingId !== todo.id" class="todo-content">
                <label class="checkbox-container">
                  <input 
                    type="checkbox" 
                    :checked="todo.isCompleted"
                    @change="toggleCompletion(todo)"
                  />
                  <span class="checkmark"></span>
                </label>
                <div class="todo-info">
                  <h3 class="todo-title">{{ todo.title }}</h3>
                  <p v-if="todo.description" class="todo-description">{{ todo.description }}</p>
                </div>
                <div class="todo-actions">
                  <button @click="startEditing(todo)" class="btn-action btn-edit">Edit</button>
                  <button @click="confirmDelete(todo.id)" class="btn-action btn-delete">Delete</button>
                </div>
              </div>

              <div v-else class="todo-edit-form">
                <div class="form-group">
                  <label for="edit-title">Title</label>
                  <input 
                    type="text" 
                    id="edit-title" 
                    v-model="editTodo.title" 
                    required
                    class="form-control"
                  />
                </div>
                
                <div class="form-group">
                  <label for="edit-description">Description</label>
                  <textarea 
                    id="edit-description" 
                    v-model="editTodo.description"
                    class="form-control"
                  ></textarea>
                </div>
                
                <div class="edit-actions">
                  <button @click="saveEdit" class="btn-save">Save</button>
                  <button @click="cancelEdit" class="btn-cancel">Cancel</button>
                </div>
              </div>
            </div>
            <p v-if="filteredTodos.length === 0" class="no-todos">
              {{ getEmptyStateMessage() }}
            </p>
          </template>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TodoService from './services/todoService'
import { debounce } from './utils/debounce'

export default {
  name: 'App',
  data() {
    return {
      todos: [],
      searchTerm: '',
      selectedProvider: 'EntityFramework',
      loading: false,
      theme: localStorage.getItem('theme') || 'light',
      sidebarCollapsed: localStorage.getItem('sidebarCollapsed') === 'true' || false,
      activeView: 'all',
      newTodo: {
        title: '',
        description: '',
        isCompleted: false
      },
      editingId: null,
      editTodo: {
        id: null,
        title: '',
        description: '',
        isCompleted: false
      }
    }
  },
  computed: {
    filteredTodos() {
      if (this.activeView === 'all') {
        return this.todos;
      } else if (this.activeView === 'completed') {
        return this.todos.filter(todo => todo.isCompleted);
      } else if (this.activeView === 'pending') {
        return this.todos.filter(todo => !todo.isCompleted);
      } else if (this.activeView === 'tags') {
        return this.todos.filter(todo => todo.title.includes('#') || todo.description.includes('#'));
      }
      return this.todos;
    }
  },
  created() {
    this.fetchTodos()
    this.debouncedSearch = debounce(this.searchTodos, 500)
    
    this.applyTheme()
  },
  methods: {
    async fetchTodos() {
      this.loading = true
      try {
        this.todos = await TodoService.getAllTodos()
      } catch (error) {
        console.error('Error fetching todos:', error)
      } finally {
        this.loading = false
      }
    },
    
    async searchTodos() {
      if (!this.searchTerm) {
        this.fetchTodos()
        return
      }
      
      this.loading = true
      try {
        this.todos = await TodoService.searchTodos(this.searchTerm)
      } catch (error) {
        console.error('Error searching todos:', error)
      } finally {
        this.loading = false
      }
    },
    
    handleSearchInput() {
      this.debouncedSearch()
    },
    
    handleSubmit() {
      this.addTodo(this.newTodo)
      
      // Reset form
      this.newTodo = {
        title: '',
        description: '',
        isCompleted: false
      }
    },
    
    async addTodo(todo) {
      try {
        const newTodo = await TodoService.createTodo(todo)
        this.todos.push(newTodo)
      } catch (error) {
        console.error('Error adding todo:', error)
      }
    },
    
    toggleCompletion(todo) {
      const updatedTodo = { ...todo, isCompleted: !todo.isCompleted }
      this.updateTodo(updatedTodo)
    },
    
    startEditing(todo) {
      this.editingId = todo.id
      this.editTodo = { ...todo }
    },
    
    saveEdit() {
      this.updateTodo(this.editTodo)
      this.editingId = null
    },
    
    cancelEdit() {
      this.editingId = null
    },
    
    async updateTodo(updatedTodo) {
      try {
        await TodoService.updateTodo(updatedTodo)
        const index = this.todos.findIndex(t => t.id === updatedTodo.id)
        if (index !== -1) {
          this.todos[index] = updatedTodo
        }
      } catch (error) {
        console.error('Error updating todo:', error)
      }
    },
    
    confirmDelete(id) {
      if (confirm('Are you sure you want to delete this task?')) {
        this.deleteTodo(id)
      }
    },
    
    async deleteTodo(id) {
      try {
        await TodoService.deleteTodo(id)
        this.todos = this.todos.filter(t => t.id !== id)
      } catch (error) {
        console.error('Error deleting todo:', error)
      }
    },
    
    async changeProvider() {
      try {
        await TodoService.setProvider(this.selectedProvider)
        this.fetchTodos()
      } catch (error) {
        console.error('Error changing provider:', error)
      }
    },
    
    toggleTheme() {
      this.theme = this.theme === 'light' ? 'dark' : 'light'
      this.applyTheme()
      localStorage.setItem('theme', this.theme)
    },
    
    applyTheme() {
      document.documentElement.setAttribute('data-theme', this.theme)
    },
    
    toggleSidebar() {
      this.sidebarCollapsed = !this.sidebarCollapsed
      localStorage.setItem('sidebarCollapsed', this.sidebarCollapsed)
    },
    
    setView(view) {
      this.activeView = view
    },
    
    clearFilter() {
      this.activeView = 'all'
    },
    
    getEmptyStateMessage() {
      if (this.activeView === 'all') {
        return 'No tasks found. Add one now!';
      } else if (this.activeView === 'completed') {
        return 'No completed tasks found.';
      } else if (this.activeView === 'pending') {
        return 'No pending tasks found.';
      } else if (this.activeView === 'tags') {
        return 'No tagged tasks found. Add tags using # in your task title or description.';
      }
      return 'No tasks found.';
    }
  }
}
</script>